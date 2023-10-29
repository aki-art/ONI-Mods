using ONITwitchLib;
using System.Linq;
using Twitchery.Content.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery.Content.Events.EventTypes
{
	internal class CarcersCurseEvent : ITwitchEvent
	{
		public const string ID = "CarcersCurse";

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public void Run(object data)
		{
			foreach (var minion in Components.LiveMinionIdentities.Items)
			{
				var go = new GameObject("AkisExtraTwitchEvents_CarcerCurseSpewer");
				go.transform.SetParent(minion.gameObject.transform);
				go.SetActive(true);

				var spewer = go.AddComponent<Spewer>();
				spewer.options = new()
				{
					new(PacuConfig.ID)
				};
				spewer.minAmount = 3f;
				spewer.maxAmount = 6f;
				spewer.minDelay = 0.25f;
				spewer.maxDelay = 0.75f;
				spewer.rotate = false;
				spewer.minDistance = 1;
				spewer.maxDistance = 4;
				spewer.targetTransform = minion.transform;
				spewer.onlyUp = true;

				spewer.StartSpewing();

				new CustomVomitChore(minion, Elements.PinkSlime, "aete_goop_vomit_kanim");
			}

			var toastGo = ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.CARCERS_CURSE.TOAST, "");
			AddImage(toastGo);
		}

		private static void AddImage(GameObject toastGo)
		{
			var texts = toastGo.GetComponentsInChildren<LocText>();

			if (texts == null)
				return;

			var last = texts.Last();

			var go = new GameObject("aete_image");
			go.transform.SetParent(last.transform.parent);

			go.transform.position = last.transform.position;
			go.transform.localScale = last.transform.localScale;

			if (last.TryGetComponent(out RectTransform rectTransform))
			{
				var newRect = go.AddOrGet<RectTransform>();
				newRect.anchoredPosition = rectTransform.anchoredPosition;
				newRect.anchorMin = rectTransform.anchorMin;
				newRect.anchorMax = rectTransform.anchorMax;
				newRect.localScale = rectTransform.localScale;
				newRect.position = newRect.position;
			}

			go.transform.localScale *= 0.8f;

			var image = go.AddComponent<Image>();
			image.sprite = Assets.GetSprite("akisextratwitchevents_carcerscurse");
			image.preserveAspect = true;

			go.SetActive(true);
		}
	}
}
