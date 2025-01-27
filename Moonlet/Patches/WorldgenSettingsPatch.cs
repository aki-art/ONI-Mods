using HarmonyLib;
using Klei.CustomSettings;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class WorldgenSettingsPatch
	{
		[HarmonyPatch(typeof(WorldGenSettings), MethodType.Constructor, [
			typeof(WorldPlacement),
			typeof(int),
			typeof(List<string>),
			typeof(List<string>),
			typeof(bool),
		])]
		public class WorldGenSettings_Ctor_Patch
		{
			public static void Prefix(WorldPlacement placement, List<string> worldTraits, List<string> storyTraits)
			{
				SwapTraits(placement.world, worldTraits);
				SwapStoryTraits(storyTraits);
			}
		}

		[HarmonyPatch(typeof(WorldGenSettings), MethodType.Constructor, [
			typeof(string),
			typeof(List<string>),
			typeof(List<string>),
			typeof(bool),
		])]
		public class WorldGenSettings_Ctor_Patch2
		{
			public static void Prefix(string worldName, List<string> worldTraits, List<string> storyTraits)
			{
				SwapTraits(worldName, worldTraits);
				SwapStoryTraits(storyTraits);
			}
		}

		private static void SwapStoryTraits(List<string> storyTraits)
		{
			var currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);

			var clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
			if (clusterData == null)
			{
				Log.Warn("Cluster data is null");
				return;
			}

			Mod.storyTraitsLoader.ApplyToActiveLoaders(loader =>
			{
				if (storyTraits.Contains(loader.template.OverrideId))
				{
					if (loader.template.Conditions.IsValid(clusterData.clusterTags))
					{
						Log.Debug($"swapping story trait {loader.template.OverrideId} to {loader.id}");
						storyTraits.Remove(loader.template.OverrideId);
						storyTraits.Add(loader.id);
					}
				}
			});
		}

		private static void SwapTraits(string worldId, List<string> worldTraits)
		{
			foreach (var swap in Mod.traitSwaps)
			{
				if (swap.worldId == worldId)
				{
					if (worldTraits.Contains(swap.originalTrait))
					{
						worldTraits.Remove(swap.originalTrait);
						worldTraits.Add(swap.replacementTrait);
					}
				}
			}
		}
	}
}
