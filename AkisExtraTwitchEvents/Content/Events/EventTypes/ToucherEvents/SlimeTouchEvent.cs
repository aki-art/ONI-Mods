using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class SlimeTouchEvent : ITwitchEvent
	{
		public bool Condition(object data) => true;

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public string GetID() => "SlimeTouch";

		public void Run(object data)
		{
			var go = new GameObject("SlimeToucher");
			var toucher = go.AddComponent<SlimeToucher>();
			toucher.lifeTime = ModTuning.MIDAS_TOUCH_DURATION;
			toucher.radius = 3f;
			toucher.cellsPerUpdate = 4;
			toucher.morbChance = 0.01f;
			toucher.slimeBlockChance = 0.15f;
			toucher.markerColor = Util.ColorFromHex("45fc03");
			toucher.fungusChance = 0.75f;

			go.SetActive(true);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.SLIMETOUCH.TOAST,
				STRINGS.AETE_EVENTS.SLIMETOUCH.DESC);
		}
	}
}
