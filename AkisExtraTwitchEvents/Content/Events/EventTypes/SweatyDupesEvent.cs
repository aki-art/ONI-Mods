using Klei.AI;
using ONITwitchLib;

namespace Twitchery.Content.Events.EventTypes
{
	public class SweatyDupesEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SweatyDupes";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			foreach (var minion in Components.LiveMinionIdentities.Items)
			{
				if (minion.TryGetComponent(out Effects effects))
					effects.Add(TEffects.SWEATY, true);
			}

			LocString[] names = [
				STRINGS.AETE_EVENTS.SWEATYDUPES.DESC1,
				STRINGS.AETE_EVENTS.SWEATYDUPES.DESC2,
				STRINGS.AETE_EVENTS.SWEATYDUPES.DESC3,
				STRINGS.AETE_EVENTS.SWEATYDUPES.DESC4,
				];

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.SWEATYDUPES.TOAST, $"{names.GetRandom()}{STRINGS.AETE_EVENTS.SWEATYDUPES.DESC_END}");
		}
	}
}
