using ONITwitchLib;
using System.Linq;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class CarcersCurseEvent() : TwitchEventBase(ID)
	{
		public const string ID = "CarcersCurse";
		private const string PREFIX = "              <color=#5476F1><b>Carcer_:</b></color> ";

		public override int GetWeight() => Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.Small;

		public override void Run()
		{
			foreach (var minion in Components.LiveMinionIdentities.Items)
			{
				var go = new GameObject("AkisExtraTwitchEvents_CarcerCurseSpewer");
				go.transform.SetParent(minion.gameObject.transform);
				go.SetActive(true);

				var spewer = go.AddComponent<Spewer>();
				spewer.options =
				[
					new(PacuConfig.ID)
				];
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

				new CustomVomitChore(minion, Elements.PinkSlime, "aete_goop_vomit_kanim", 100f);
			}

			var toastGo = ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.CARCERSCURSE.TOAST, "");
			AddImage(toastGo);
		}

		private static void AddImage(GameObject toastGo)
		{
			var texts = toastGo.GetComponentsInChildren<LocText>();

			if (texts == null)
				return;

			var last = texts.Last();

			var go = Object.Instantiate(ModAssets.Prefabs.carcersCursePrompt); //new GameObject("aete_image");
			go.transform.SetParent(last.transform.parent);

			go.transform.position = last.transform.position;
			go.transform.localScale = last.transform.localScale;

			if (last.TryGetComponent(out RectTransform rectTransform))
			{
				var newRect = go.AddOrGet<RectTransform>();
				newRect.localScale = rectTransform.localScale;
				newRect.position = rectTransform.position;
			}

			go.transform.Find("Text").GetComponent<LocText>().text = $"{PREFIX}{STRINGS.UI.AKIS_EXTRA_TWITCH_EVENTS.CARCERPROMPT}";

			go.SetActive(true);
		}
	}
}
