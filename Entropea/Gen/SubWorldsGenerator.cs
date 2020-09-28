using FUtility;
using ProcGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Entropea.Gen.WorldGenerator;

namespace Entropea.Gen
{
    public class SubWorldsGenerator
    {
        public static Dictionary<string, SubWorldData> subworlds;
        public static List<SubWorldInfo> subworldinfos;
        public static string path;
        private const byte SUBWORLDS_MIN = 10;
        private const byte SUBWORLDS_MAX = 20;
        public const string STARTER_NAME = "starter";
        public static string GetPath(string name) => $"subworlds/{worldId.ToString()}/{name}"; // do not use for filepaths

        public static void Initialize()
        {
            subworlds = new Dictionary<string, SubWorldData>();
            Directory.CreateDirectory(System.IO.Path.Combine(ModAssets.ModPath, "worldgen", "subworlds", worldId.ToString()));
        }

        public static void GenerateStarter()
        {
            var central = new Feature
            {
                type = "features/generic/StartLocation"
            };

            var tags = new List<string>() {
                "Hatch",
                "LeafyPlant",
                "SeaLettuceSeed"
            };

            var starterBiome = new BiomeGenerator(true, 293.15f, "starter");
            var testBiome = new WeightedBiome(starterBiome.path + "/Basic", 1)
            {
                tags = tags
            };


            var starter = new SubWorldData()
            {
                tags = new List<string>()
                {
                    "IgnoreCaveOverride",
                    "NoGlobalFeatureSpawning",
                    "StartWorld"
                },

                centralFeature = central,
                borderOverride = "rocky",
                biomeNoise = "noise/SandstoneStart",
                temperatureRange = Temperature.Range.Room,
            };

            starter.biomes.Add(testBiome);

            subworlds.Add(GetPath(STARTER_NAME), starter);
            ModAssets.SaveYAML(starter, System.IO.Path.Combine("worldgen", "subworlds", worldId.ToString(), STARTER_NAME + ".yaml"));
        }

        internal static void GenerateStarterSubworld()
        {

        }

        internal static void GenerateSubWorlds()
        {
            int subWorldCount = random.RandomRange(SUBWORLDS_MIN, SUBWORLDS_MAX);
            Log.Debuglog($"Generating {subWorldCount} Subworlds");
            subworldinfos = new List<SubWorldInfo>();
            for (int i = 0; i < subWorldCount; i++)
            {
                var temp = WeightedRandom.Choose(Tuning.temperatures, random);
                subworldinfos.Add(new SubWorldInfo(i.ToString(), temp.Range, false));
            }
            throw new NotImplementedException();
        }

        internal static SubWorldData GenerateNew(SubWorldInfo sub)
        {
            SubWorldData sw = new SubWorldData();
            subworlds.Add(GetPath(sub.path), sw);
            return sw;
        }

        public class SubWorldInfo
        {
            public bool isStarter;
            public string id;
            public Temperature.Range temperature;
            public float specificTemperature;
            public string path;
            public List<Tag> tags;
            public BiomeGenerator[] biomes;

            public SubWorldInfo(string id, Temperature.Range temperature, bool isStarter = false)
            {
                this.id = id;
                this.isStarter = isStarter;
                this.temperature = temperature;
                path = "subworlds/" + worldId + "/" + id;
                specificTemperature = random.RandomRange(SettingsCache.temperatures[temperature].min, SettingsCache.temperatures[temperature].min);
                tags = new List<Tag>();
                biomes = new BiomeGenerator[1];
                biomes[0] = new BiomeGenerator(isStarter, specificTemperature, id);
            }


            public bool HasTag(Tag tag)
            {
                return tags.Contains(tag);
            }

            public void AddTag(Tag tag)
            {
                if (!HasTag(tag))
                    tags.Add(tag);
            }

        }
    }
}
