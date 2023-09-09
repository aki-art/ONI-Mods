using DecorPackA.Buildings.MoodLamp;
using UnityEngine;

namespace DecorPackA.UI
{
	public class DecorPackA_RotatableSideScreen : SideScreenContent
	{
		private FKnobControl angleKnob;
		private RotatableLamp targetLamp;

		public override int GetSideScreenSortOrder() => 10;

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out RotatableLamp rotatable) && rotatable.enabled;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);
			if (target.TryGetComponent(out RotatableLamp rotatable))
			{
				angleKnob.Angle = rotatable.angle;
				targetLamp = rotatable;
			}
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			FUtility.FUI.Helper.ListChildren(transform);

			angleKnob = transform.Find("Contents/AngleControl/RadialControl").gameObject.AddComponent<FKnobControl>();
			angleKnob.knob = angleKnob.transform.Find("KnobCenter");
		}

		public override void Show(bool show = true)
		{
			base.Show(show);
			angleKnob.OnChanged += OnAngleChanged;
		}

		private void OnAngleChanged()
		{
			if (targetLamp != null)
				targetLamp.SetAngle(angleKnob.Angle);
		}
	}
}
