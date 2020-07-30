namespace Curtain
{
    public partial class Curtain
    {
        private Controller.Instance controller;
        public class Controller : GameStateMachine<Controller, Controller.Instance, Curtain>
        {
            public State closed;
            public State closing;
            public State open;
            public State opening;
            public State passing;
            public State passingPst;
            public State locked;
            public State unlocking;

            public BoolParameter isOpen;
            public BoolParameter isClosed;
            public BoolParameter isLocked;

            public override void InitializeStates(out BaseState default_state)
            {
                serializable = true;
                default_state = closed;

                closed
                    .ParamTransition(isClosed, opening, IsFalse)
                    .EventTransition(GameHashes.WalkBy, passing)
                    .Update((smi, dt) => smi.master.CheckDupePassing(), UpdateRate.SIM_200ms)
                    .PlayAnim("closed");
                closing
                    .PlayAnim("permanentOpenPst")
                    .OnAnimQueueComplete(closed);
                open
                    .ParamTransition(isOpen, closing, IsFalse)
                    .PlayAnim("permanentOpen");
                opening
                    .PlayAnim("permanentOpenPre")
                    .OnAnimQueueComplete(open);
                passing
                    .Enter(smi => smi.master.Open())
                    .PlayAnim(smi => smi.GetMovementAnim(), KAnim.PlayMode.Once)
                    .ScheduleGoTo(.5f, closed)
                    .Exit(smi => smi.master.Close());
                locked
                    .PlayAnim("lockedPre")
                    .QueueAnim("locked");
                unlocking
                    .PlayAnim("lockedPst")
                    .GoTo(closed);
            }

            public new class Instance : GameInstance
            {
                public Instance(Curtain curtain) : base(curtain) { }
                internal string GetMovementAnim() => smi.master.passingLeft ? "openLeft" : "openRight";
            }
        }
    }
}
