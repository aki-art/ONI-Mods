using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class LeakyCursorExtremeEvent() : TwitchEventBase(ID)
	{
		public const string ID = "LeakyCursorExtreme";

		private const float DURATION = 60f;
		private const float DROPLET_PER_SECOND = 30f;

		public override Danger GetDanger() => Danger.Extreme;

		public override bool Condition() => AkisTwitchEvents.MaxDanger > Danger.High;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var potentialElements = AkisTwitchEvents.Instance.GetLiquids(0, 9975);
			potentialElements.Shuffle();

			var elementId = potentialElements.GetRandom();
			var element = ElementLoader.FindElementByHash(elementId);

			var totalAmount = Mathf.Min(element.maxMass, element.defaultValues.mass) * 5f;
			float amountPerDroplet = totalAmount / (DURATION * DROPLET_PER_SECOND);

			var rainer = new GameObject("AETE_CursorRainer").AddComponent<CursorRainer>();

			rainer.temperature = element.defaultValues.temperature;
			rainer.duration = DURATION;
			rainer.elementIdx = element.idx;
			rainer.amountPerDroplet = amountPerDroplet;

			rainer.gameObject.SetActive(true);
			rainer.StartRaining();

			AkisTwitchEvents.Instance.durationMarker.Show(DURATION, LeakyCursorSafeEvent.markerColor);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.LEAKYCURSOREXTREME.TOAST,
				STRINGS.AETE_EVENTS.LEAKYCURSORSAFE.DESC.Replace("{Element}", element.tag.ProperNameStripLink()));
		}
	}
}
