using KSerialization;

namespace Curtain
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Curtain : Workable, ISaveLoadable//, ISim200ms
    {
        [MyCmpReq]
        public Building building;
        [Serialize]
        public bool Permeable { get; set; }
        [Serialize]
        public bool Passable { get; set; }
        [Serialize]
        public ControlState CurrentState { get; set; }
        [Serialize]
        public ControlState RequestedState { get; set; }
        private Chore changeStateChore;
        private static readonly KAnimFile[] anims = new KAnimFile[] { 
            Assets.GetAnim("anim_use_remote_kanim") 
        };
        public Curtain()
        {
            SetOffsetTable(OffsetGroups.InvertedStandardTable);
        }

        public enum ControlState
        {
            Open,
            Auto,
            Locked
        }
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            overrideAnims = anims;
            synchronizeAnims = false;
            SetWorkTime(3f);
        }

        protected override void OnSpawn()
        {
            UpdateSimState();
            UpdatePassableState();

            foreach (int cell in building.PlacementCells)
            {
                Grid.FakeFloor[cell] = true;
                Grid.HasDoor[cell] = true;
            }
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
            UpdateSimState();
            UpdatePassableState();
            GetComponent<KSelectable>().RemoveStatusItem(ModAssets.ChangeCurtainControlState);
            Trigger((int)GameHashes.DoorStateChanged, this);
        }

        public void QueueStateChange(ControlState state)
        {
            Debug.Log("statechange to " + state);
            RequestedState = state;
            if (state == CurrentState) return;

            if (DebugHandler.InstantBuildMode)
            {
                CurrentState = RequestedState;
                UpdateSimState();
                UpdatePassableState();
                return;
            }

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
                only_when_operational: false );

        }

        private void UpdateSimState()
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
        }

        private void UpdatePassableState()
        {
            Passable = CurrentState != ControlState.Locked;
            foreach (int cell in building.PlacementCells)
            {
                Game.Instance.SetDupePassableSolid(cell, Passable, true);
                Grid.DupeImpassable[cell] = !Passable;
                Grid.DupePassable[cell] = Passable;
                Pathfinding.Instance.AddDirtyNavGridCell(cell);
            }
        }

        private void Open()
        {
            CurrentState = ControlState.Open;
            foreach (int cell in building.PlacementCells)
            {
                SimMessages.Dig(cell, Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimDoorOpened)).index, true);
            }
        }

        private void Close()
        {
            CurrentState = ControlState.Auto;
            DisplaceElement();
        }

        private void Lock()
        {
            CurrentState = ControlState.Locked;
            DisplaceElement();
        }

        private void DisplaceElement()
        {
            var pe = GetComponent<PrimaryElement>();
            float mass = pe.Mass / 2;

            foreach (int cell in building.PlacementCells)
            {
                var handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(OnSimDoorClosed));
                SimMessages.ReplaceAndDisplaceElement(cell, pe.ElementID, CellEventLogger.Instance.DoorClose, mass, pe.Temperature, byte.MaxValue, 0, handle.index);

                World.Instance.groundRenderer.MarkDirty(cell);
            }
        }

        private void OnSimDoorOpened()
        {
            StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
            HandleVector<int>.Handle handle = structureTemperatures.GetHandle(gameObject);
            structureTemperatures.UnBypass(handle);
        }

        private void OnSimDoorClosed()
        {
            StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
            HandleVector<int>.Handle handle = structureTemperatures.GetHandle(gameObject);
            structureTemperatures.Bypass(handle);
        }


        public class Controller : GameStateMachine<Controller, Controller.Instance, Curtain>
        {
            public State closed;
            public override void InitializeStates(out BaseState default_state)
            {
                serializable = true;
                default_state = closed;
            }
            public new class Instance : GameInstance
            {
                public Instance(Curtain curtain) : base(curtain)
                {
                }
            }
        }
    }
}
