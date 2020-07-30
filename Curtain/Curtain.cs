using KSerialization;
using System;

namespace Curtain
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public partial class Curtain : Workable, ISaveLoadable
    {
        [MyCmpReq]
        public Building building;
        [Serialize]
        public ControlState CurrentState { get; set; }
        [Serialize]
        public ControlState RequestedState { get; set; }
        [Serialize]
        public State reqState { get; set; }
        [Serialize]
        public State state { get; set; }
        private WorkChore<Curtain> changeStateChore;
        private readonly KAnimFile[] anims = new KAnimFile[] { Assets.GetAnim("anim_use_remote_kanim") };

        public Curtain()
        {
            SetOffsetTable(OffsetGroups.InvertedStandardTable);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            overrideAnims = anims;
            synchronizeAnims = false;
        }

        protected override void OnSpawn()
        {
            controller = new Controller.Instance(this);
            controller.StartSM();

            SetDefaultCellFlags();
            UpdateState();
            StartPartitioner();
        }

        public bool IsOpen() => state.HasFlag(State.Passable | State.Permeable);
        public bool IsClosed() => state.HasFlag(State.Passable) && !state.HasFlag(State.Permeable);
        public bool IsLocked() => !(state.HasFlag(State.Passable) || !state.HasFlag(State.Permeable));

        private void Open()
        {
            CurrentState = ControlState.Open;
            state |= State.Passable | State.Permeable;

            foreach (int cell in building.PlacementCells)
                Dig(cell);
            SetControllerState();

        }

        private void Close()
        {
            CurrentState = ControlState.Auto;
            state |= State.Passable;
            state &= ~State.Permeable;

            DisplaceElement();
            SetControllerState();
        }

        private void Lock()
        {
            CurrentState = ControlState.Locked;
            state &= ~(State.Passable | State.Permeable);

            DisplaceElement();
            SetControllerState();
        }

        private void SetControllerState()
        {
            controller.sm.isOpen.Set(IsOpen(), controller);
            controller.sm.isClosed.Set(IsClosed(), controller);
            controller.sm.isLocked.Set(IsLocked(), controller);
        }

        protected override void OnCleanUp()
        {
            foreach (int cell in building.PlacementCells)
            {
                CleanSim(cell);
                CleanPassable(cell);
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
            GetComponent<KSelectable>().RemoveStatusItem(ModAssets.ChangeCurtainControlState);
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
            }
            else
            {
                GetComponent<KSelectable>().AddStatusItem(ModAssets.ChangeCurtainControlState, this);

                changeStateChore = new WorkChore<Curtain>(
                    chore_type: Db.Get().ChoreTypes.Toggle,
                    target: this,
                    chore_provider: null,
                    run_until_complete: true,
                    on_complete: null,
                    on_begin: null,
                    on_end: null,
                    allow_in_red_alert: true,
                    schedule_block: null,
                    ignore_schedule_block: false,
                    only_when_operational: false);
            }
        }

        public enum ControlState
        {
            Open,
            Auto,
            Locked
        }

        [Flags]
        public enum State
        {
            Permeable = 1,
            Passable = 2,
            PassingLeft = 4
        }
    }
}
