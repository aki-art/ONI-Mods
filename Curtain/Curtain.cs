using Harmony;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Curtain
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public partial class Curtain : Workable, ISaveLoadable, ISim200ms
    {
        [MyCmpReq]
        public Building building;
        [MyCmpReq]
        KSelectable kSelectable;
        [Serialize]
        public ControlState CurrentState { get; set; }
        [Serialize]
        public ControlState RequestedState { get; set; }
#pragma warning disable IDE0052
        private WorkChore<Curtain> changeStateChore;
#pragma warning restore IDE0052 
        private bool meltCheck;
        public Flutterable flutterable;

        private Dictionary<string, string> soundEvents;

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
            soundEvents = new Dictionary<string, string>
            {
                { "swoosh", GlobalAssets.GetSound("drecko_ruffle_scales_short") },
                { "open", GlobalAssets.GetSound("sauna_door_open") },
                { "close", GlobalAssets.GetSound("sauna_door_close") },
                { "lock", GlobalAssets.GetSound("dupe_scratch_single") },
                { "unlock", GlobalAssets.GetSound("jobstation_job_grab") }
            };

            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            flutterable = FindOrAdd<Flutterable>();
            flutterable.Listening = false;

            controller = new Controller.Instance(this);
            controller.StartSM();

            SetDefaultCellFlags();
            UpdateState();
            RequestedState = CurrentState;
            ApplyRequestedControlState();

            StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
            HandleVector<int>.Handle handle = structureTemperatures.GetHandle(gameObject);
            structureTemperatures.Bypass(handle);

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
            PlaySoundEffect("swoosh", 4);
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

            foreach (int cell in building.PlacementCells)
            {
                //SetCellPassable(cell, true, false);
                CleanSim(cell);
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
            {
                changeStateChore.Cancel("");
            }

            changeStateChore = null;
            CurrentState = RequestedState;
            UpdateState();
            kSelectable.RemoveStatusItem(ModAssets.CurtainStatus);
            Trigger((int)GameHashes.DoorStateChanged, this);
        }

        public void QueueStateChange(ControlState state)
        {
            RequestedState = state;
            if (state == CurrentState)
            {
                if(changeStateChore != null)
                {
                    changeStateChore.Cancel("");
                    changeStateChore = null;
                }

                kSelectable.RemoveStatusItem(ModAssets.CurtainStatus, true);
                return;
            };

            if (DebugHandler.InstantBuildMode)
            {
                if (changeStateChore != null)
                {
                    changeStateChore.Cancel("Debug state change");
                }

                ApplyRequestedControlState();
                return;
            }

            kSelectable.AddStatusItem(ModAssets.CurtainStatus, this);

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
                //SimMessages.SetInsulation(cell, GetComponent<PrimaryElement>().Element.thermalConductivity * 0.25f);
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
            if (Grid.IsValidCell(cell))
                Grid.Foundation[cell] = false;
            SimMessages.ClearCellProperties(cell, 12);
            Grid.RenderedByWorld[cell] = RenderedByWorld(cell);
            SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0);
            //SimMessages.Dig(cell);
           // Grid.CritterImpassable[cell] = false;
            //Grid.DupeImpassable[cell] = false;
            Pathfinding.Instance.AddDirtyNavGridCell(cell);
            Grid.HasDoor[cell] = false;
            Grid.HasAccessDoor[cell] = false;
            Game.Instance.SetDupePassableSolid(cell, false, Grid.Solid[cell]);
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

        private void PlaySoundEffect(string sound, float vol)
        {
            SoundEvent.EndOneShot(
                SoundEvent.BeginOneShot(
                    soundEvents[sound], 
                    transform.position,
                    vol, 
                    SoundEvent.ObjectIsSelectedAndVisible(gameObject)));
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
            //Grid.DupePassable[cell] = passable;
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