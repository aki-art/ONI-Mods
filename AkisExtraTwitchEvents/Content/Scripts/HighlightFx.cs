using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class HighlightFx : KMonoBehaviour
	{
		[MyCmpReq] public KBatchedAnimController kbac;

		[Serialize] public bool isPaused;
		[Serialize] public float elapsedTime;
		[Serialize] public bool hasPlayedSound;

		[SerializeField] public Color goalTintColor;
		[SerializeField] public float duration;
		[SerializeField] public string soundFx;
		[SerializeField] public float soundTimestamp;

		public Color baseHighLightColor;

		public static Gradient highLightGradient;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (highLightGradient == null)
			{
				highLightGradient = new Gradient();
				highLightGradient.SetKeys(
					new[]
					{
						new GradientColorKey(Color.black, 0f),
						new GradientColorKey(goalTintColor, 0.5f),
						new GradientColorKey(Color.white, 0.66f),
						new GradientColorKey(goalTintColor, 0.9f),
						new GradientColorKey(Color.black, 1f),
					},
					new[]
					{
						new GradientAlphaKey(1f, 0)
					});
			}

			baseHighLightColor = Color.black;
		}

		public void Play()
		{
			isPaused = false;
			elapsedTime = 0;
		}

		public static void TryApplyHighlight(GameObject go, float value)
		{
			if (go.TryGetComponent(out HighlightFx midasFx))
			{
				if (!midasFx.ShouldTint())
					return;

				value *= 0.3f;
				midasFx.baseHighLightColor = new Color(value, value, value);
			}
		}

		public bool ShouldTint() => elapsedTime <= duration;

		private void Update()
		{
			if (isPaused)
				return;

			elapsedTime += Time.deltaTime;

			kbac.HighlightColour = highLightGradient.Evaluate(elapsedTime) + baseHighLightColor;

			if(elapsedTime > duration)
				isPaused = true;
		}
	}
}
