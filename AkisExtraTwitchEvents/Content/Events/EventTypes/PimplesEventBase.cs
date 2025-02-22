using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public abstract class PimplesEventBase(string id) : TwitchEventBase(id)
	{
		public abstract int SpawnRange();

		public override void Run()
		{
			var startPosition = PosUtil.ClampedMouseWorldPos();

			var spawner = new GameObject("AETE_PimpleSpawner").AddComponent<ThingSpawner>();
			spawner.transform.position = startPosition;
			spawner.minCount = 4;
			spawner.maxCount = 10;
			spawner.prefabTags = [PimpleConfig.ID];
			spawner.radius = SpawnRange();
			spawner.minDelay = 0.1f;
			spawner.maxDelay = 0.3f;
			spawner.soundFx = ModAssets.Sounds.POP;
			spawner.sceneLayer = Grid.SceneLayer.FXFront2;
			spawner.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
			spawner.followCursor = true;
			spawner.volume = 6f;
			spawner.configureSpawnFn = ConfigureSpawner;

			spawner.Begin(IsCellValid);

			ToastManager.InstantiateToastWithPosTarget(STRINGS.AETE_EVENTS.PIMPLES.TOAST, STRINGS.AETE_EVENTS.PIMPLES.DESC, startPosition);
		}

		protected virtual void ConfigureSpawner(GameObject go)
		{
		}

		private bool IsCellValid(int cell)
		{
			return !Mod.pimples.Any(p => Grid.PosToCell(p) == cell);
		}
	}
}
