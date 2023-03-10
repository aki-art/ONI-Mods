using ProcGen;
using Randomizer.Elements;
using System;
using System.Collections.Generic;

namespace Randomizer.Content.Scripts.Generators.YamlWorld
{
    public class BiomeGenerator : ProceduralGenerator<BiomeSettings>
    {
        private ElementComposition composition;

        public List<string> biomes;

        public BiomeGenerator(SeededRandom rng, ElementComposition composition, bool starter) : base(rng, "biomes", "biome")
        {
            this.composition = composition;
            biomes = new List<string>();
            var settings = new BiomeSettings
            {
                TerrainBiomeLookupTable = new()
            };

            var config = new ElementBandConfiguration();
            var bands = GenerateBand(rng, config, starter);
            settings.TerrainBiomeLookupTable.Add("Basic", config);

            SettingsCache.biomes.BiomeBackgroundElementBandConfigurations[FullName() + "/Basic"] = config;
            SettingsCache.biomeSettingsCache[FullName()] = settings;
            biomes.Add(FullName() + "/Basic");

            WriteYaml(settings, name);
        }

        private List<string> GenerateBand(SeededRandom rng, ElementBandConfiguration config, bool starter)
        {
            var bands = new List<string>()
            {
                ElementRole.MINERAL1
            };

            if (rng.RandomValue() < 0.33f) bands.Add(ElementRole.MINERAL2);
            if (rng.RandomValue() < 0.33f) bands.Add(ElementRole.MINERAL2);
            if (rng.RandomValue() < 0.75f) bands.Add(ElementRole.METAL1);
            if (rng.RandomValue() < 0.3f) bands.Add(ElementRole.METAL2);
            if (rng.RandomValue() < 0.3f) bands.Add(ElementRole.METAL2);
            if (starter || rng.RandomValue() < 0.2f) bands.Add(ElementRole.FARMABLE);
            if (starter || rng.RandomValue() < 0.2f) bands.Add(ElementRole.FARMABLE);
            if (rng.RandomValue() < 0.5f) bands.Add(ElementRole.GAS);
            if (rng.RandomValue() < 0.2f) bands.Add(ElementRole.GAS);
            if (!starter && rng.RandomValue() < 0.5f) bands.Add(ElementRole.LIQUID);

            bands.Shuffle();

            foreach (var band in bands)
            {
                var element = composition.GetRandom(band);
                if(element != null)
                {
                    config.Add(new ElementGradient(element.id.ToString(), rng.RandomValue(), new SampleDescriber.Override()));
                }
                else
                {
                    Log.Warning("could not generate element for configuration " + band);
                }
            }

            return bands;
        }
    }
}
