using UnityEngine;

namespace StripDoor
{
    class StripDoor : StateMachineComponent<StripDoor.StatesInstance>
	{
		private const float ANIMATION_COOLDOWN = .495f;

		private Door door;
		private StripDoorReactable reactable;
		private KBatchedAnimController overlay;
		private readonly Grid.SceneLayer overlayLayer = Grid.SceneLayer.Ground;

		protected override void OnSpawn()
		{
			door = GetComponent<Door>();
			overlay = CreateOverlayAnim();

			base.OnSpawn();

			smi.StartSM();
		}

		protected KBatchedAnimController CreateOverlayAnim()
		{
			KBatchedAnimController effect = FXHelpers.CreateEffect("stripdooroverlay_kanim", transform.position, transform);
			effect.destroyOnAnimComplete = false;
			effect.fgLayer = overlayLayer;
			effect.SetLayer((int)overlayLayer);
			effect.SetSceneLayer(overlayLayer);

			return effect;
		}

		private void PlayOverlayAnimOnce(string anim)
		{
			overlay.Play(anim, KAnim.PlayMode.Once, 1f, 0);
		}

		private void CreateNewReactable()
		{
			if (reactable != null)
			{
				return;
			}

			reactable = new StripDoorReactable(this);
		}

		private void OrphanReactable() => reactable = null;


		private class StripDoorReactable : Reactable
		{
			private readonly StripDoor stripDoorToReact;
			private Navigator reactor_navigator;

			public StripDoorReactable(StripDoor stripDoor) : base(stripDoor.gameObject, "StripDoorReactable", Db.Get().ChoreTypes.Checkpoint, 1, 1, false, 0f, ANIMATION_COOLDOWN, 0)
			{
				stripDoorToReact = stripDoor;
				preventChoreInterruption = true;
			}

			public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
			{
				if (reactor != null)
				{
					return false;
				}

				if (stripDoorToReact == null)
				{
					Cleanup();
					return false;
				}

				return true;
			}

			protected override void InternalBegin()
			{
				reactor_navigator = reactor.GetComponent<Navigator>();

				stripDoorToReact.OrphanReactable();
				stripDoorToReact.CreateNewReactable();
			}

			public override void Update(float dt)
			{
				if (stripDoorToReact == null || reactor_navigator == null)
				{
					Cleanup();
					return;
				}

				bool isDoorOpen = stripDoorToReact.door.CurrentState == Door.ControlState.Opened;

				if (isDoorOpen)
				{
					OpenDoor();
				}
				else
				{
					PassByDoor();
				}

				Cleanup();
				return;
			}

			private void OpenDoor()
			{
				// Add pre opening anim
				stripDoorToReact.smi.GoTo(stripDoorToReact.smi.sm.permaOpen);
			}

			private void PassByDoor()
			{
				NavGrid.Transition nextTransition = reactor_navigator.GetNextTransition();
				bool goingLeft = nextTransition.x > 0;
				string anim = goingLeft ? "openLeft" : "openRight";

				stripDoorToReact.PlayOverlayAnimOnce(anim);
				stripDoorToReact.smi.GoTo(stripDoorToReact.smi.sm.passing);
			}

			protected override void InternalEnd()
			{
			}

			protected override void InternalCleanup()
			{
			}
		}

		public class States : GameStateMachine<States, StatesInstance, StripDoor>
        {
            public State closed;
            public State passing;
            public State permaOpen;

			public override void InitializeStates(out BaseState default_state)
            {
                default_state = closed;

				closed
					.Enter("PlayOverlayAnim", smi => smi.master.PlayOverlayAnimOnce("closed"))
					.Update("CheckState", (smi, dt) => { smi.master.CheckClosedState(); }, UpdateRate.SIM_200ms);
				passing
					.ScheduleGoTo(ANIMATION_COOLDOWN, closed);
				permaOpen
					.Enter("PlayOverlayAnim", smi => smi.master.PlayOverlayAnimOnce("permanentOpen"))
					.Update("CheckState", (smi, dt) => { smi.master.CheckOpenState(); }, UpdateRate.SIM_200ms);
			}
		}
		bool IsDoorOpen() => door.CurrentState == Door.ControlState.Opened;

		private void CheckClosedState()
		{
			if (IsDoorOpen())
			{
				smi.GoTo(smi.sm.permaOpen);
				return;
			}

			CreateNewReactable();
		}

		private void CheckOpenState()
		{
			if (!IsDoorOpen())
			{
				smi.GoTo(smi.sm.closed);
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
