using UnityEngine;
using UnityEngine.UI;

namespace Twitchery.Content.Scripts
{
	public class AETE_CursorDurationMarker : KMonoBehaviour, ISimEveryTick
	{
		private Image ring;
		private float duration;
		private float elapsed;

		public void SimEveryTick(float dt)
		{
			if (elapsed < duration)
			{
				if (ring != null)
					ring.fillAmount = Mathf.Clamp01(1f - (elapsed / duration));

				elapsed += dt;
				transform.position = KInputManager.GetMousePos() with { z = -5f };
			}
			else if (isActiveAndEnabled)
			{
				gameObject.SetActive(false);
			}
		}

		public void Show(float duration, Color color)
		{
			gameObject.SetActive(true);

			if (ring == null)
				InitRing();

			ring.color = color;
			elapsed = 0;
			this.duration = duration;
		}

		private void InitRing()
		{
			ring = gameObject.AddOrGet<Image>();
			ring.type = Image.Type.Filled;
			ring.fillMethod = Image.FillMethod.Radial360;
			ring.sprite = Assets.GetSprite("akisextratwitchevents_small_ring");

			var canvasGroup = gameObject.AddOrGet<CanvasGroup>();
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			var rectTransform = gameObject.AddOrGet<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.localScale *= 0.5f;
		}
	}
}

