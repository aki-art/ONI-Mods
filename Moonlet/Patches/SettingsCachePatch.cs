using HarmonyLib;
using Klei;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class SettingsCachePatch
	{
		[HarmonyPatch(typeof(SettingsCache), "LoadFiles", typeof(string), typeof(string), typeof(List<YamlIO.Error>))]
		public class SettingsCache_LoadFiles_Patch
		{
			public static void Prefix(string worldgenFolderPath, List<YamlIO.Error> errors)
			{
				Mod.subWorldsLoader.ApplyToActiveTemplates(item => item.LoadContent(SettingsCache.subworlds));
				Mod.clustersLoader.ApplyToActiveTemplates(item => item.LoadContent(SettingsCache.clusterLayouts.clusterCache));
				Mod.featuresLoader.ApplyToActiveTemplates(item => item.LoadContent(SettingsCache.featureSettings));

				foreach (var feature in Mod.loadFeatures)
				{
					if (!SettingsCache.featureSettings.ContainsKey(feature))
					{
						Log.Debug($"Feature {feature} was referenced in a Moonlet file, but not loaded from Moonlet.");
						SettingsCache.LoadFeature(feature, errors);
					}
				}

				foreach (var biome in Mod.loadBiomes)
				{
					if (!SettingsCache.biomeSettingsCache.ContainsKey(biome))
					{
						Log.Debug($"Biome {biome} was referenced in a Moonlet file, but not loaded from Moonlet.");
						SettingsCache.LoadFeature(biome, errors);

						if (!SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.ContainsKey(biome))
							Log.Warn($"{biome} could not be loaded.");
					}
				}
			}
		}

		[HarmonyPatch(typeof(SettingsCache), "LoadWorldTraits")]
		public class SettingsCache_LoadWorldTraits_Patch
		{
			public static void Postfix()
			{
				Mod.traitsLoader.ApplyToActiveTemplates(item => item.LoadContent(SettingsCache.worldTraits));
			}
		}

		[HarmonyPatch(typeof(SettingsCache), "LoadFeatures")]
		public class SettingsCache_LoadFeatures_Patch
		{
			public static void Postfix()
			{
				Log.Debug("LoadFeautres postfix");
			}
		}
	}
}
