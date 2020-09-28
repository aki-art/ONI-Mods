﻿namespace Curtain
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
            public State passingWaiting;

            public BoolParameter isOpen;
            public BoolParameter isClosed;
            public BoolParameter isLocked;

            public override void InitializeStates(out BaseState default_state)
            {
                serializable = true;
                default_state = closed;

               
                closed
                    .ParamTransition(isOpen, opening, IsTrue)
                    .ParamTransition(isLocked, locked, IsTrue)
                    .Enter(smi => smi.master.flutterable.Listening = true)
                    .PlayAnim("closed");
                closing
                    .PlayAnim("permanentOpenPst")
                    .OnAnimQueueComplete(closed);
                open
                    .Enter(smi => smi.master.flutterable.Listening = false)
                    .ParamTransition(isOpen, closing, IsFalse)
                    .PlayAnim("permanentOpen");
                opening
                    .PlayAnim("permanentOpenPre")
                    .OnAnimQueueComplete(open);
                passing
                    .Enter(smi => smi.master.Open(false))
                    .PlayAnim(smi => smi.GetMovementAnim(), KAnim.PlayMode.Once)
                    .Exit(smi => smi.master.flutterable.Listening = false);
                locked
                    .Enter(smi => smi.master.flutterable.Listening = false)
                    .PlayAnim("lockedPre")
                    .QueueAnim("locked")
				    .ParamTransition(isLocked, unlocking, IsFalse);
                unlocking
                    .PlayAnim("lockedPst")
                    .OnAnimQueueComplete(closed);
            }

            public new class Instance : GameInstance
            {
                public Instance(Curtain curtain) : base(curtain) { }
                internal string GetMovementAnim() => smi.master.flutterable.passingLeft ? "openLeft" : "openRight";
            }
        }
    }
}