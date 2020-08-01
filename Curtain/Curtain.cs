using Harmony;
using KSerialization;
using UnityEngine;
using System.Linq;

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
        private HandleVector<int>.Handle pickupablesChangedEntry;
        private Extents pickupableExtents;
        private bool passedDirty;
        private bool passingLeft;

        private readonly KAnimFile[] anims;

        public Curtain()
        {
            SetOffsetTable(OffsetGroups.InvertedStandardTable);
            anims = new KAnimFile[]
            {
                Assets.GetAnim("anim_use_remote_kanim")
            }; 
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
            controller = new Controller.Instance(this);
            controller.StartSM();

            SetDefaultCellFlags();
            UpdateState();
            RequestedState = CurrentState;
            ApplyRequestedControlState();
            StartPartitioner();
        }

        private void Open()
        {
            CurrentState = ControlState.Open;
            foreach (int cell in building.PlacementCells)
                Dig(cell);
            UpdateController();
        }

        private void Close()
        {
            CurrentState = ControlState.Auto;
            DisplaceElement();
            UpdateController();
        }

        private void Lock()
        {
            CurrentState = ControlState.Locked;
            DisplaceElement();
            UpdateController();
        }

        protected override void OnCleanUp()
        {
            foreach (int cell in building.PlacementCells)
            {
                CleanSim(cell);
                SetCellPassable(cell, false, false);
            }

            GameScenePartitioner.Instance.Free(ref pickupablesChangedEntry);
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
                CurrentState = RequestedState;
                UpdateState();
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

            // Allowing curtains to have their settings copied to doors
            var door = ((GameObject)obj).GetComponent<Door>();
            if (door != null)
                QueueStateChange((ControlState)door.RequestedState);
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
            Grid.HasDoor[cell] = false;
            Grid.HasAccessDoor[cell] = false;

            if (Grid.Element[cell].IsSolid)
                SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0);
        }

        private void DisplaceElement()
        {
            var pe = GetComponent<PrimaryElement>();
            float mass = pe.Mass / 2;

            foreach (int cell in building.PlacementCells)
            {
                HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimDoorClosed));
                SimMessages.ReplaceAndDisplaceElement(cell, pe.ElementID, CellEventLogger.Instance.DoorClose, mass, pe.Temperature, byte.MaxValue, 0, handle.index);
                SimMessages.SetCellProperties(cell, (byte)Sim.Cell.Properties.Transparent);
                World.Instance.groundRenderer.MarkDirty(cell);
            }
        }

        private void Dig(int cell)
        {
            var item = new Game.CallbackInfo(OnSimDoorOpened);
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

        private void OnSimDoorOpened()
        {
            ByPassTemp(false);
            meltCheck = false;
        }

        private void OnSimDoorClosed()
        {
            ByPassTemp(true);
            meltCheck = true;
        }

        private void ByPassTemp(bool bypass)
        {
            var structureTemperatures = GameComps.StructureTemperatures;
            var handle = structureTemperatures.GetHandle(gameObject);

            if (bypass)
                structureTemperatures.Bypass(handle);
            else
                structureTemperatures.UnBypass(handle);
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


        private void StartPartitioner()
        {
            pickupableExtents = Extents.OneCell(building.PlacementCells[0]);

            pickupablesChangedEntry = GameScenePartitioner.Instance.Add(
               "Curtain.PickupablesChanged",
               gameObject,
               pickupableExtents,
               GameScenePartitioner.Instance.pickupablesChangedLayer, OnPickupablesChanged);

            passedDirty = true;
        }

        private void OnPickupablesChanged(object data)
        {
            Pickupable p = data as Pickupable;
            if (p && IsDupe(p))
                passedDirty = true;
        }

        internal void CheckDupePassing()
        {
            if (!passedDirty) return;
            var pooledList = GatherEntries();

            foreach (var entry in pooledList.Select(e => e.obj as Pickupable))
            {
                if (IsDupe(entry))
                {
                    UpdateMovement(entry.GetComponent<Navigator>());
                    return;
                }
            }

            passedDirty = false;
        }

        private void UpdateMovement(Navigator navigator)
        {
            if (navigator.IsMoving())
            {
                passingLeft = navigator.GetNextTransition().x > 0;
                Trigger((int)GameHashes.WalkBy, this);
            }

            passedDirty = false;
        }

        private ListPool<ScenePartitionerEntry, Curtain>.PooledList GatherEntries()
        {
            var pooledList = ListPool<ScenePartitionerEntry, Curtain>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(pickupableExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
            return pooledList;
        }

        private bool IsDupe(Pickupable pickupable) => pickupable.KPrefabID.HasTag(GameTags.DupeBrain);

        public enum ControlState
        {
            Auto,
            Open,
            Locked
        }
    }
}
