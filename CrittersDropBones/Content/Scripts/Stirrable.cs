using KSerialization;
using System;
using UnityEngine;

namespace CrittersDropBones.Content.Scripts
{
    public class Stirrable : StateMachineComponent<Stirrable.StatesInstance>
    {
        [MyCmpGet]
        public Operational operational;

        [MyCmpGet]
        public ComplexFabricator fabricator;

        [Serialize]
        public float elapsedSinceLastStir = 0;

        [SerializeField]
        public float stirIntervalSeconds;

        public static readonly Operational.Flag operationalFlag = new("CrittersDropBones_CookingPot_Flag", Operational.Flag.Type.Requirement);

        public override void OnSpawn()
        {
            base.OnSpawn();
            operational.SetFlag(operationalFlag, true);

            smi.StartSM();
            smi.sm.stirrableWorkable.Set(GetComponent<StirrableWorkable>(), smi);
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, Stirrable, object>.GameInstance
        {
            public Operational operational;
            public ComplexFabricator fabricator;

            public StatesInstance(Stirrable master) : base(master)
            {
                operational = master.operational;
                fabricator = master.fabricator;
            }

            public Chore CreateChore()
            {
                return new WorkChore<StirrableWorkable>(Db.Get().ChoreTypes.Cook, smi.master, only_when_operational: false);
            }
        }

        public class States : GameStateMachine<States, StatesInstance, Stirrable>
        {
            public State off;
            public ActiveState active;
            public TargetParameter stirrableWorkable;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = off;

                off
                    .ToggleOperationalFlag(operationalFlag)
                    .EventTransition(GameHashes.FabricatorOrdersUpdated, active, HasWorkingOrder);

                active
                    .Enter(AskForStir)
                    .DefaultState(active.stirring)
                    .EventTransition(GameHashes.FabricatorOrdersUpdated, off, smi => !HasWorkingOrder(smi));

                active.satisfied
                    .PlayAnim("working_loop", KAnim.PlayMode.Loop)
                    .ToggleOperationalFlag(operationalFlag)
                    .UpdateTransition(active.stirring, NeedsStirring, UpdateRate.RENDER_1000ms);

                active.stirring
                    .PlayAnim("working_pre")
                    .QueueAnim("stirring", true)
                    .ToggleChore(smi => smi.CreateChore(), active.stirringPst)
                    .Exit(smi => smi.master.elapsedSinceLastStir = 0);

                active.stirringPst
                    .PlayAnim("working_pst")
                    .OnAnimQueueComplete(active.satisfied);
            }

            private bool NeedsStirring(StatesInstance smi, float dt)
            {
                Log.Debuglog("dt: " + dt);
                smi.master.elapsedSinceLastStir += dt;
                return smi.master.elapsedSinceLastStir > smi.master.stirIntervalSeconds;
            }

            private void AskForStir(StatesInstance smi)
            {
                smi.master.elapsedSinceLastStir = 0;
            }

            private bool HasWorkingOrder(StatesInstance smi) => smi.fabricator.HasWorkingOrder;

            public class ActiveState : State
            {
                public State satisfied;
                public State stirringPre;
                public State stirring;
                public State stirringPst;
                public State paused;
            }
        }
    }
}
