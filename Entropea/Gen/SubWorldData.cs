using FUtility;
using KSerialization.Converters;
using ProcGen;
using System.Collections.Generic;
using static ProcGen.SampleDescriber;
using static ProcGen.SubWorld;

namespace Entropea.Gen
{
    public class SubWorldData
    {
        public List<SampleDescriber> samplers { get; }
        public ZoneType zoneType { get; }
        public float minEnergy { get; set; }
        public int iterations { get; set; }
        public Dictionary<string, int> featureTemplates { get; set; }
        public Dictionary<string, string[]> pointsOfInterest { get; set; }
        public List<WeightedBiome> biomes { get; set; }
        public int minChildCount { get; set; }
        public List<string> tags { get; set; }
        public Override overrides { get; set; }
        public List<Feature> features { get; set; }
        public Feature centralFeature { get; set; }
        [StringEnumConverter]
        public Temperature.Range temperatureRange { get; set; }
        public string borderOverride { get; set; }
        public string densityNoise { get; set; }
        public string overrideNoise { get; set; }
        public string biomeNoise { get; set; }
        public float pdWeight { get; }
        public MinMax density { get; set; }
        public float avoidRadius { get; set; }
        [StringEnumConverter]
        public PointGenerator.SampleBehaviour sampleBehaviour { get; set; }
        public bool doAvoidPoints { get; set; }
        public bool dontRelaxChildren { get; set; }


        public SubWorldData(bool addDummyBiome = false)
        {

            zoneType = ZoneType.CrystalCaverns;
            Log.Debuglog($"Chosen zonetype: {zoneType}");

            biomeNoise = "noise/SandstoneStart";
            temperatureRange = Temperature.Range.Room;
            pdWeight = 2;
            density = new MinMax(10, 20);
            avoidRadius = 20;

            minChildCount = 6;
            doAvoidPoints = false;
            dontRelaxChildren = true;
            sampleBehaviour = PointGenerator.SampleBehaviour.PoissonDisk;

            tags = new List<string>();
            biomes = new List<WeightedBiome>();

            if(addDummyBiome)
            {
                var testBiome = new WeightedBiome("biomes/Frozen/Wet", 1)
                {
                    tags = new List<string>()
                    {
                        "Hatch",
                        "LeafyPlant",
                        "SeaLettuceSeed"
                    }
                };

                biomes.Add(testBiome);
            }

        }
    }
    public class WeightedBiome : IWeighted
    {
        public WeightedBiome(string name, float weight)
        {
            this.name = name;
            this.weight = weight;
        }

        public string name { get; }
        public float weight { get; set; }
        public List<string> tags { get; set; }
    }
}