using HarmonyLib;
using Moonlet.Templates.SubTemplates;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using PeterHan.PLib.Core;
using ProcGen;
using System;
using System.Collections.Generic;
using static ProcGen.SubWorld;
using static ProcGen.World;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class SubworldLoader(SubworldTemplate template, string sourceMod) : TemplateLoaderBase<SubworldTemplate>(template, sourceMod), IWorldGenValidator
	{
		private SubWorld cachedSubworld;
		private const string NO_BORDER = "NONE";

		public override void Initialize()
		{
			id = $"subworlds{relativePath}";
			template.Id = id;
			base.Initialize();
		}

		public SubWorld Get()
		{
			var result = CopyProperties<SubWorld>();

			result.minChildCount = template.MinChildCount.CalculateOrDefault(2);
			result.borderOverridePriority = template.BorderOverridePriority.CalculateOrDefault(0);
			result.extraBiomeChildren = template.ExtraBiomeChildren.CalculateOrDefault(0);
			result.iterations = template.Iterations.CalculateOrDefault(0);
			result.features ??= [];
			result.tags ??= [];
			result.biomes ??= [];
			result.samplers ??= [];
			result.featureTemplates ??= [];
			result.pdWeight = template.PdWeight.CalculateOrDefault(1f);
			result.borderSizeOverride = new MinMax(1f, 2.5f);
			result.minEnergy = template.MinEnergy.CalculateOrDefault(0);
			result.name = template.Id;
			result.subworldTemplateRules = ShadowTypeUtil.CopyList<TemplateSpawnRules, TemplateSpawnRuleC>(template.SubworldTemplateRules, Issue);

			return result;
		}

		public void LoadContent(Dictionary<string, SubWorld> subWorlds, bool force)
		{
			if (template.Id == null)
			{
				Warn("Null subworld ID.");
				return;
			}

			if (!force && cachedSubworld != null && SettingsCache.subworlds.ContainsKey(template.Id))
				return;

			var subWorld = Get();

			if (Enum.TryParse(template.ZoneType, out ZoneType zoneType))
				subWorld.zoneType = zoneType;
			else if (ZoneTypeUtil.TryParse(template.ZoneType, out var moonletZoneType))
				subWorld.zoneType = (ZoneType)moonletZoneType;
			else
			{
				Warn($"{template.ZoneType} is not a registered ZoneType. Create a ZoneType by configuring it in moonlet/data/worldgen/zonetypes, or use one of the existing ones: " + Enum.GetNames(typeof(ZoneType)).Join());
				subWorld.zoneType = ZoneType.Sandstone;
			}

			if (Enum.TryParse<Temperature.Range>(template.TemperatureRange, out var temperature))
				subWorld.temperatureRange = temperature;
			else if (Mod.temperaturesLoader.ranges.TryGetValue(template.TemperatureRange, out var range))
				subWorld.temperatureRange = range;
			else
			{
				Warn($"{template.TemperatureRange} is not a registered TemperatureRange. Create a TemperatureRange by configuring it in moonlet/data/worldgen/temperatures.yaml");
				subWorld.temperatureRange = Temperature.Range.Room;
			}

			subWorld.EnforceTemplateSpawnRuleSelfConsistency();

			subWorlds[template.Id] = subWorld;

			Mod.loadNoise.Add(subWorld.biomeNoise);
			Mod.loadNoise.Add(subWorld.densityNoise);
			Mod.loadNoise.Add(subWorld.overrideNoise);

			if (subWorld.centralFeature != null)
				Mod.loadFeatures.Add(subWorld.centralFeature.type);

			foreach (var biome in subWorld.biomes)
				Mod.loadBiomes.Add(biome.name);

			subWorld.features ??= [];

			foreach (var feature in subWorld.features)
				Mod.loadFeatures.Add(feature.type);
		}

		public override void RegisterTranslations()
		{
		}

		public void ValidateWorldGen()
		{
			if (template.BiomeNoise != null && !SettingsCache.noise.trees.ContainsKey(template.BiomeNoise))
				Warn($"Issue with subWorld {id}: {template.BiomeNoise} is not a registered Noise.");

			if (template.DensityNoise != null && !SettingsCache.noise.trees.ContainsKey(template.DensityNoise))
				Warn($"Issue with subWorld {id}: {template.DensityNoise} is not a registered Noise.");

			if (template.OverrideNoise != null && !SettingsCache.noise.trees.ContainsKey(template.OverrideNoise))
				Warn($"Issue with subWorld {id}: {template.OverrideNoise} is not a registered Noise.");

			if (template.Biomes == null)
				Warn($"Issue with subWorld {id}: no biomes are defined.");
			else
			{
				foreach (var biome in template.Biomes)
				{
					if (!SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.ContainsKey(biome.name))
						Warn($"Issue with subWorld {id}: {biome.name} is not a registered Biome.");

				}
			}

			if (template.BorderOverride != null && template.BorderOverride != NO_BORDER && !SettingsCache.borders.ContainsKey(template.BorderOverride))
				Warn($"Issue with subWorld {id}: {template.BorderOverride} is not a registered Border.");

			if (template.ZoneType != null && !IsZonetypeValid(template.ZoneType))
				Warn($"Issue with subWorld {id}: {template.ZoneType} is not a registered ZoneType.");

			if (template.TemperatureRange != null && !IsTemperatureRangeValid(template.TemperatureRange))
				Warn($"Issue with subWorld {id}: {template.TemperatureRange} is not a registered TemperatureRange.");

			if (template.Features != null)
			{
				foreach (var feature in template.Features)
				{
					if (!SettingsCache.featureSettings.ContainsKey(feature.type))
						Warn($"Issue with subWorld {id}: {feature.type} is not a registered Feature.");
				}
			}

			if (template.SubworldTemplateRules != null)
				foreach (var templateRule in template.SubworldTemplateRules)
				{
					// TODO: should be its own template loader and checker
					foreach (var name in templateRule.Names)
					{
						if (!TemplateCache.TemplateExists(name))
							Issue($"{name} is not a registered Template.");
					}
				}

			if (template.CentralFeature != null && !SettingsCache.featureSettings.ContainsKey(template.CentralFeature.type))
				Warn($"Issue with subWorld {id}: {template.CentralFeature.type} is not a registered Feature.");
		}

		private bool IsElementValid(string elementId)
		{
			try
			{
				Enum.Parse(typeof(SimHashes), elementId);
				return true;
			}
			catch (Exception e)
			{
				Error(e.ToString());
				return false;
			}
		}

		private bool IsTemperatureRangeValid(string temperatureRange)
		{
			if (Enum.TryParse<Temperature.Range>(temperatureRange, out var _))
				return true;

			if (Mod.temperaturesLoader.ranges.ContainsKey(temperatureRange))
				return true;

			return false;
		}

		private bool IsZonetypeValid(string zoneType)
		{
			if (Enum.TryParse<ZoneType>(zoneType, true, out var _))
				return true;

			if (ZoneTypeUtil.TryParse(zoneType, out var _))
				return true;

			return false;
		}
	}
}
