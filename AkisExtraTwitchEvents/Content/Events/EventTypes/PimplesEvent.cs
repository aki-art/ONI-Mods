using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class PimplesEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Pimples";

		public static int spawnRange = 8;

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var spawner = new GameObject("AETE_PimpleSpawner").AddComponent<ThingSpawner>();
			spawner.transform.position = PosUtil.ClampedMouseWorldPos();
			spawner.minCount = 4;
			spawner.maxCount = 10;
			spawner.prefabTags = [PimpleConfig.ID];
			spawner.radius = spawnRange;
			spawner.minDelay = 0.1f;
			spawner.maxDelay = 0.3f;
			spawner.soundFx = ModAssets.Sounds.POP;
			spawner.sceneLayer = Grid.SceneLayer.FXFront2;
			spawner.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
			spawner.followCursor = true;
			spawner.volume = 2f;

			spawner.Begin(IsCellValid);

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.PIMPLES.TOAST, STRINGS.AETE_EVENTS.PIMPLES.DESC);
		}

		private bool IsCellValid(int cell)
		{
			return !Mod.pimples.Any(p => Grid.PosToCell(p) == cell);
		}
	}
}
