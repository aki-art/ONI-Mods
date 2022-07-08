using KSerialization;
using UnityEngine;

namespace CrittersDropBones.Buildings.SlowCooker
{
    public class Stirrable : StateMachineComponent<Stirrable.StatesInstance>//, IGameObjectEffectDescriptor
    {
        [MyCmpGet]
        private Operational operational;

        [MyCmpGet]
        private ComplexFabricator fabricator;

        [Serialize]
        private float lastStir = 0;

        [SerializeField]
        private float stirDelaySeconds = 10;

        public static readonly Operational.Flag operationalFlag = new Operational.Flag("cooking_pot", Operational.Flag.Type.Requirement);

        protected override void OnSpawn()
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
            lastStir = 0;
            smi.sm.stirComplete.Trigger(smi);
            SetWorker(null);
        }

        public class StatesInstance : GameStateMachine<Stirrable.States, StatesInstance, Stirrable, object>.GameInstance
        {
            public StatesInstance(Stirrable master) : base(master) { }

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
                    //on_complete: OnCompleteChore,
                    only_when_operational: false);
            }

            private void OnCompleteChore(Chore obj)
            {
                smi.master.lastStir = 0;
                smi.sm.stirComplete.Trigger(smi);
            }

            internal bool NeedsStirring(float dt)
            {
                master.lastStir += dt;
                if (!smi.master.operational.IsOperational || smi.master.fabricator.CurrentWorkingOrder is null)
                {
                    return false;
                }

                return master.lastStir >= master.stirDelaySeconds;
            }
        }

        public class States : GameStateMachine<States, StatesInstance, Stirrable>
        {
            public State off;
            public State workingPre;
            public State working;
            public State needsStirring;
            public State beingStirred;
            public State stirredPst;

            public Signal stirComplete;

            public TargetParameter worker;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = working;

                off
                    .ToggleStatusItem("off", "")
                    .Enter(smi => smi.ResetWorkable())
                    .PlayAnim("off")
                    .EventTransition(GameHashes.OperationalChanged, workingPre, smi => smi.GetComponent<Operational>().IsOperational);

                workingPre
                    .PlayAnim("working_pre")
                    .OnAnimQueueComplete(working);

                working
                    .ToggleStatusItem("working", "")
                    .Enter(smi => smi.ResetWorkable())
                    .PlayAnim("working_loop", KAnim.PlayMode.Loop)
                    .ToggleOperationalFlag(operationalFlag)
                    .UpdateTransition(needsStirring, (smi, dt) => smi.NeedsStirring(dt), UpdateRate.SIM_4000ms);

                needsStirring
                    .PlayAnim("ready")
                    .ToggleChore(smi => smi.CreateChore(), working)
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
                    .OnAnimQueueComplete(working);
            }
        }
    }
}
