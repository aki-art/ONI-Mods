using HarmonyLib;
using Klei;
using ObjectCloner;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Moonlet.Patches
{
	public class SettingsCachePatch
	{

		[HarmonyPatch(typeof(ProcGenGame.WorldGen), nameof(ProcGenGame.WorldGen.ReportWorldGenError))]
		public class ProcGenGame_WorldGen_ReportWorldGenError_Patch
		{
			public static void Prefix(Exception e, string errorMessage = null)
			{
				Log.Warn($"{e.Message} {errorMessage}");
			}
		}

		[HarmonyPatch(typeof(SettingsCache), nameof(SettingsCache.LoadFiles), typeof(string), typeof(string), typeof(List<YamlIO.Error>))]
		public class SettingsCache_LoadFiles_Patch
		{
			public static void Prefix(string worldgenFolderPath, List<YamlIO.Error> errors)
			{
				try
				{
					BeforeLoadingSettingsCache(errors);
				}
				// we really want to catch everything this time, because all throws are suppressed during worldgen
				catch (Exception e)
				{
					Log.Error(e);
				}
			}

			private static void BeforeLoadingSettingsCache(List<YamlIO.Error> errors)
			{
				var stopWatch = new Stopwatch();
				stopWatch.Start();

				Log.Debug("=========== WORLDGEN LOAD START =============");

				Mod.subWorldsLoader.ApplyToActiveTemplates(item => item.LoadContent(SettingsCache.subworlds, false));
				Mod.clustersLoader.ApplyToActiveTemplates(item => item.LoadContent(SettingsCache.clusterLayouts.clusterCache));
				Mod.featuresLoader.ApplyToActiveTemplates(item => item.LoadContent());
				Mod.biomesLoader.ApplyToActiveTemplates(item => item.LoadContent());
				Mod.libNoiseLoader.ApplyToActiveTemplates(item => item.LoadContent());

				foreach (var feature in Mod.loadFeatures)
				{
					if (!SettingsCache.featureSettings.ContainsKey(feature))
					{
						Log.Debug($"Feature {feature} was referenced in a Moonlet file, but not loaded from Moonlet.");
						SettingsCache.LoadFeature(feature, errors);

						if (!SettingsCache.featureSettings.ContainsKey(feature))
							Log.Warn($"{feature} not found.");
					}
				}

				foreach (var biome in Mod.loadBiomes)
				{
					if (!SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.ContainsKey(biome))
					{
						Log.Debug($"Biome {biome} was referenced in a Moonlet file, but not loaded from Moonlet.");
						SettingsCache.LoadBiome(biome, errors);

						if (!SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.ContainsKey(biome))
							Log.Warn($"{biome} not found.");
					}
				}

				foreach (var noise in Mod.loadNoise)
				{
					if (noise.IsNullOrWhiteSpace())
						continue;

					if (SettingsCache.noise.trees.ContainsKey(noise))
						continue;

					Log.Debug($"Biome {noise} was referenced in a Moonlet file, but not loaded from Moonlet.");

					if (SettingsCache.noise.LoadTree(noise) == null)
						Log.Warn($"{noise} not found.");
				}

				Log.Debug("=========== WORLDGEN LOAD END =============");

				stopWatch.Stop();
				Log.Info($"Moonlet loaded worlgen content in {stopWatch.ElapsedMilliseconds} ms");
			}

			public static void Postfix()
			{
				try
				{
					AfterLoadingSettingsCache();
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}

			private static void AfterLoadingSettingsCache()
			{
				Mod.borderLoader.ApplyToActiveTemplates(item => item.LoadContent());
				Mod.mobsLoader.ApplyToActiveTemplates(item => item.LoadContent());
				Mod.temperaturesLoader.ApplyToActiveTemplates(item => item.LoadContent());

				Mod.clustersLoader.ApplyToActiveTemplates(template => template.ValidateWorldGen());
				Mod.worldsLoader.ApplyToActiveTemplates(template => template.ValidateWorldGen());
				Mod.subWorldsLoader.ApplyToActiveTemplates(template => template.ValidateWorldGen());
				Mod.libNoiseLoader.ApplyToActiveTemplates(template => template.ValidateWorldGen());
			}
		}

		[HarmonyPatch(typeof(SettingsCache), nameof(SettingsCache.LoadSubworlds))]
		public class SettingsCache_LoadSubworlds_Patch
		{
			// remove moonlet subworld paths from before the yaml loading, which has no null checks and would crash
			// from these outsider files not in the "expected" location
			public static void Prefix(ref List<WeightedSubworldName> subworlds, ref object __state)
			{
				var state = new List<WeightedSubworldName>();

				foreach (var template in Mod.subWorldsLoader.GetTemplates())
				{
					for (int i = 0; i < subworlds.Count; i++)
					{
						var subworld = subworlds[i];
						if (template.id == subworld.name)
						{
							var entry = SerializingCloner.Copy(subworld);
							state.Add(entry);
							subworlds.RemoveAt(i);
						}
					}
				}

				__state = state;
			}

			public static void Postfix(ref List<WeightedSubworldName> subworlds, ref object __state)
			{
				if (__state is List<WeightedSubworldName> state && state.Count > 0)
					subworlds.AddRange(state);
			}
		}

		[HarmonyPatch(typeof(SettingsCache), nameof(SettingsCache.LoadWorldTraits))]
		public class SettingsCache_LoadWorldTraits_Patch
		{
			public static void Postfix()
			{
				Mod.traitsLoader.ApplyToActiveTemplates(item => item.LoadContent(SettingsCache.worldTraits));
			}
		}
	}
}
