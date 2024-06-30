using HarmonyLib;
using ProcGen;
using UnityEngine;

namespace Moonlet.Patches
{
	public class TEMP
	{


		[HarmonyPatch(typeof(ManagementMenu), "OnPrefabInit")]
		public class ManagementMenu_OnPrefabInit_Patch
		{
			public static void Prefix()
			{
				Log.Debug("ManagementMenu OnPrefabInit");

				Log.Debug("Loaded clusters:");
				foreach (var cluster in SettingsCache.clusterLayouts.clusterCache)
				{
					Log.Debug("\t - " + cluster.Key);
				}

				Log.Debug("Loaded worlds:");
				foreach (var world in SettingsCache.worlds.worldCache)
				{
					Log.Debug("\t - " + world.Key);
				}
			}
		}

		[HarmonyPatch(typeof(WorldTrait), nameof(WorldTrait.IsValid))]
		public class WorldTrait_isvalid_Patch
		{
			public static void Prefix(WorldTrait __instance, ProcGen.World world)
			{
				// TODO
				int num2 = 0;
				foreach (var item in __instance.globalFeatureMods)
				{
					Log.Debug($"{item.Key}, {item.Value}");
					num2 += Mathf.FloorToInt(world.worldTraitScale * (float)item.Value);
				}

			}
		}
	}
}
