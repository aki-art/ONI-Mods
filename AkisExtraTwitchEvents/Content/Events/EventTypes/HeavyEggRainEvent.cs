using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	class HeavyEggRainEvent() : TwitchEventBase(ID)
	{
		public const string ID = "HeavyEggRain";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var startPosition = PosUtil.ClampedMouseWorldPos();

			var go = new GameObject("AETE_HeavyRainSpawner");
			var spawner = go.AddComponent<ThingSpawner>();
			spawner.transform.position = startPosition;
			spawner.minCount = 30;
			spawner.maxCount = 50;
			spawner.prefabTags = [GenericEggCometConfig.ID];
			spawner.radius = 8;
			spawner.minDelay = 0.5f;
			spawner.maxDelay = 1f;
			spawner.soundFx = ModAssets.Sounds.POP;
			spawner.sceneLayer = Grid.SceneLayer.FXFront2;
			spawner.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
			spawner.followCursor = true;
			spawner.volume = 6f;
			spawner.Begin((cell, _) => GridUtil.IsCellEmpty(cell));

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.HEAVYEGGRAIN.TOAST, STRINGS.AETE_EVENTS.HEAVYEGGRAIN.DESC);
		}
	}
}
