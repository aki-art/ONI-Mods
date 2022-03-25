using AETNTweaks.Buildings.PyrositePylon;
using FUtility;
using KSerialization;
using UnityEngine;

namespace AETNTweaks.Components
{
    public class PulseController : StateMachineComponent<PulseController.SMInstance>
    {
        [SerializeField]
        public float pulseFrequency;

        [SerializeField]
        public int range;

        [MyCmpReq]
        private PulseVisualizer visualizer;

        private Extents extents;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
            extents = new Extents(this.NaturalBuildingCell(), range);
        }

        // TODO: instead of iterating area, i should make the attached pyrosites subscribe to this
        private void Pulse()
        {
            var list = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
            GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, list);

            foreach (ScenePartitionerEntry scenePartitionerEntry in list)
            {
                if (scenePartitionerEntry.obj is BuildingComplete go && go.TryGetComponent(out Pyrosite pyrosite))
                {
                    Log.Debuglog("pyrosite pulse sent");
                    pyrosite.Trigger((int)ModHashes.AETNTweaks_StartPulse, this);
                }
            }

            list.Recycle();

            visualizer.ShowFx();
        }

        public class States : GameStateMachine<States, SMInstance, PulseController>
        {
            public State dormant;
            public State pulsing;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = dormant;

                // TODO: is there a better way to just send a periodic signal? :thonk:
                dormant
#if DEBUG
                    .ToggleStatusItem("Dormant", "")
#endif
                    .ToggleComponent<PulseVisualizer>(true)
                    .ScheduleGoTo(smi => smi.master.pulseFrequency, pulsing);

                pulsing
#if DEBUG
                    .ToggleStatusItem("Pulsing", "")
#endif
                    .Enter(smi => smi.master.Pulse())
                    .GoTo(dormant); // finish animation first
            }
        }

        [SerializationConfig(MemberSerialization.OptIn)]
        public class SMInstance : GameStateMachine<States, SMInstance, PulseController, object>.GameInstance
        {

            public SMInstance(PulseController master) : base(master)
            {
            }
        }
    }
}
