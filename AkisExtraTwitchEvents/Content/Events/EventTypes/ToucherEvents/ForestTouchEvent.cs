using ONITwitchLib;
using Twitchery.Content.Scripts.Touchers;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes.ToucherEvents
{
	public class ForestTouchEvent() : TwitchEventBase(ID)
	{
		public override int GetWeight() => Consts.EventWeight.Common;

		public const string ID = "ForestTouch";

		public override Danger GetDanger() => Danger.Medium;

		public override void Run()
		{
			var go = new GameObject("ForestToucher");
			var toucher = go.AddComponent<ForestToucher>();
			toucher.lifeTime = ModTuning.MIDAS_TOUCH_DURATION;
			toucher.radius = 3f;
			toucher.cellsPerUpdate = 4;
			toucher.markerColor = Util.ColorFromHex("1c7016");

			go.SetActive(true);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.FORESTTOUCH.TOAST,
				STRINGS.AETE_EVENTS.FORESTTOUCH.DESC);
		}
	}
}
