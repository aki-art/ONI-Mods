using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes.ToucherEvents
{
	public class SlimeTouchEvent() : TwitchEventBase(ID)
	{
		public override int GetWeight() => Consts.EventWeight.Common;

		public const string ID = "SlimeTouch";

		public override Danger GetDanger() => Danger.Medium;

		public override void Run()
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
