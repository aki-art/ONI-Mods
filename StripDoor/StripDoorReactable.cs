using UnityEngine;

namespace StripDoor
{
    partial class StripDoor
    {
        private class StripDoorReactable : Reactable
		{
			private StripDoor stripDoor;
			private Navigator navigator;

			public StripDoorReactable(StripDoor sDoor) : 
				base(sDoor.gameObject, "StripDoorReactable", Db.Get().ChoreTypes.Checkpoint, 1, 1, false, ANIMATION_COOLDOWN, ANIMATION_COOLDOWN, 0f)
			{
				stripDoor = sDoor;
				preventChoreInterruption = false;
			}

			public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
			{
				return stripDoor.door.CurrentState == Door.ControlState.Auto;
			}

			protected override void InternalBegin()
			{
				navigator = reactor.GetComponent<Navigator>();

				bool isMinionMoving = GetAnim(navigator, out string anim);
				if (isMinionMoving) 
					stripDoor.FlutterStrips(anim);

				stripDoor.OrphanReactable();
				stripDoor.CreateNewReactable();

				Debug.Log("Reactable InternalBegin");
			}

			public override void Update(float dt)
			{
				Cleanup();
			}

			protected override void InternalEnd() { }

			protected override void InternalCleanup() { }

			private bool GetAnim(Navigator navigator, out string animName)
			{
				if (navigator.IsMoving())
				{
					bool movingLeft = navigator.GetNextTransition().x > 0;
					animName = movingLeft ? "openLeft" : "openRight";
					return true;
				}
				else
				{
					animName = "invalid";
					return false;
				}
			}
		}
	}
}
