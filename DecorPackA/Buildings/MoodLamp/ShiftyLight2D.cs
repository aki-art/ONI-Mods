using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	public class ShiftyLight2D : KMonoBehaviour, ISim33ms
	{
		[MyCmpReq] private Operational operational;
		[MyCmpReq] private Light2D light2D;
		[MyCmpReq] private MoodLamp lamp;

		[SerializeField] public Color color1;
		[SerializeField] public Color color2;
		[SerializeField] public float duration;

		private float elapsed;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe(ModEvents.OnMoodlampChanged, OnMoodlampChanged);
			OnMoodlampChanged(lamp.currentVariantID);
		}

		private void OnMoodlampChanged(object data)
		{
			if (data is string moodLampId)
			{
				var moodlamp = ModDb.lampVariants.TryGet(moodLampId);

				if(moodlamp != null && moodlamp.shifty)
				{
					color1 = moodlamp.color;
					color2 = moodlamp.color2;
					duration = moodlamp.shiftDuration;
					elapsed = Random.Range(0, duration);
				}
			}
		}

		public void Sim33ms(float dt)
		{
			if (!enabled || !operational.IsOperational)
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
