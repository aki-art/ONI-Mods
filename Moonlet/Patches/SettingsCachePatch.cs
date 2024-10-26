using HarmonyLib;
using Klei;
using Moonlet.TemplateLoaders;
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

				Mod.subWorldsLoader.ApplyToActiveLoaders(item => item.LoadContent(SettingsCache.subworlds, false));
				Mod.clustersLoader.ApplyToActiveLoaders(item => item.LoadContent(SettingsCache.clusterLayouts.clusterCache));
				Mod.featuresLoader.ApplyToActiveLoaders(item => item.LoadContent());
				Mod.biomesLoader.ApplyToActiveLoaders(item => item.LoadContent());
				Mod.libNoiseLoader.ApplyToActiveLoaders(item => item.LoadContent());

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

				if (ClusterLoader.referencedWorldsNotLoadedWithMoonlet != null)
					SettingsCache.worlds.LoadReferencedWorlds(ClusterLoader.referencedWorldsNotLoadedWithMoonlet, errors);


				//if (WorldLoader.referencedSubWorldsNotLoadedWithMoonlet != null)
				//	SettingsCache.subworlds.LoadReferencedWorlds(ClusterLoader.referencedWorldsNotLoadedWithMoonlet, errors);


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
				Mod.borderLoader.ApplyToActiveLoaders(item => item.LoadContent());
				Mod.mobsLoader.ApplyToActiveLoaders(item => item.LoadContent());
				Mod.temperaturesLoader.ApplyToActiveLoaders(item => item.LoadContent());
				Mod.subworldMixingLoader.ApplyToActiveLoaders(item => item.LoadContent());

				Mod.clustersLoader.ApplyToActiveLoaders(template => template.ValidateWorldGen());
				Mod.worldsLoader.ApplyToActiveLoaders(template => template.ValidateWorldGen());
				Mod.subWorldsLoader.ApplyToActiveLoaders(template => template.ValidateWorldGen());
				Mod.libNoiseLoader.ApplyToActiveLoaders(template => template.ValidateWorldGen());

				var metals = new List<string>();

				Mod.elementsLoader.ApplyToActiveLoaders(template =>
				{
					if (template.IsMetal())
						metals.Add(template.id);
				});

				AddElementsToTrait("MetalPoor", metals);
				AddElementsToTrait("MetalRich", metals);

				if (SettingsCache.worlds?.worldCache != null)
				{
					foreach (var world in SettingsCache.worlds.worldCache)
					{
						if (world.Value == null || world.Value.disableWorldTraits || world.Value.worldTraitRules == null)
							continue;

						foreach (var rules in world.Value.worldTraitRules)
						{
							rules.forbiddenTags ??= [];
							rules.forbiddenTags.Add("DoNotGenerate");
						}
					}
				}
			}

			private static void AddElementsToTrait(string traitId, IEnumerable<string> elements)
			{
				if (SettingsCache.worldTraits.TryGetValue(traitId, out var trait))
				{
					if (trait.elementBandModifiers == null || trait.elementBandModifiers.Count == 0)
					{
						Log.Warn($"Could not add Moonlet metals to {traitId}.");
						return;
					}

					var massMultiplier = trait.elementBandModifiers[0].massMultiplier;
					var bandMultiplier = trait.elementBandModifiers[0].bandMultiplier;

					foreach (var metal in elements)
					{
						trait.elementBandModifiers.Add(new WorldTrait.ElementBandModifier()
						{
							element = metal,
							massMultiplier = massMultiplier,
							bandMultiplier = bandMultiplier
						});
					}
				}
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

				foreach (var template in Mod.subWorldsLoader.GetLoaders())
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
				Mod.traitsLoader.ApplyToActiveLoaders(item => item.LoadContent(SettingsCache.worldTraits));
			}
		}
	}
}
