using Klei;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using System;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class SubworldLoader(SubworldTemplate template, string sourceMod) : TemplateLoaderBase<SubworldTemplate>(template, sourceMod)
	{
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
			result.features ??= new List<Feature>();
			result.tags ??= new List<string>();
			result.biomes ??= new List<WeightedBiome>();
			result.samplers ??= new List<SampleDescriber>();
			result.featureTemplates ??= new Dictionary<string, int>();
			result.pdWeight = template.PdWeight.CalculateOrDefault(1f);
			result.borderSizeOverride = new MinMax(1f, 2.5f);
			result.minEnergy = template.MinEnergy.CalculateOrDefault(0);

			return result;
		}

		public void LoadContent(Dictionary<string, SubWorld> subWorlds)
		{
			Log.Debug($"loading {template.Id}");

			var subWorld = Get();

			YamlIO.Save(subWorld, "C:/Users/Aki/Desktop/yaml tests/subworldtest.yaml");

			if (Enum.TryParse(template.ZoneType, out ZoneType zoneType))
				subWorld.zoneType = zoneType;
			//else if (ZoneTypeUtil.TryParse(template.ZoneType, out var moonletZoneType))
			//subWorld.zoneType = (ZoneType)moonletZoneType;
			else
			{
				Warn($"{template.ZoneType} is not a registered ZoneType. Create a ZoneType by configuring is in moonlet/data/worldgen/zonetypes");
				subWorld.zoneType = ZoneType.Sandstone;
			}

			if (Enum.TryParse<Temperature.Range>(template.TemperatureRange, out var temperature))
				subWorld.temperatureRange = temperature;
			else
			{
				Warn($"{template.TemperatureRange} is not a registered TemperatureRange. Create a TemperatureRange by configuring is in moonlet/data/worldgen/temperatures.yaml");
				subWorld.temperatureRange = Temperature.Range.Room;
			}

			subWorld.EnforceTemplateSpawnRuleSelfConsistency();

			subWorlds[template.Id] = subWorld;

			SettingsCache.noise.LoadTree(subWorld.biomeNoise);
			SettingsCache.noise.LoadTree(subWorld.densityNoise);
			SettingsCache.noise.LoadTree(subWorld.overrideNoise);

			if (subWorld.centralFeature != null)
				Mod.loadFeatures.Add(subWorld.centralFeature.type);

			foreach (var biome in subWorld.biomes)
				Mod.loadBiomes.Add(biome.name);

			subWorld.features ??= new();

			foreach (var feature in subWorld.features)
				Mod.loadFeatures.Add(feature.type);
		}

		public override void RegisterTranslations()
		{
		}
	}
}
