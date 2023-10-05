using FUtility;
using ONITwitchLib;
using System.Collections.Generic;

namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public class TrickOrTreatEvent() : EventBase("TrickOrTreat")
	{
		public static HashSet<string> goodEvents = new HashSet<string>()
		{
			HiddenTripleBoxEvent.ID,
			HiddenFoodRainEvent.ID,
		};

		public static readonly List<string> ALL_EVENTS = new()
		{
			// good
			HiddenTripleBoxEvent.ID,
			HiddenFoodRainEvent.ID,
			// bad
			//HiddenRotFoodEvent.ID,
			PiptergeistEvent.ID,
			HiddenSugarSicknessEvent.ID
		};

		public static List<string> eventPool = new();

		public override void Initialize()
		{
			InitPool();
		}

		private static void InitPool()
		{
			eventPool ??= new();
			eventPool.AddRange(ALL_EVENTS);
			eventPool.Shuffle();
		}

		public override void Run()
		{
			if (eventPool.Count == 0)
				InitPool();

			string subEventId = eventPool[0];
			RunEvent(subEventId);

			if (goodEvents.Contains(subEventId))
				AudioUtil.PlayGlobalSound(ModAssets.Sounds.CHEER, Utils.GetSFXVolume());
			else
				AudioUtil.PlayGlobalSound(ModAssets.Sounds.EVILLAUGH, Utils.GetSFXVolume() * 0.8f);

			eventPool.RemoveAt(0);
		}

		public override int GetNiceness() => Intent.NEUTRAL;

		private static void RunEvent(string subEventId)
		{
			var subEvent = EventManager.Instance.GetEventByID(nameof(SpookyPumpkinSO), subEventId);

			if (subEvent == null)
			{
				ToastManager.InstantiateToast("Error", "Something went wrong");
				return;
			}

			var data = DataManager.Instance.GetDataForEvent(subEvent);
			subEvent.Trigger(data);
		}

		public override Danger GetDanger() => Danger.None; // sub events get filtered instead
	}
}
