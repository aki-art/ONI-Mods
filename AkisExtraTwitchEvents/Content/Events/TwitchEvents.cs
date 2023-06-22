using ONITwitchLib;
using ONITwitchLib.Core;
using System.Collections.Generic;

namespace Twitchery.Content.Events
{
	public class TwitchEvents
	{
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
			AddEvent<JelloRainEvent>(STRINGS.AETE_EVENTS.JELLO_RAIN.TOAST, foods);
			AddEvent<RadDishEvent>(STRINGS.AETE_EVENTS.RAD_DISH.TOAST, foods);
			AddEvent<PizzaDeliveryEvent>(STRINGS.AETE_EVENTS.PIZZA_DELIVERY.TOAST, foods);

			deckInst.AddGroup(foods);

			var visuals = EventGroup.GetOrCreateGroup(VISUALS);
			AddEvent<RetroVisionEvent>(STRINGS.AETE_EVENTS.RETRO_VISION.TOAST, visuals);
			AddEvent<InvisibleLiquidsEvent>(STRINGS.AETE_EVENTS.INVISIBLE_LIQUIDS.TOAST, visuals);
			AddEvent<EggEvent>(STRINGS.AETE_EVENTS.EGG.TOAST, visuals);

			deckInst.AddGroup(visuals);

			deckInst.AddGroup(SingleEvent<CoffeeBreakEvent>(STRINGS.AETE_EVENTS.COFFEE_BREAK.TOAST));
			deckInst.AddGroup(SingleEvent<MidasTouchEvent>(STRINGS.AETE_EVENTS.MIDAS.TOAST, Danger.Medium));
			deckInst.AddGroup(SingleEvent<DoubleTroubleEvent>(STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TOAST));
			deckInst.AddGroup(SingleEvent<GiantCrabEvent>(STRINGS.AETE_EVENTS.GIANT_CRAB.TOAST));
			deckInst.AddGroup(SingleEvent<PolymorphEvent>(STRINGS.AETE_EVENTS.POLYMOPRH.TOAST_ALT));
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
