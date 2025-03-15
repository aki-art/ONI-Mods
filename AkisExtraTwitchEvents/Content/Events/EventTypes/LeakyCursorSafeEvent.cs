using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class LeakyCursorSafeEvent() : TwitchEventBase(ID)
	{
		public const string ID = "LeakyCursorSafe";

		private const float DURATION = 60f;
		private const float DROPLET_PER_SECOND = 30f;
		public static Color markerColor = Util.ColorFromHex("d16e11CC");

		public override Danger GetDanger() => Danger.Small;

		public override bool Condition() => AkisTwitchEvents.MaxDanger <= Danger.High;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var potentialElements = AkisTwitchEvents.Instance.GetGenerallySafeLiquids();
			potentialElements.Shuffle();

			var elementId = potentialElements.GetRandom();
			var element = ElementLoader.FindElementByHash(elementId);

			var totalAmount = Mathf.Min(element.maxMass, element.defaultValues.mass) * 5f;
			float amountPerDroplet = totalAmount / (DURATION * DROPLET_PER_SECOND);

			var rainer = new GameObject("AETE_CursorRainer").AddComponent<CursorRainer>();

			rainer.temperature = 320;
			rainer.duration = DURATION;
			rainer.elementIdx = element.idx;
			rainer.amountPerDroplet = amountPerDroplet;

			rainer.gameObject.SetActive(true);
			rainer.StartRaining();

			AkisTwitchEvents.Instance.durationMarker.Show(DURATION, markerColor);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.LEAKYCURSORSAFE.TOAST,
				STRINGS.AETE_EVENTS.LEAKYCURSORSAFE.DESC.Replace("{Element}", element.tag.ProperNameStripLink()));
		}
	}
}
