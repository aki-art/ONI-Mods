using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class DeathLaserEvent() : TwitchEventBase(ID)
	{
		public const string ID = "DeathLaser";

		public override Danger GetDanger() => Danger.Deadly;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var go = new GameObject("geyser spawner");

			var cursor = go.AddComponent<WarningCursor>();

			cursor.OnTimerDoneFn += Fire;
			cursor.startDelaySeconds = 1.5f;
			cursor.endDelaySeconds = 0.1f;
			cursor.timer = 12f;
			cursor.disallowRocketInteriors = true;
			cursor.disallowProtectedCells = true;
			cursor.overTimer = 120f;
			cursor.animationFile = "aete_warning_long_kanim";
			cursor.snapToTile = true;

			go.SetActive(true);

			cursor.StartTimer();

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.DEATHLASER.TOAST,
				STRINGS.AETE_EVENTS.DEATHLASER.DESC);
		}

		private void Fire(Transform transform)
		{
			var laserGo = FUtility.Utils.Spawn(DeathLaserConfig.ID, PosUtil.ClampedMouseWorldPos());
			laserGo.SetActive(true);
		}
	}
}
