using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class TEMPORARY_SlicksterRainEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SlicksterRain2";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var startPosition = PosUtil.ClampedMouseWorldPos();

			var go = new GameObject("AETE_SlicksterSpawner");
			var spawner = go.AddComponent<ThingSpawner>();
			spawner.transform.position = startPosition;
			spawner.minCount = 10;
			spawner.maxCount = 10;
			spawner.prefabTags = [OilFloaterConfig.ID];
			spawner.radius = 4;
			spawner.minDelay = 0.5f;
			spawner.maxDelay = 1f;
			spawner.soundFx = ModAssets.Sounds.POP;
			spawner.sceneLayer = Grid.SceneLayer.FXFront2;
			spawner.spawnFx = ModAssets.Fx.fungusPoof;
			spawner.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
			spawner.followCursor = true;
			spawner.volume = 6f;
			spawner.Begin((cell, _) => GridUtil.IsCellEmpty(cell));

			ToastManager.InstantiateToast(
				Strings.Get("STRINGS.ONITWITCH.TOASTS.SPAWN_PREFAB.TITLE"),
				string.Format(
					Strings.Get("STRINGS.ONITWITCH.TOASTS.SPAWN_PREFAB.BODY_FORMAT"),
					Util.StripTextFormatting(global::STRINGS.CREATURES.SPECIES.OILFLOATER.NAME)
				)
			);
		}
	}
}
