using HarmonyLib;
using Moonlet.Scripts;
using ProcGen;
using System;
using static ProcGen.SubWorld;

namespace Moonlet.Patches
{
	public class SaveLoaderPatch
	{
		public static void Reload(string clusterId)
		{
			var clusterTags = SettingsCache.clusterLayouts.clusterCache[clusterId].clusterTags;

			Mod.elementsLoader.Reload(clusterId, clusterTags);
		}

		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), typeof(IReader))]
		public class SaveLoader_Load_Patch
		{
			public static void Postfix(SaveLoader __instance)
			{
				//Reload(__instance.GameInfo.clusterId);
			}
		}

		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.LoadFromWorldGen))]
		public class SaveLoader_LoadFromWorldGen_Patch
		{
			public static void Postfix(SaveLoader __instance)
			{
				//Reload(__instance.Cluster.Id);
			}
		}

		[HarmonyPatch(typeof(SaveLoader), "OnSpawn")]
		public class SaveLoader_OnSpawn_Patch
		{
			[HarmonyPriority(Priority.First)] // do this early so other mods can pack on top
			public static void Postfix(SaveLoader __instance)
			{
				if (Moonlet_Mod.Instance != null)
				{
					RestoreZoneTypes(__instance);
				}
			}

			private static void RestoreZoneTypes(SaveLoader saveLoader)
			{
				Moonlet_Mod.Instance.cachedZoneTypesIndices = [];
				foreach (var detail in saveLoader.clusterDetailSave.overworldCells)
				{
					if (((int)detail.zoneType + 1) <= Enum.GetValues(typeof(ZoneType)).Length)
						continue;

					if (Moonlet_Mod.Instance.cachedZoneTypesIndices.TryGetValue((int)detail.zoneType, out var expectedName))
					{
						var currentName = detail.zoneType.ToString();
						if (expectedName != currentName)
						{
							Log.Debug($"Mismatched zonetype.");
							try
							{
								var correctZoneType = (ZoneType)Enum.Parse(typeof(ZoneType), expectedName);
								detail.zoneType = correctZoneType;

								Log.Debug($" Restoring {currentName} ({(int)detail.zoneType}) to {expectedName} ({(int)correctZoneType})");
							}
							catch (Exception ex)
							{
								Log.Warn($"Could not restore zonetype {expectedName} from {currentName}. {ex}");

							}
						}
					}
					else
						Moonlet_Mod.Instance.cachedZoneTypesIndices[(int)detail.zoneType] = detail.zoneType.ToString();
				}
			}
		}
	}
}
