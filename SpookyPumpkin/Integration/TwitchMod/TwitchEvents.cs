using ONITwitchLib;
using ONITwitchLib.Core;
using System.Collections.Generic;

namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public class TwitchEvents
	{
		public static EventInfo
			trickOrTreatEvent,
			hiddenRotFoodEvent, // disabled atm
			hiddenTripleBoxEvent,
			piptergeistEvent,
			foodRainEvent,
			sugarSicknessEvent;

		private static readonly HashSet<EventGroup> groups = new();

		public static void OnDbInit()
		{
			if (!TwitchModInfo.TwitchIsPresent)
				return;

			var deckInst = TwitchDeckManager.Instance;

			trickOrTreatEvent = SingleEvent<TrickOrTreatEvent>().ev;
			hiddenRotFoodEvent = SingleEvent<HiddenRotFoodEvent>().ev;
			hiddenTripleBoxEvent = SingleEvent<HiddenTripleBoxEvent>().ev;
			piptergeistEvent = SingleEvent<PiptergeistEvent>().ev;
			foodRainEvent = SingleEvent<HiddenFoodRainEvent>().ev;
			sugarSicknessEvent = SingleEvent<HiddenSugarSicknessEvent>().ev;

			foreach (var group in groups)
				deckInst.AddGroup(group);
		}


		private static (EventInfo ev, EventGroup group) SingleEvent<T>() where T : EventBase, new()
		{
			var eventInstance = new T();
			var (ev, group) = EventGroup.DefaultSingleEventGroup(eventInstance.id, eventInstance.GetWeight(), eventInstance.GetName());
			ev.AddListener(_ => eventInstance.Run());
			ev.AddCondition(_ => eventInstance.Condition());
			ev.Danger = eventInstance.GetDanger();

			groups.Add(group);

			eventInstance.Initialize();

			return (ev, group);
		}
	}
}
