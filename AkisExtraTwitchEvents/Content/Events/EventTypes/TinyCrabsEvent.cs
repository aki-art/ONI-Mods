using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class TinyCrabsEvent() : TwitchEventBase(ID)
	{
		public const string ID = "TinyCrabs";

		public override Danger GetDanger() => Danger.High;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var go = new GameObject("TinyCrabSpawner");
			go.AddComponent<TinyCrabSpawner>();
			go.SetActive(true);

			ToastManager.InstantiateToastWithPosTarget(
				STRINGS.AETE_EVENTS.TINYCRABS.TOAST,
				STRINGS.AETE_EVENTS.TINYCRABS.DESC,
				Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		}
	}
}
