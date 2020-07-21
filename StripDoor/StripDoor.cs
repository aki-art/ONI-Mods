using UnityEngine;

namespace StripDoor
{
    class StripDoor : StateMachineComponent<StripDoor.StatesInstance>
    {
        private const float ANIMATION_COOLDOWN = .495f;
        private KBatchedAnimController overlay;
        private Door door;
        private PassDirection passDirection = PassDirection.Stopped;
        private GameObject minionPassing;

        protected override void OnSpawn()
        {
            overlay = CreateOverlayAnim();
            door = GetComponent<Door>();
            base.OnSpawn();
            smi.StartSM();
        }

        protected KBatchedAnimController CreateOverlayAnim()
        {
            Grid.SceneLayer overlayLayer = Grid.SceneLayer.Ground;
            KBatchedAnimController effect = FXHelpers.CreateEffect("stripdooroverlay_kanim", transform.position, transform);
            effect.destroyOnAnimComplete = false;
            effect.fgLayer = overlayLayer;
            effect.SetLayer((int)overlayLayer);
            effect.SetSceneLayer(overlayLayer);

            return effect;
        }

        private void FlutterStrips()
        {
            if(smi.master.minionPassing == null || passDirection == PassDirection.Stopped)
            {
                return;
            }
            string anim = passDirection == PassDirection.Left ? "openLeft" : "openRight";
            overlay.Play(anim);
            overlay.Queue("closed");
        }

        private bool IsMovingMinionInCell(int cell)
        {
            GameObject minion = Grid.Objects[cell, (int)ObjectLayer.Minion];
            if (minion != null)
            {
                minionPassing = minion;
                passDirection = GetDirection(minion);

                return true;
            }

            return false;
        }

        private PassDirection GetDirection(GameObject minion)
        {
            Navigator navigator = minion.GetComponent<Navigator>();
            if(navigator.IsMoving())
            { 
                sbyte navigatorTransitionX = navigator.GetNextTransition().x;
                bool movingLeft = navigatorTransitionX > 0;
                return movingLeft ? PassDirection.Left : PassDirection.Right;
            }

            return PassDirection.Stopped;
        }
        enum PassDirection
        {
            Left,
            Right,
            Stopped
        }

        public class States : GameStateMachine<States, StatesInstance, StripDoor>
        {
            public State closed;
            public State passing;
            public State permaOpen;
            public State permaOpenPre;
            public State permaOpenPst;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = closed;
                closed
                    .Enter("PlayOverlayAnim", smi => smi.master.overlay.Play("closed"))
                    .Enter(smi => smi.master.minionPassing = null)
                    .Transition(passing, new Transition.ConditionCallback(IsPassing), UpdateRate.SIM_200ms)
                    .Transition(permaOpen, new Transition.ConditionCallback(IsPermaOpen), UpdateRate.SIM_1000ms);
                passing
                    .Enter(smi => smi.master.FlutterStrips())
                    .ScheduleGoTo(ANIMATION_COOLDOWN, closed);
                permaOpenPre
                    .Enter("PlayOverlayAnim", smi => smi.master.overlay.Play("permaOpenPre"))
                    .GoTo(permaOpen);
                permaOpen
                    .Enter("PlayOverlayAnim", smi => smi.master.overlay.Queue("permanentOpen"))
                    .Transition(permaOpenPst, Not(new Transition.ConditionCallback(IsPermaOpen)), UpdateRate.SIM_1000ms);
                permaOpenPst
                    .Enter("PlayOverlayAnim", smi => smi.master.overlay.Play("permaOpenPst"))
                    .GoTo(closed);

            }

            private bool IsPermaOpen(StatesInstance smi) => smi.master.door.CurrentState == Door.ControlState.Opened;

            private bool IsPassing(StatesInstance smi)
            {
                int bottomCell = Grid.PosToCell(smi);
                int topCell = Grid.CellAbove(bottomCell);

                bool areBothCellsValid = !Grid.IsValidCell(bottomCell) || !Grid.IsValidCell(topCell);
                bool hasMovingMinion = smi.master.IsMovingMinionInCell(bottomCell) || smi.master.IsMovingMinionInCell(topCell);

                return areBothCellsValid && hasMovingMinion;
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, StripDoor, object>.GameInstance
        {
            public StatesInstance(StripDoor smi) : base(smi)
            {
            }
        }
    }
}
