using ProcGen;
using System;
using System.Collections.Generic;
using static Entropea.Elements;

namespace Entropea.Gen
{
    public class BiomeGenerator
    {
        public ComposableDictionary<string, ElementBandConfiguration> TerrainBiomeLookupTable { get; private set; }
        Dictionary<ElementRole, SimHashes> elements;
        readonly bool isStarter;
        public Temperature.Range temperature;
        public float specificTemperature;
        public string path;
        public string biomeId;

        public BiomeGenerator(bool starter, float temperature, string biomeId)
        {
            isStarter = starter;
            specificTemperature = temperature;
            Generate();
            path = "biomes/" + WorldGenerator.worldId + biomeId;
        }
        public void Generate()
        {
            ChooseElements(WorldGenerator.random);
            
            int iterations = UnityEngine.Random.Range(1, 3);
            var configuration = new ElementBandConfiguration(iterations * 2);
            for (int i = 0; i < iterations; i++)
            {
                ElementGradient primary = new ElementGradient(elements[ElementRole.Primary].ToString(), 0.25f, null);
                ElementGradient secondary = new ElementGradient(elements[ElementRole.Secondary].ToString(), 0.1f, null);
                ElementGradient tertiary = new ElementGradient(elements[ElementRole.Tertiary].ToString(), 0.05f, null);
                configuration.Add(primary);
                configuration.Add(secondary);
                configuration.Add(tertiary);
            }

            ElementGradient gas = new ElementGradient(elements[ElementRole.Tertiary].ToString(), 0.05f, null);
            configuration.Add(gas);

            TerrainBiomeLookupTable = new ComposableDictionary<string, ElementBandConfiguration>();
            TerrainBiomeLookupTable.Add("Basic", configuration);

            ModAssets.SaveYAML(this, System.IO.Path.Combine("worldgen", "biomes", WorldGenerator.worldId.ToString(), biomeId + ".yaml"));
        }

        public void ChooseElements(SeededRandom random)
        {
            byte digLevel = isStarter ? DigLevel.Soft : byte.MaxValue;
            AddElement(ElementRole.Primary, State.Solid, random, 1, digLevel);
            AddElement(ElementRole.Secondary, State.Solid, random, 1, digLevel);
            AddElement(ElementRole.Tertiary, State.Solid, random, 0.5f, digLevel);
            AddElement(ElementRole.Lakes, State.Liquid, random, 0.9f);
            AddElement(ElementRole.Gas, State.Liquid, random);
            AddElement(ElementRole.Pockets, State.Liquid, random, 0.5f);
        }

        private void AddElement(ElementRole role, State state, SeededRandom random, float chance = 1, byte digLevel = byte.MaxValue)
        {
            if (chance == 1 || random.RandomValue() <= chance)
                elements.Add(role, GetRandomElement(state, specificTemperature, digLevel, false, elements));
        }


        public enum Variation
        {
            Basic,
            Wet,
            Dry,
            Cold,
            Hot
        }

        public enum ElementRole
        {
            Primary,
            Secondary,
            Tertiary,
            Metal,
            Lakes,
            Gas,
            Pockets,
            Precious
        }
    }
}
