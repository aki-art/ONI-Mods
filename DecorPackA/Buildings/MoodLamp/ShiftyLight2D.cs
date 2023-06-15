using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	internal class ShiftyLight2D : KMonoBehaviour, ISim33ms
	{
		[MyCmpReq] private Operational operational;
		[MyCmpReq] private Light2D light2D;

		[SerializeField] public Color color1;
		[SerializeField] public Color color2;
		[SerializeField] public float duration;

		private float elapsed;

		public override void OnSpawn()
		{
			base.OnSpawn();
			elapsed = Random.Range(0, duration);
		}

		public void Sim33ms(float dt)
		{
			if (!enabled)
				return;

			if (!operational.IsOperational)
				return;

			elapsed += dt;

			var t = Mathf.Min(elapsed / duration, 1f);

			if (t > 0.5f)
				t = 1f - t;

			light2D.Color = Color.Lerp(color1, color2, t * 2);

			// reset
			if (elapsed >= duration)
				elapsed = 0f;
		}
	}
}
