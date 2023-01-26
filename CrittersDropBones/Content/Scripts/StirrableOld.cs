using KSerialization;
using UnityEngine;

namespace CrittersDropBones.Content.Scripts
{
    public class StirrableOld : StateMachineComponent<StirrableOld.StatesInstance>
    {
        [MyCmpGet]
        private Operational operational;

        [MyCmpGet]
        private ComplexFabricator fabricator;

        [Serialize]
        public float elapsedSinceLastStir = 0;

        [SerializeField]
        public float stirIntervalSeconds = 10f;

        public static readonly Operational.Flag operationalFlag = new Operational.Flag("CrittersDropBones_CookingPot_Flag", Operational.Flag.Type.Requirement);

        public override void OnSpawn()
        {
            base.OnSpawn();
            operational.SetFlag(operationalFlag, false);

            smi.StartSM();
        }

        public void SetWorker(Worker worker)
        {
            smi.sm.worker.Set(worker, smi);
        }

        public void CompleteStir()
        {
            elapsedSinceLastStir = 0;
            smi.sm.stirComplete.Trigger(smi);
            SetWorker(null);
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, StirrableOld, object>.GameInstance
        {
            public Operational operational;

            public StatesInstance(StirrableOld master) : base(master)
            {
                if(master.TryGetComponent(out Operational operational))
                {
                    this.operational = operational;
                }
            }

            public void ResetWorkable()
            {
                var workable = master.GetComponent<StirrableWorkable>();
                workable.ShowProgressBar(true);
                workable.WorkTimeRemaining = workable.GetWorkTime();
            }

            internal Chore CreateChore()
            {
                return new WorkChore<StirrableWorkable>(
                    Db.Get().ChoreTypes.Cook,
                    smi.master,
                    on_complete: OnCompleteChore,
                    only_when_operational: false);
            }

            private void OnCompleteChore(Chore obj)
            {
                smi.master.elapsedSinceLastStir = 0;
                smi.sm.stirComplete.Trigger(smi);
            }

            internal bool NeedsStirring(float dt)
            {
                master.elapsedSinceLastStir += dt;
                if (!smi.master.operational.IsOperational || smi.master.fabricator.CurrentWorkingOrder is null)
                {
                    return false;
                }

                return master.elapsedSinceLastStir >= master.stirIntervalSeconds;
            }
        }

        public class States : GameStateMachine<States, StatesInstance, StirrableOld>
        {
            public State off;
            public State cookinPre;
            public State cookin;
            public State waitingOnStir;
            public State beingStirred;
            public State stirredPst;

            public Signal stirComplete;

            public TargetParameter worker;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = cookin;

                off
                    .ToggleStatusItem("off", "")
                    .Enter(smi => smi.ResetWorkable())
                    .PlayAnim("off")
                    .EventTransition(GameHashes.OperationalChanged, cookinPre, smi => smi.GetComponent<Operational>().IsOperational);

                cookinPre
                    .PlayAnim("working_pre")
                    .OnAnimQueueComplete(cookin);

                cookin
                    .ToggleStatusItem("working", "")
                    .Enter(smi => smi.ResetWorkable())
                    .PlayAnim("working_loop", KAnim.PlayMode.Loop)
                    .ToggleOperationalFlag(operationalFlag)
                    .UpdateTransition(waitingOnStir, (smi, dt) => smi.NeedsStirring(dt), UpdateRate.SIM_4000ms);

                waitingOnStir
                    .PlayAnim("ready")
                    .ToggleChore(smi => smi.CreateChore(), stirredPst)
                    .ToggleStatusItem(ModAssets.StatusItems.needsStirring)
                    .ParamTransition(worker, beingStirred, IsNotNull);

                beingStirred
                    .Target(worker)
                    .PlayAnim("working_pre")
                    .ToggleStatusItem("being stirred", "")
                    .QueueAnim("working_loop", true)
                    .OnSignal(stirComplete, stirredPst);

                stirredPst
                    .Target(worker)
                    .ToggleStatusItem("stirred pst", "")
                    .PlayAnim("working_pst")
                    .OnAnimQueueComplete(cookin);
            }
        }
    }
}
