using HarmonyLib;
using ONITwitchLib;
using ONITwitchLib.Core;
using System.Collections.Generic;
using Twitchery.Content.Events.EventTypes;
using Twitchery.Content.Events.EventTypes.ToucherEvents;

namespace Twitchery.Content.Events
{
	public class TwitchEvents
	{
		private static readonly HashSet<EventGroup> groups = [];
		private static readonly HashSet<TwitchEventBase> events = [];

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
			TOUCHERS = "AETE_Touchers",
			PIPS = "AETE_Pips",
			RAINS = "AETE_Rains";

		public static void OnDbInit()
		{
			if (!TwitchModInfo.TwitchIsPresent)
				return;

			var deckInst = TwitchDeckManager.Instance;

			var foods = EventGroup.GetOrCreateGroup(FOOD);
			var visuals = EventGroup.GetOrCreateGroup(VISUALS);
			var touchers = EventGroup.GetOrCreateGroup(TOUCHERS);
			var rains = EventGroup.GetOrCreateGroup(RAINS);

			CreateEvent<RadDishEvent>(foods);
			CreateEvent<PizzaDeliveryEvent>(foods);

			if (!(bool)TwitchSettings.GetSettingsDictionary()["PhotosensitiveMode"])
				CreateEvent<RetroVisionEvent>(visuals);
			CreateEvent<InvisibleLiquidsEvent>(visuals);
			CreateEvent<EggEvent>(visuals);
			CreateEvent<HotTubEvent>(visuals);

			CreateEvent<SlimeTouchEvent>(touchers);
			CreateEvent<FreezeTouchEvent>(touchers);
			CreateEvent<MidasTouchEvent>(touchers);
			CreateEvent<ForestTouchEvent>(touchers);

			CreateEvent<BrackeneRainEvent>(rains);
			CreateEvent<JelloRainEvent>(rains);
			CreateEvent<GoopRainEvent>(rains);
			CreateEvent<RainbowRainEvent>(rains);

			CreateSingleEvent<PipSplosionEvent>();
			CreateSingleEvent<CarcersCurseEvent>();
			CreateSingleEvent<GiantCrabEvent>();
			CreateSingleEvent<SpawnDeadlyElement2Event>().SetName(Strings.Get("STRINGS.ONITWITCH.EVENTS.ELEMENT_GROUP_DEADLY"));
			CreateSingleEvent<TreeEvent>();
			CreateSingleEvent<DoubleTroubleEvent>();
			CreateSingleEvent<CoffeeBreakEvent>();
			CreateSingleEvent<PlaceAquariumEvent>();
			CreateSingleEvent<MegaFartEvent>();
			CreateSingleEvent<RockPaperScissorsEvent>();
			CreateSingleEvent<PlaceGeyserEvent>();
			CreateSingleEvent<SinkHoleEvent>();
			CreateSingleEvent<PimplesEventSmall>();
			CreateSingleEvent<PimplesEventMedium>();
			CreateSingleEvent<SolarStormEventSmall>();
			CreateSingleEvent<SolarStormEventMedium>();
			CreateSingleEvent<MugShotsEvent>();

			// temporarily disabled while being reworked
			//CreateSingleEvent<PolymorphEvent>(out var polyEvent);
			//AkisTwitchEvents.polymorphEvent = polyEvent;


#if HULK
			deckInst.AddGroup(SingleEvent<SpawnHulkEvent>(STRINGS.AETE_EVENTS.HULK.TOAST, Danger.None).group);
#endif

			/*			var (wereVoleEv, wereGroup) = SingleEvent<WereVoleEvent>(STRINGS.AETE_EVENTS.WEREVOLE.EVENT_NAME, Danger.Small);
						WereVoleEvent.ev = wereVoleEv;
						deckInst.AddGroup(wereGroup);*/
			/*
						var pips = EventGroup.GetOrCreateGroup(PIPS);
						AkisTwitchEvents.encouragePipEvent = AddEvent<EncourageRegularPipEvent>(STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.TOAST, pips).ev;
						AddEvent<SpawnRegularPipEvent>(STRINGS.AETE_EVENTS.REGULAR_PIP.TOAST, pips);

						deckInst.AddGroup(pips);*/


			//CreateSingleEvent<ColossalFartEvent>();
			// CreateSingleEvent<AllOfTheOthersEvent>();
			//CreateSingleEvent<ChatRaidEvent>(Danger.None);
			//CreateSingleEvent<SeedyPipEvent>();
			//CreateSingleEvent<AlienAbductionEvent>();
			//CreateEvent<ChaosTouchEvent>(touchers);

			deckInst.AddGroup(foods);
			deckInst.AddGroup(visuals);
			deckInst.AddGroup(touchers);
			deckInst.AddGroup(rains);

			foreach (var group in groups)
				deckInst.AddGroup(group);
		}

		private static void CreateEvent<T>(EventGroup group) where T : TwitchEventBase, new()
		{
			var eventInstance = new T();
			var ev = group.AddEvent(
				eventInstance.id,
				eventInstance.GetWeight(),
				eventInstance.GetName());
			eventInstance.ConfigureEvent(ev);

			events.Add(eventInstance);
			groups.Add(group);
		}

		private static T CreateSingleEvent<T>() where T : TwitchEventBase, new()
		{
			return CreateSingleEvent<T>(out _);
		}

		private static T CreateSingleEvent<T>(out EventInfo info) where T : TwitchEventBase, new()
		{
			var eventInstance = new T();
			return CreateSingleEvent(eventInstance, out info);
		}

		private static T CreateSingleEvent<T>(T eventInstance, out EventInfo info) where T : TwitchEventBase
		{
			var (ev, group) = EventGroup.DefaultSingleEventGroup(eventInstance.id, eventInstance.GetWeight(), eventInstance.GetName());
			info = ev;

			eventInstance.ConfigureEvent(ev);

			events.Add(eventInstance);
			groups.Add(group);

			return eventInstance;
		}

		public static void OnDraw() => events.Do(e => e.OnDraw());

		public static void OnGameReload() => events.Do(e => e.OnGameLoad());
	}
}
