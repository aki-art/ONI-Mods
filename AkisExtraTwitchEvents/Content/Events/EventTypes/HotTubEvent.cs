using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class HotTubEvent() : TwitchEventBase(ID)
	{
		public const string ID = "HotTub";

		public override int GetWeight() => Consts.EventWeight.Common;
		public override Danger GetDanger() => Danger.None;


		public override bool Condition() => !AkisTwitchEvents.Instance.HotTubActive;

		public override void Run()
		{
			var go = new GameObject("AkisExtraTwitchEvents_HotTubController");
			go.AddComponent<HotTubController>().durationSeconds = 15f;
		}
	}
}
