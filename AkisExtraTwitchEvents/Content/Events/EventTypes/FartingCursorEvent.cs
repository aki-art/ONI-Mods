using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class FartingCursorEvent() : TwitchEventBase(ID)
	{
		public const string ID = "FartingCursor";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var duration = 30.0f;
			var totalMass = 500;

			var go = new GameObject("AETE_FartingCursor");

			var timer = go.AddComponent<AETE_Timer>();
			timer.time = duration;
			timer.id = "fart_over";
			timer.Begin();

			var farter = go.AddComponent<FartingCursor>();
			farter.massPerSecondEmitted = totalMass / duration;
			farter.Begin();

			AkisTwitchEvents.Instance.durationMarker.Show(duration, LeakyCursorSafeEvent.markerColor);

			go.SetActive(true);
		}
	}
}
