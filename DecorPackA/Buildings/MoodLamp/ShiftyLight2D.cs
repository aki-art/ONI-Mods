using System.Collections.Generic;
using UnityEngine;
using static STRINGS.BUILDING.STATUSITEMS.ACCESS_CONTROL;

namespace DecorPackA.Buildings.MoodLamp
{
	public class ShiftyLight2D : KMonoBehaviour, ISim33ms
	{
		[MyCmpReq] private Operational operational;
		[MyCmpReq] private MoodLamp lamp;

		[SerializeField] public Color color1;
		[SerializeField] public Color color2;
		[SerializeField] public float duration;

		private float elapsed;
		private bool isActive;

		public static HashedString
			COLOR2 = "Shifty_Color2",
			DURATION = "Shifty_Duration";

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe(ModEvents.OnMoodlampChanged, OnMoodlampChanged);
			OnMoodlampChanged(lamp.currentVariantID);
		}

		private void OnMoodlampChanged(object data)
		{
			if (LampVariant.HasTag(data, LampVariants.TAGS.SHIFTY))
			{
				color1 = LampVariant.GetDataOrDefault(data, "Color", Color.white);
				color2 = LampVariant.GetCustomDataOrDefault(data, COLOR2, Color.black);
				duration = LampVariant.GetCustomDataOrDefault(data, DURATION, 7f);

				isActive = true;
				return;
			}

			isActive = false;
		}

		public void Sim33ms(float dt)
		{
			if (!isActive || !operational.IsOperational)
				return;

			elapsed += dt;

			var t = Mathf.Min(elapsed / duration, 1f);

			if (t > 0.5f)
				t = 1f - t;

			lamp.SetLightColor(Color.Lerp(color1, color2, t * 2));

			// reset
			if (elapsed >= duration)
				elapsed = 0f;
		}
	}
}
