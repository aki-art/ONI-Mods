using Harmony;
using KSerialization;
using UnityEngine;

namespace Curtain
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public partial class Curtain : Workable, ISaveLoadable, ISim200ms
    {
        [MyCmpReq]
        public Building building;
        [Serialize]
        public ControlState CurrentState { get; set; }
        [Serialize]
        public ControlState RequestedState { get; set; }
#pragma warning disable IDE0052
        private WorkChore<Curtain> changeStateChore;
#pragma warning restore IDE0052 
        private bool meltCheck;
        public Flutterable flutterable;
        private string symbolPrefix;

        private readonly KAnimFile[] anims = new KAnimFile[]
        {
            Assets.GetAnim("anim_use_remote_kanim")
        };

        public Curtain()
        {
            SetOffsetTable(OffsetGroups.InvertedStandardTable);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            overrideAnims = anims;
            synchronizeAnims = false;
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            flutterable = gameObject.AddComponent<Flutterable>();
            flutterable.Listening = false;

            symbolPrefix = GetAnimPrefix();

            controller = new Controller.Instance(this, GetAnimPrefix());
            controller.StartSM();

            SetDefaultCellFlags();
            UpdateState();
            RequestedState = CurrentState;
            ApplyRequestedControlState();

            StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
            HandleVector<int>.Handle handle = structureTemperatures.GetHandle(gameObject);
            structureTemperatures.Bypass(handle);

        }

        private string GetAnimPrefix()
        {
            SimHashes simHash = GetComponent<PrimaryElement>().ElementID;
            string color = "";
            switch (simHash)
            {
                case SimHashes.SuperInsulator:
                    color = "pink_";
                    break;
                case SimHashes.SolidViscoGel:
                    color = "purple_";
                    break;
                case SimHashes.Isoresin:
                    color = "yellow_";
                    break;
            }

            return color;
        }


        public void Open(bool updateControlState = true)
        {
            foreach (int cell in building.PlacementCells)
                Dig(cell);

            if (updateControlState)
            {
                CurrentState = ControlState.Open;
                UpdateController();
            }
        }

        public void Close()
        {
            CurrentState = ControlState.Auto;
            DisplaceElement();
            UpdateController();
        }

        public void Lock()
        {
            CurrentState = ControlState.Locked;
            DisplaceElement();
            UpdateController();
        }

        public void Flutter()
        {
            flutterable.Listening = false;
            controller.GoTo(controller.sm.passing);
        }

        public void OnPassedBy()
        {
            Close();
            controller.GoTo(controller.sm.closed);
        }

        protected override void OnCleanUp()
        {
            controller.GoTo(controller.sm.passingWaiting);
            controller.StopSM("");
            flutterable.Listening = false;
            flutterable.SetInactive();

            foreach (int cell in building.PlacementCells)
            {
                CleanSim(cell);
                SetCellPassable(cell, true, false);
            }

            changeStateChore = null;
            base.OnCleanUp();
        }

        protected override void OnCompleteWork(Worker worker)
        {
            base.OnCompleteWork(worker);
            changeStateChore = null;
            ApplyRequestedControlState();
        }

        private void ApplyRequestedControlState()
        {
            if (changeStateChore != null)
                changeStateChore.Cancel("");
            changeStateChore = null;
            CurrentState = RequestedState;
            UpdateState();
            GetComponent<KSelectable>().RemoveStatusItem(ModAssets.CurtainStatus);
            Trigger((int)GameHashes.DoorStateChanged, this);
        }

        public void QueueStateChange(ControlState state)
        {
            RequestedState = state;
            if (state == CurrentState) return;

            if (DebugHandler.InstantBuildMode)
            {
                if (changeStateChore != null)
                {
                    changeStateChore.Cancel("Debug state change");
                }
                ApplyRequestedControlState();
                return;
            }

            GetComponent<KSelectable>().AddStatusItem(ModAssets.CurtainStatus, this);

            changeStateChore = new WorkChore<Curtain>(
                chore_type: Db.Get().ChoreTypes.Toggle,
                target: this,
                only_when_operational: false);
        }

        private void OnCopySettings(object obj)
        {
            var curtain = ((GameObject)obj).GetComponent<Curtain>();
            if (curtain != null)
            {
                QueueStateChange(curtain.RequestedState);
                return;
            }
        }

        public void Sim200ms(float dt)
        {
            if (meltCheck)
            {
                var structureTemperatures = GameComps.StructureTemperatures;
                var handle = structureTemperatures.GetHandle(gameObject);
                if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
                {
                    foreach (int cell in building.PlacementCells)
                    {
                        if (!Grid.Solid[cell])
                        {
                            Util.KDestroyGameObject(this);
                            return;
                        }
                    }
                }
            }
        }

        private void UpdateState()
        {
            switch (CurrentState)
            {
                case ControlState.Open:
                    Open();
                    break;
                case ControlState.Locked:
                    Lock();
                    break;
                case ControlState.Auto:
                default:
                    Close();
                    break;
            }

            UpdatePassable();
        }

        private void SetDefaultCellFlags()
        {
            foreach (int cell in building.PlacementCells)
            {
                Grid.FakeFloor[cell] = IsTopCell(cell);
                Grid.HasDoor[cell] = true;
                Grid.RenderedByWorld[cell] = false;
                SimMessages.ClearCellProperties(cell, (byte)Sim.Cell.Properties.Unbreakable);
                SimMessages.SetInsulation(cell, GetComponent<PrimaryElement>().Element.thermalConductivity * 0.25f);
            }
        }

        private void UpdatePassable()
        {
            bool passable = CurrentState != ControlState.Locked;
            bool permeable = CurrentState == ControlState.Open;

            foreach (int cell in building.PlacementCells)
                SetCellPassable(cell, passable, permeable);
        }

        private static void CleanSim(int cell)
        {
            SimMessages.ClearCellProperties(cell, 12);
            Grid.RenderedByWorld[cell] = RenderedByWorld(cell);
            Grid.FakeFloor[cell] = false;
            Grid.Foundation[cell] = false;
            Grid.HasDoor[cell] = false;
            Grid.HasAccessDoor[cell] = false;
            SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0);
            Grid.CritterImpassable[cell] = false;
            Grid.DupeImpassable[cell] = false;
            Pathfinding.Instance.AddDirtyNavGridCell(cell);
        }

        private void DisplaceElement()
        {
            var pe = GetComponent<PrimaryElement>();
            float mass = pe.Mass / 2;

            foreach (int cell in building.PlacementCells)
            {
                var item = new Game.CallbackInfo(() => meltCheck = true);
                var cb = Game.Instance.callbackManager.Add(item);
                SimMessages.ReplaceAndDisplaceElement(cell, pe.ElementID, CellEventLogger.Instance.DoorClose, mass, pe.Temperature, byte.MaxValue, 0, cb.index);
                var flags = Sim.Cell.Properties.LiquidImpermeable | Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.Transparent;
                SimMessages.ClearCellProperties(cell, (byte)flags);
                World.Instance.groundRenderer.MarkDirty(cell);
            }
        }

        private void Dig(int cell)
        {
            var item = new Game.CallbackInfo(() => meltCheck = false);
            var cb = Game.Instance.callbackManager.Add(item);
            SimMessages.Dig(cell, cb.index, true);
        }

        private static void SetCellPassable(int cell, bool passable, bool permeable)
        {
            Game.Instance.SetDupePassableSolid(cell, passable, !permeable);
            Grid.DupeImpassable[cell] = !passable;
            Grid.DupePassable[cell] = passable;
            Pathfinding.Instance.AddDirtyNavGridCell(cell);
        }

        private void UpdateController()
        {
            controller.sm.isOpen.Set(CurrentState == ControlState.Open, controller);
            controller.sm.isClosed.Set(CurrentState == ControlState.Auto, controller);
            controller.sm.isLocked.Set(CurrentState == ControlState.Locked, controller);
        }

        private static bool RenderedByWorld(int cell)
        {
            var substance = Traverse.Create(Grid.Element[cell].substance);
            return substance.Field("renderedByWorld").GetValue<bool>();
        }

        private bool IsTopCell(int cell)
        {
            return !(building.GetExtents().Contains(Grid.CellToXY(Grid.CellAbove(cell))));
        }

        public enum ControlState
        {
            Auto,
            Open,
            Locked
        }
    }
}