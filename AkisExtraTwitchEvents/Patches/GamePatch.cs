using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Patches
{
	public class GamePatch
	{
		[HarmonyPatch(typeof(Game), nameof(Game.InitializeFXSpawners))]
		public class Game_InitializeFXSpawners_Patch
		{
			public static void Prefix(Game __instance)
			{
				if (__instance.fxSpawnData == null)
				{
					Log.Warning("No spawn data");
					return;
				}

				var spawnData = new List<Game.SpawnPoolData>(__instance.fxSpawnData);
				var prefab = spawnData.Find(d => d.id == SpawnFXHashes.OxygenEmissionBubbles).fxPrefab;

				if (prefab == null)
				{
					Log.Warning("FX prefab not found.");
					return;
				}

				spawnData.Add(new Game.SpawnPoolData()
				{
					id = ModAssets.Fx.pinkPoof,
					initialCount = 1,
					spawnOffset = Vector3.zero,
					spawnRandomOffset = new Vector2(0.1f, 0.1f),
					colour = Color.white,
					fxPrefab = GetNewPrefab(prefab, "aete_pink_poof_kanim"),
					initialAnim = "poof"
				});

				spawnData.Add(new Game.SpawnPoolData()
				{
					id = ModAssets.Fx.fungusPoof,
					initialCount = 1,
					spawnOffset = Vector3.zero,
					spawnRandomOffset = new Vector2(0.1f, 0.1f),
					colour = Color.white,
					fxPrefab = GetNewPrefab(prefab, "aete_fungus_poof_kanim"),
					initialAnim = "poof"
				});


				__instance.fxSpawnData = spawnData.ToArray();
			}

			private static GameObject GetNewPrefab(GameObject original, string newAnim = null, float scale = 1f)
			{
				var prefab = Object.Instantiate(original);
				var kbac = prefab.GetComponent<KBatchedAnimController>();

				if (!newAnim.IsNullOrWhiteSpace())
					kbac.AnimFiles[0] = Assets.GetAnim(newAnim);

				kbac.animScale *= scale;

				return prefab;
			}
		}
	}
}
