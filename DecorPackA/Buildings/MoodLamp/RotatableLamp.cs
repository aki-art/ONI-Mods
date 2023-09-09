using KSerialization;
using System;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class RotatableLamp : KMonoBehaviour
	{
		[Serialize] public float angle;
		[Serialize] public bool initialized;

		[MyCmpReq] private MoodLamp moodLamp;

		private KBatchedAnimController lampAnimController;
		private KBatchedAnimController arrowAnimController;

		public override void OnCmpEnable()
		{
			base.OnCmpEnable();

			if(!initialized)
			{
				initialized = true;
				angle = UnityEngine.Random.Range(0, 360);
			}

			lampAnimController = moodLamp.lampKbac;
			arrowAnimController = moodLamp.UseSecondaryKbac();
			arrowAnimController.Rotation = angle;

			UpdateArrowPosition();
		}

		public override void OnCmpDisable()
		{
			base.OnCmpDisable();
			arrowAnimController.Rotation = 0;
		}

		private void UpdateArrowPosition()
		{
			arrowAnimController.transform.position = lampAnimController
				.GetSymbolTransform("rotation_marker", out bool _)
				.GetColumn(3) with
			{
				z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront)
			};
		}

		public void SetAngle(float angle)
		{
			this.angle = angle;

			if (arrowAnimController != null)
				arrowAnimController.Rotation = angle;
		}
	}
}
