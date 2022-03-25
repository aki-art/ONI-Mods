using AETNTweaks.Components;
using FUtility;
using UnityEngine;

namespace AETNTweaks.Buildings.PyrositePylon
{
    public class Pyrosite : StateMachineComponent<Pyrosite.SMInstance>
    {
        [SerializeField]
        public float activeDuration;

        public static Operational.Flag flag = new Operational.Flag("recentlyReceivedPulse", Operational.Flag.Type.Functional);

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

        public class States : GameStateMachine<States, SMInstance, Pyrosite>
        {
            public State off;
            public State on;

            public TargetParameter parentAETN;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = off;

                root
                    .InitializeOperationalFlag(flag, false)
                    .EventHandler((GameHashes)ModHashes.AETNTweaks_StartPulse, (smi, data) =>
                     {
                         if (data is PulseController controller)
                         {
                             smi.sm.parentAETN.Set(controller, smi);
                         }

                         smi.GoTo(smi.sm.on);
                     });

                off
#if DEBUG 
                    .ToggleStatusItem("Off", "");
#endif

                on
#if DEBUG 
                    .ToggleStatusItem("On", "")
#endif
                    .Enter(smi =>
                    {
                        var parentAetn = smi.sm.parentAETN.Get(smi);
                        if(parentAetn != null)
                        {
                            smi.tether.enabled = true;
                            smi.tether.SetEnds(smi.sm.parentAETN.Get(smi).transform, smi.transform);
                        }
                    })
                    //.OnTargetLost(parentAETN, off)
                    .ScheduleGoTo(smi => smi.master.activeDuration, off)
                    .ToggleOperationalFlag(flag);
            }
        }
        public class SMInstance : GameStateMachine<States, SMInstance, Pyrosite, object>.GameInstance
        {
            public Tether tether;

            public SMInstance(Pyrosite master) : base(master)
            {
                tether = master.GetComponent<Tether>();
            }
        }
    }
}
