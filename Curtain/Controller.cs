namespace Curtain
{
    public partial class Curtain
    {
        private Controller.Instance controller;
        public class Controller : GameStateMachine<Controller, Controller.Instance, Curtain>
        {
            public string symbolPrefix = "";

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
                    .Enter(smi => Debug.Log("playing: " + symbolPrefix + "closed")) // outputs: "purple_closed"
                    .PlayAnim(smi => smi.GetAnim("closed"), KAnim.PlayMode.Once); //plays "closed"
                closing
                    .PlayAnim(smi => smi.GetAnim("permanentOpenPst"), KAnim.PlayMode.Once)
                    .OnAnimQueueComplete(closed);
                open
                    .Enter(smi => smi.master.flutterable.Listening = false)
                    .ParamTransition(isOpen, closing, IsFalse)
                    .PlayAnim(smi => smi.GetAnim("permanentOpen"), KAnim.PlayMode.Once);
                opening
                    .PlayAnim(smi => smi.GetAnim("permanentOpenPre"), KAnim.PlayMode.Once)
                    .OnAnimQueueComplete(open);
                passing
                    .Enter(smi => smi.master.Open(false))
                    .PlayAnim(smi => smi.GetMovementAnim(), KAnim.PlayMode.Once)
                    .Exit(smi => smi.master.flutterable.Listening = false);
                locked
                    .Enter(smi => smi.master.flutterable.Listening = false)
                    .PlayAnim(smi => smi.GetAnim("lockedPre"), KAnim.PlayMode.Once)
                    .QueueAnim("", false, smi => smi.GetAnim("locked"))
				    .ParamTransition(isLocked, unlocking, IsFalse);
                unlocking
                    .PlayAnim(smi => smi.GetAnim("lockedPst"), KAnim.PlayMode.Once)
                    .OnAnimQueueComplete(closed);
            }

            public new class Instance : GameInstance
            {
                readonly string prefix;

                public Instance(Curtain curtain, string prefix) : base(curtain) 
                {
                    this.prefix = prefix;
                }

                internal string GetMovementAnim() =>  smi.master.flutterable.passingLeft ? prefix + "openLeft" : prefix + "openRight";
                internal string GetAnim(string name) => prefix + name;

            }
        }
    }
}
