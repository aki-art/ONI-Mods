using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class MidasTouchEvent() : TwitchEventBase("MidasTouch")
	{
		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => WEIGHTS.COMMON;

		public override void Run()
		{
			var go = new GameObject("MidasToucher");
			var midasToucher = go.AddComponent<MidasToucher>();
			midasToucher.lifeTime = ModTuning.MIDAS_TOUCH_DURATION;
			midasToucher.radius = 3f;
			midasToucher.cellsPerUpdate = 2;
			midasToucher.markerColor = Util.ColorFromHex("ffe373");

			go.SetActive(true);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.MIDASTOUCH.TOAST,
				STRINGS.AETE_EVENTS.MIDASTOUCH.DESC);

		}
	}
}
