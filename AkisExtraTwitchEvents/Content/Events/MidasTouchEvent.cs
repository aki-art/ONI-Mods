using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
	public class MidasTouchEvent : ITwitchEvent
	{
		public bool Condition(object data) => true;

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public string GetID() => "MidasTouch";

		public void Run(object data)
		{
			var go = new GameObject("MidasToucher");
			var midasToucher = go.AddComponent<MidasToucher>();
			midasToucher.lifeTime = ModTuning.MIDAS_TOUCH_DURATION;
			midasToucher.radius = 3f;
			midasToucher.cellsPerUpdate = 2;
			midasToucher.markerColor = Util.ColorFromHex("ffe373");

			go.SetActive(true);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.MIDAS.TOAST,
				STRINGS.AETE_EVENTS.MIDAS.DESC);
		}
	}
}
