using ONITwitchLib;
using Twitchery.Content.Scripts.Touchers;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes.ToucherEvents
{
	public class VoidTouchEvent() : TwitchEventBase(ID)
	{
		public override int GetWeight() => Consts.EventWeight.Common;

		public const string ID = "VoidTouch";

		public override Danger GetDanger() => Danger.Medium;

		public override bool Condition()
		{
			return Db.Get().Techs.IsTechItemComplete(ExteriorWallConfig.ID);
		}

		public override void Run()
		{
			var go = new GameObject("VoidToucher");
			var toucher = go.AddComponent<VoidToucher>();
			toucher.lifeTime = ModTuning.MIDAS_TOUCH_DURATION;
			toucher.radius = 2f;
			toucher.cellsPerUpdate = 8;
			toucher.markerColor = Util.ColorFromHex("340d63");

			go.SetActive(true);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.VOIDTOUCH.TOAST,
				STRINGS.AETE_EVENTS.VOIDTOUCH.DESC);
		}
	}
}
