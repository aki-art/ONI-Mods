using ONITwitchLib;
using ONITwitchLib.Core;
using System;
using System.Collections.Generic;
using Twitchery.Content.Events.RegularPipEvents;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	public class TwitchEvents
	{
		public static List<ITwitchEvent> myEvents = new();

		public class Weights
		{
			public const int
				COMMON = 24,
				UNCOMMON = 19,
				RARE = 10,
				VERY_RARE = 5,
				GUARANTEED = 20000;
		}

		public const string
			FOOD = "AETE_Food",
			VISUALS = "AETE_Visuals",
			PIPS = "AETE_Pips";

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
			AddEvent<HotTubEvent>("Hot Tub", visuals);

			deckInst.AddGroup(visuals);

			deckInst.AddGroup(SingleEvent<CoffeeBreakEvent>(STRINGS.AETE_EVENTS.COFFEE_BREAK.TOAST).group);
			deckInst.AddGroup(SingleEvent<PipSplosionEvent>(STRINGS.AETE_EVENTS.PIPSPLOSION.TOAST).group);
			deckInst.AddGroup(SingleEvent<MidasTouchEvent>(STRINGS.AETE_EVENTS.MIDAS.TOAST, Danger.Medium).group);
			deckInst.AddGroup(SingleEvent<SlimeTouchEvent>(STRINGS.AETE_EVENTS.SLIMETOUCH.TOAST, Danger.Medium).group);
			deckInst.AddGroup(SingleEvent<BrackeneRainEvent>(STRINGS.AETE_EVENTS.BRACKENE_RAIN.TOAST, Danger.Small).group);
			deckInst.AddGroup(SingleEvent<DoubleTroubleEvent>(STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TOAST, Danger.Medium).group);
			deckInst.AddGroup(SingleEvent<CarcersCurseEvent>(STRINGS.AETE_EVENTS.CARCERS_CURSE.TOAST, Danger.Small).group);
			deckInst.AddGroup(SingleEvent<GiantCrabEvent>(STRINGS.AETE_EVENTS.GIANT_CRAB.TOAST).group);
			var (polyEvent, polyGroup) = SingleEvent<PolymorphEvent>(STRINGS.AETE_EVENTS.POLYMOPRH.TOAST_ALT);
			AkisTwitchEvents.polymorphEvent = polyEvent;

			deckInst.AddGroup(polyGroup);
			deckInst.AddGroup(SingleEvent<GoopRainEvent>(STRINGS.AETE_EVENTS.SLIME_RAIN.TOAST, Danger.Small).group);
			deckInst.AddGroup(SingleEvent<TreeEvent>(STRINGS.AETE_EVENTS.TREE.TOAST, Danger.Medium).group);
			deckInst.AddGroup(SingleEvent<SpawnHulkEvent>(STRINGS.AETE_EVENTS.HULK.TOAST, Danger.None).group);

			var (wereVoleEv, wereGroup) = SingleEvent<WereVoleEvent>(STRINGS.AETE_EVENTS.WEREVOLE.EVENT_NAME, Danger.Small);
			WereVoleEvent.ev = wereVoleEv;
			deckInst.AddGroup(wereGroup);

			var pips = EventGroup.GetOrCreateGroup(PIPS);
			AkisTwitchEvents.encouragePipEvent = AddEvent<EncourageRegularPipEvent>(STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.TOAST, pips).ev;
			AddEvent<SpawnRegularPipEvent>(STRINGS.AETE_EVENTS.REGULAR_PIP.TOAST, pips);

			deckInst.AddGroup(pips);
		}

		private static (EventInfo ev, EventGroup group) AddEvent<T>(string friendlyName, EventGroup group, Danger danger = Danger.None) where T : ITwitchEvent, new()
		{
			var eventInstance = new T();

			var ev = group.AddEvent(eventInstance.GetID(), Weights.COMMON, friendlyName);
			ev.AddListener(eventInstance.Run);
			ev.AddCondition(eventInstance.Condition);
			ev.Danger = danger;

			myEvents.Add(eventInstance);

			return (ev, group);
		}

		private static (EventInfo ev, EventGroup group) SingleEvent<T>(string friendlyName, Danger danger = Danger.None, Dictionary<string, object> data = null, int weight = Weights.COMMON) where T : ITwitchEvent, new()
		{
			var eventInstance = new T();
			var (ev, group) = EventGroup.DefaultSingleEventGroup(eventInstance.GetID(), weight, friendlyName);
			ev.AddListener(eventInstance.Run);
			ev.AddCondition(eventInstance.Condition);
			ev.Danger = danger;

			if (data != null)
				DataManager.Instance.SetDataForEvent(ev, data);

			myEvents.Add(eventInstance);

			return (ev, group);
		}
	}
}
