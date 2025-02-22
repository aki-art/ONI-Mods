using ONITwitchLib;
using Twitchery.Content.Scripts.Touchers;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes.ToucherEvents
{
	public class FreezeTouchEvent() : TwitchEventBase(ID)
	{
		public override int GetWeight() => Consts.EventWeight.Common;

		public const string ID = "FreezeTouch";

		public override Danger GetDanger() => Danger.Medium;

		public override void Run()
		{
			var go = new GameObject("FreezeToucher");
			var toucher = go.AddComponent<FreezeToucher>();
			toucher.lifeTime = ModTuning.FREEZE_TOUCH_DURATION;
			toucher.radius = 4f;
			toucher.cellsPerUpdate = 8;
			toucher.markerColor = Util.ColorFromHex("18a5ff");
			toucher.temperatureShift = -10;

			go.SetActive(true);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.FREEZETOUCH.TOAST,
				STRINGS.AETE_EVENTS.FREEZETOUCH.DESC);
		}
	}
}
