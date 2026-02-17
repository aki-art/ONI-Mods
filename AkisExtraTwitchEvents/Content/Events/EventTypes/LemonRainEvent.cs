using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs.Foods;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class LemonRainEvent() : TwitchEventBase(ID)
	{
		public const string ID = "LemonRain";

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var startPosition = PosUtil.ClampedMouseWorldPos();

			var go = new GameObject("AETE_LemonSpawner");
			var spawner = go.AddComponent<ThingSpawner>();
			spawner.transform.position = startPosition;
			spawner.minCount = 20;
			spawner.maxCount = 35;
			spawner.prefabTags = [LemonConfig.ID];
			spawner.radius = 6;
			spawner.minDelay = 0.2f;
			spawner.maxDelay = 0.5f;
			spawner.soundFx = ModAssets.Sounds.POP;
			spawner.sceneLayer = Grid.SceneLayer.FXFront2;
			spawner.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			spawner.followCursor = true;
			spawner.volume = 6f;
			spawner.Begin((cell, _) => GridUtil.IsCellEmpty(cell));

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.LEMONRAIN.TOAST, STRINGS.AETE_EVENTS.LEMONRAIN.DESC);
		}
	}
}
