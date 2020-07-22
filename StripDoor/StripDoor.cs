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
        private string swooshSound;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            swooshSound = GlobalAssets.GetSound("drecko_ruffle_scales_short");
            overlay = CreateOverlayAnim();
            door = GetComponent<Door>();
            smi.StartSM();
        }

        protected KBatchedAnimController CreateOverlayAnim()
        {
            Grid.SceneLayer overlayLayer = Grid.SceneLayer.Ground;
            KBatchedAnimController effect = FXHelpers.CreateEffect("stripdooroverlay_kanim", transform.position, transform);
            effect.destroyOnAnimComplete = false;
            effect.fgLayer = overlayLayer;
            effect.SetSceneLayer(overlayLayer);

            return effect;
        }

        private void FlutterStrips()
        {
            if(smi.master.minionPassing == null || passDirection == PassDirection.Stopped)
            {
                return;
            }

            SoundEvent.EndOneShot(SoundEvent.BeginOneShot(swooshSound, transform.position, 2f, SoundEvent.ObjectIsSelectedAndVisible(gameObject)));
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
#pragma warning disable 649
            public State closed;
            public State passing;
            public State permaOpen;
            public State permaOpenPre;
            public State permaOpenPst;
            public State lockedPre;
            public State locked;
            public State lockedPst;
#pragma warning restore 649  

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = closed;
                closed
                    .Enter(smi => smi.master.ClearDoorState())
                    .Transition(passing, new Transition.ConditionCallback(IsPassing), UpdateRate.SIM_200ms)
                    .Transition(permaOpenPre, new Transition.ConditionCallback(IsPermaOpen), UpdateRate.SIM_1000ms)
                    .Transition(lockedPre, new Transition.ConditionCallback(IsLocked), UpdateRate.SIM_1000ms);
                passing
                    .Enter(smi => smi.master.FlutterStrips())
                    .ScheduleGoTo(ANIMATION_COOLDOWN, closed);
                permaOpenPre
                    .Enter(smi => smi.master.overlay.Play("permaOpenPre"))
                    .GoTo(permaOpen);
                permaOpen
                    .Enter(smi => smi.master.overlay.Queue("permanentOpen"))
                    .Transition(permaOpenPst, Not(new Transition.ConditionCallback(IsPermaOpen)), UpdateRate.SIM_1000ms);
                permaOpenPst
                    .Enter(smi => smi.master.overlay.Play("permaOpenPst"))
                    .GoTo(closed);
                lockedPre
                    .Enter(smi => smi.master.overlay.Play("lockedPre"))
                    .GoTo(locked);
                locked
                    .Enter(smi => smi.master.overlay.Queue("locked"))
                    .Transition(permaOpenPst, Not(new Transition.ConditionCallback(IsLocked)), UpdateRate.SIM_1000ms);
                lockedPst
                    .Enter(smi => smi.master.overlay.Play("locked")) // TODO: lockedPst animation
                    .GoTo(closed);
            }

            private bool IsPermaOpen(StatesInstance smi) => smi.master.door.CurrentState == Door.ControlState.Opened;
            private bool IsLocked(StatesInstance smi) => smi.master.door.CurrentState == Door.ControlState.Locked;

            private bool IsPassing(StatesInstance smi)
            {
                int bottomCell = Grid.PosToCell(smi);
                int topCell = Grid.CellAbove(bottomCell);

                bool areBothCellsValid = Grid.IsValidCell(bottomCell) && Grid.IsValidCell(topCell);
                bool hasMovingMinion = smi.master.IsMovingMinionInCell(bottomCell) || smi.master.IsMovingMinionInCell(topCell);

                return areBothCellsValid && hasMovingMinion;
            }
        }

        private void ClearDoorState()
        {
            smi.master.overlay.Queue("closed");
            smi.master.minionPassing = null;
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, StripDoor, object>.GameInstance
        {
            public StatesInstance(StripDoor smi) : base(smi)
            {
            }
        }
    }
}
