using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitchery.Content.Events;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class GamePatch
	{
		[HarmonyPatch(typeof(Game), nameof(Game.OnSpawn))]
		public class Game_OnSpawn_Patch
		{
			public static void Postfix()
			{
				TwitchEvents.OnGameReload();
			}
		}

		[HarmonyPatch(typeof(Game), "StepTheSim")]
		public class Game_StepTheSim_Patch
		{
			public static Color32 magmaColor;
			public static bool initializedColor;

			[HarmonyPriority(Priority.High + 1)] // sorry Sgt but I'm overriding your rainbows for a few seconds :D
			[HarmonyPrefix]
			public static void EarlyPrefix()
			{
				if (!initializedColor)
					magmaColor = ElementLoader.FindElementByHash(SimHashes.Magma).substance.colour;
			}

			// credit: https://github.com/Sgt-Imalas/Sgt_Imalas-Oni-Mods/blob/ced8e53f4e4cef8e04af3d1fae600fc7818c3f99/Imalas_TwitchChaosEvents/Patches.cs#L111
			[HarmonyPriority(Priority.High)]
			[HarmonyPostfix]
			public static void EarlyPostfix()
			{
				if (AkisTwitchEvents.Instance == null || !AkisTwitchEvents.Instance.hotTubActive)
					return;

				var pixelsPtr = PropertyTextures.externalLiquidTex;

				Parallel.For(
					0,
					Grid.CellCount,
					cell => ProcessPixel(pixelsPtr, cell, magmaColor.r, magmaColor.g, magmaColor.b));
			}

			private static unsafe void ProcessPixel(IntPtr pixelsPtr, int cell, byte r, byte g, byte b)
			{
				if (!Grid.IsActiveWorld(cell)) return;// || Grid.Solid[cell]) return;

				var pixel = (byte*)pixelsPtr.ToPointer() + (cell * 4);
				pixel[0] = r;
				pixel[1] = g;
				pixel[2] = b;
				pixel[3] = 255;
			}
		}

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

				spawnData.Add(new Game.SpawnPoolData()
				{
					id = ModAssets.Fx.slimeSplat,
					initialCount = 1,
					spawnOffset = Vector3.zero,
					spawnRandomOffset = new Vector2(0.1f, 0.1f),
					colour = Color.white,
					fxPrefab = GetNewPrefab(prefab, "aete_slime_splat_kanim"),
					initialAnim = "sploosh"
				});

				spawnData.Add(new Game.SpawnPoolData()
				{
					id = ModAssets.Fx.freezeMarker,
					initialCount = 1,
					spawnOffset = new Vector3(0.5f, 0.5f),
					spawnRandomOffset = Vector2.zero,
					colour = new Color(0.3f, 0.3f, 0.3f, 1),
					fxPrefab = GetNewPrefab(prefab, "aete_freezemarker_kanim"),
					initialAnim = "appear"
				});


				__instance.fxSpawnData = spawnData.ToArray();
			}

			private static GameObject GetNewPrefab(GameObject original, string newAnim = null, float scale = 1f)
			{
				var prefab = UnityEngine.Object.Instantiate(original);
				var kbac = prefab.GetComponent<KBatchedAnimController>();

				if (!newAnim.IsNullOrWhiteSpace())
					kbac.AnimFiles[0] = Assets.GetAnim(newAnim);

				kbac.animScale *= scale;

				return prefab;
			}


		}
	}
}
