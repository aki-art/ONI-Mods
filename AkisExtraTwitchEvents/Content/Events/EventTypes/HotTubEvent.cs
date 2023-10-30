using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class HotTubEvent : ITwitchEvent
	{
		public const string ID = "HotTub";

		public int GetWeight() => TwitchEvents.Weights.UNCOMMON;

		public bool Condition(object data) => !AkisTwitchEvents.Instance.hotTubActive;

		public string GetID() => ID;

		public void Run(object data)
		{
			var go = new GameObject("AkisExtraTwitchEvents_HotTubController");
			go.AddComponent<HotTubController>().durationSeconds = (SpeedControlScreen.Instance.speed + 1) * 10f;
		}
	}
}
