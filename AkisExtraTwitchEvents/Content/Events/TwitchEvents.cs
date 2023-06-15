using ONITwitchLib;
using ONITwitchLib.Core;
using System.Collections.Generic;

namespace Twitchery.Content.Events
{
	public class TwitchEvents
	{
		public static ColorScrambleEvent colorScrambleEvent;

		public static List<ITwitchEvent> myEvents = new();

		public class Weights
		{
			public const int
				COMMON = 25,
				RARE = 10,
				GUARANTEED = 20000;
		}

		public const string
			FOOD = "AETE_Food",
			VISUALS = "AETE_Visuals";

		public static void OnDbInit()
		{
			if (!TwitchModInfo.TwitchIsPresent)
				return;

			var deckInst = TwitchDeckManager.Instance;

			var foods = EventGroup.GetOrCreateGroup(FOOD);
			AddEvent<JelloRainEvent>("Jello Rain", foods);
			AddEvent<RadDishEvent>("Rad Dish", foods);
			AddEvent<PizzaDeliveryEvent>("Pizza Delivery", foods);

			deckInst.AddGroup(foods);

			var visuals = EventGroup.GetOrCreateGroup(VISUALS);
			AddEvent<RetroVisionEvent>("Retro Vision", visuals);
			AddEvent<InvisibleLiquidsEvent>("Invisible Liquids", visuals);

			deckInst.AddGroup(visuals);

			deckInst.AddGroup(SingleEvent<CoffeeBreakEvent>("Coffee Break"));
			deckInst.AddGroup(SingleEvent<MidasTouchEvent>("Midas Time", Danger.Medium));
			deckInst.AddGroup(SingleEvent<DoubleTroubleEvent>("Double Trouble"));
		}

		private static EventGroup AddEvent<T>(string friendlyName, EventGroup group, Danger danger = Danger.None) where T : ITwitchEvent, new()
		{
			var eventInstance = new T();

			var ev = group.AddEvent(eventInstance.GetID(), Weights.COMMON, friendlyName);
			ev.AddListener(eventInstance.Run);
			ev.AddCondition(eventInstance.Condition);
			ev.Danger = danger;

			myEvents.Add(eventInstance);

			return group;
		}

		private static EventGroup SingleEvent<T>(string friendlyName, Danger danger = Danger.None) where T : ITwitchEvent, new()
		{
			var eventInstance = new T();
			var (ev, group) = EventGroup.DefaultSingleEventGroup(eventInstance.GetID(), Weights.COMMON, friendlyName);
			ev.AddListener(eventInstance.Run);
			ev.AddCondition(eventInstance.Condition);
			ev.Danger = danger;

			myEvents.Add(eventInstance);

			return group;
		}
	}
}
