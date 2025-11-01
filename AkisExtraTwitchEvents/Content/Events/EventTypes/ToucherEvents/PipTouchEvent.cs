using ONITwitchLib;
using Twitchery.Content.Scripts.Touchers;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes.ToucherEvents
{
	public class PipTouchEvent() : TwitchEventBase(ID)
	{
		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public const string ID = "PipTouch";

		public override Danger GetDanger() => Danger.High;

		public override void Run()
		{
			var go = new GameObject("PipToucher");
			var toucher = go.AddComponent<PipToucher>();
			toucher.lifeTime = ModTuning.MIDAS_TOUCH_DURATION / 3f;
			toucher.radius = 2f;
			toucher.cellsPerUpdate = 4;
			toucher.markerColor = Util.ColorFromHex("60d100");

			go.SetActive(true);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.PIPTOUCH.TOAST,
				STRINGS.AETE_EVENTS.PIPTOUCH.DESC);
		}
	}
}
