using ProcGen;
using Randomizer.Elements;
using System;
using System.Collections.Generic;

namespace Randomizer.Content.Scripts.Generators.YamlWorld
{
    public class SubWorldGenerator : ProceduralGenerator<SubWorld>
    {
        private readonly float maxHardness;
        private readonly Temperature temperature;
        private readonly bool starter;
        private readonly List<string> tags;
        public ElementComposition composition;

        public SubWorldGenerator(SeededRandom rng, float maxHardness, Temperature.Range temperatureRange, bool starter = false, List<string> tags = null) : base(rng, "subworlds", "subworld")
        {
            this.maxHardness = maxHardness;
            this.temperature = SettingsCache.temperatures[temperatureRange];
            this.starter = starter;
            this.tags = tags;
            Generate();
        }

        private void Generate()
        {
            if (starter)
            {
                composition = new ElementComposition(
                    GameUtil.GetTemperatureConvertedToKelvin(10, GameUtil.TemperatureUnit.Celsius),
                    GameUtil.GetTemperatureConvertedToKelvin(60, GameUtil.TemperatureUnit.Celsius),
                    ElementCollector.DIG_HARDNESS.SOFT,
                    true,
                    ElementCollector.DIG_HARDNESS.SOFT,
                    ElementCollector.DIG_HARDNESS.MEDIUM);
            }
            else
            {
                composition = new ElementComposition(
                    temperature.min,
                    temperature.max,
                    maxHardness,
                    false,
                    ElementCollector.DIG_HARDNESS.MEDIUM,
                    ElementCollector.DIG_HARDNESS.SUPERHARD);
            }

            var features = new List<Feature>();
            var featureCount = rng.RandomRange(3, 7);
            for (int i = 0; i <= featureCount; i++)
            {
                var shapeCount = Enum.GetValues(typeof(ProcGen.Room.Shape)).Length;
                var shape = rng.RandomRange(0, shapeCount - 1);
                features.Add(new Feature()
                {
                    type = new FeatureGenerator(rng, 2, 7, (ProcGen.Room.Shape)shape, composition).FullName()
                });
            }

            var biomes = new BiomeGenerator(rng, composition, true);

            var subWorld = new SubWorld()
            {
                name = "Randomizer.STRINGS.WORLDS.RANDOM_PRESET_DEFAULT.NAME",
                descriptionKey = "Randomizer.STRINGS.WORLDS.RANDOM_PRESET_DEFAULT.NAME",
                utilityKey = "Randomizer.STRINGS.WORLDS.RANDOM_PRESET_DEFAULT.NAME",
                biomeNoise = "noise/Sandstone",
                temperatureRange = Temperature.Range.Room,
                tags = new()
                {
                    WorldGenTags.IgnoreCaveOverride.ToString(),
                    WorldGenTags.NoGlobalFeatureSpawning.ToString(),
                },
                density = new MinMax(10, 20),
                avoidRadius = 20f,
                doAvoidPoints = false,
                dontRelaxChildren = true,
                sampleBehaviour = PointGenerator.SampleBehaviour.PoissonDisk,
                biomes = new()
                {
                    new WeightedBiome(biomes.biomes[0], 1f)
                },
                zoneType = ModDb.zoneTypesNoSpace.GetRandom(),
                features = features
            };

            if (starter)
            {
                subWorld.tags.Add(WorldGenTags.StartWorld.ToString());
                subWorld.centralFeature = new()
                {
                    type = "features/generic/StartLocation"
                };
                subWorld.pdWeight = 6f;
            }

            if(tags != null)
            {
                subWorld.tags.AddRange(tags);
            }

            Log.Debuglog("Generated subworld: " + FullName());

            subWorld.EnforceTemplateSpawnRuleSelfConsistency();
            SettingsCache.subworlds[FullName()] = subWorld;
            SettingsCache.noise.LoadTree(subWorld.biomeNoise);
            SettingsCache.noise.LoadTree(subWorld.densityNoise);
            SettingsCache.noise.LoadTree(subWorld.overrideNoise);

            WriteYaml(subWorld, name);
        }
    }
}
