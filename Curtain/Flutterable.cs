using KSerialization;
using System.Linq;

namespace Curtain
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Flutterable : StateMachineComponent<Flutterable.StatesInstance>
    {
        private HandleVector<int>.Handle pickupablesChangedEntry;
        [MyCmpReq] Curtain curtain;
        [MyCmpReq] Building building;
        private Extents pickupableExtents;
        public bool passingLeft;

        [Serialize]
        public bool Listening { get; set; } = false;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
            StartPartitioner();
        }

        private void StartPartitioner()
        {
            pickupableExtents = Extents.OneCell(building.PlacementCells[0]);
            pickupablesChangedEntry = GameScenePartitioner.Instance.Add("Curtain.PickupablesChanged", gameObject, pickupableExtents, GameScenePartitioner.Instance.pickupablesChangedLayer, OnPickupablesChanged);
        }

        private void OnPickupablesChanged(object obj)
        {
            if (!Listening) return;

            Pickupable p = obj as Pickupable;
            if (p && IsDupe(p) && Waiting)
                UpdateMovement(p);
        }

        private void UpdateMovement(Pickupable dupe)
        {
            var navigator = dupe.GetComponent<Navigator>();
            if (navigator != null && navigator.IsMoving())
            {
                if (navigator.GetNextTransition().startAxis == NavAxis.X)
                {
                    passingLeft = navigator.GetNextTransition().x > 0;
                    Trigger((int)GameHashes.WalkBy, this); 
                }
            }
            else
                smi.GoTo(smi.sm.idlingInside);
        }

        public bool IsDupeStandingHere()
        {
            var pooledList = GatherEntries();
            foreach (var entry in pooledList.Select(e => e.obj as Pickupable))
            {
                if (IsDupe(entry))
                    return true;
            }

            return false;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            if (smi.IsRunning())
            {
                smi.StopSM("cleanup");
            }
            GameScenePartitioner.Instance.Free(ref pickupablesChangedEntry);
        }

        public void SetInactive()
        {
            smi.GoTo(smi.sm.inactive);
        }

        private bool IsDupe(Pickupable pickupable) => pickupable.KPrefabID.HasTag(GameTags.DupeBrain);
        private bool Waiting => smi.IsInsideState(smi.sm.waiting);

        private ListPool<ScenePartitionerEntry, Curtain>.PooledList GatherEntries()
        {
            var pooledList = ListPool<ScenePartitionerEntry, Curtain>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(pickupableExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
            return pooledList;
        }

        public class States : GameStateMachine<States, StatesInstance, Flutterable>
        {
            public State passing;
            public State idlingInside;
            public State waiting;
            public State inactive;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = waiting;

                waiting
                    .EventTransition(GameHashes.WalkBy, passing);
                passing
                    .Enter(smi => smi.master.curtain.Flutter())
                    .ScheduleGoTo(.5f, idlingInside);
                idlingInside
                    .Transition(waiting, Not(smi => smi.master.IsDupeStandingHere()), UpdateRate.RENDER_200ms)
                    .Exit(smi => smi.master.curtain.OnPassedBy());
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, Flutterable, object>.GameInstance
        {
            public StatesInstance(Flutterable smi) : base(smi) {  }
        }
    }
}
