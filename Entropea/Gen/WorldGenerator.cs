using FUtility;
using Klei;
using Klei.CustomSettings;
using Newtonsoft.Json;
using ProcGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using static Entropea.Gen.WorldData;

namespace Entropea.Gen
{
    public class WorldGenerator
    {
        internal static WorldData world;
        internal static int worldId;
        public static string Name => "Entropea_" + worldId;
        public static string Path => "worlds/" + Name;
        internal static bool generated = false;
        internal static int seed = 0;
        public static SeededRandom random;
        public static List<SimHashes> usedElements;

        private static string FilePath => System.IO.Path.Combine(ModAssets.ModPath, "worldgen", Path + ".yaml");

        public static void CreateEmptyWorldFile(int id)
        {
            worldId = id;
            Log.Debuglog("Generating dummy world " + id);
            world = new WorldData(worldId, Name);
            ModAssets.SaveYAML(world, $"worldgen/{Path}.yaml");
        }

        internal static void Generate()
        {
            if (generated) return;
            var start = System.DateTime.UtcNow;
            var worldSeed = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
            seed = int.Parse(worldSeed.id);
            random = new SeededRandom(seed);

            Log.Info($"Generating randomized world. seed: [{seed}]");

            SubWorldsGenerator.Initialize();
            GenerateStarterSubWorld();

            GenerateTestWorld();
            SettingsCache.LoadSubworlds(world.subworldFiles, null);

            Log.Info($"Generation finished in {(System.DateTime.UtcNow - start).TotalMilliseconds} ms.");
        }

        public static void GenerateStarterSubWorld()
        {
            SubWorldsGenerator.GenerateStarter();

            world.startSubworldName = SubWorldsGenerator.GetPath(SubWorldsGenerator.STARTER_NAME);
            world.startingBaseTemplate = "sandstoneBase";
            world.noStart = false;
        }


        public static void GetDummyWorld()
        {
            ProcGen.World pgWorld = YamlIO.LoadFile<ProcGen.World>(FilePath);
            SettingsCache.worlds.worldCache[Path] = pgWorld;
        }

        public static void CleanTempFiles()
        {
            // Deleting unused world files
            var path = System.IO.Path.Combine(ModAssets.ModPath, "worldgen", "worlds");
            string[] files = new string[0];
            try
            {
                files = Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException obj)
            {
                Debug.LogWarning(obj);
            }

            foreach (string file in files)
            {
                string name = System.IO.Path.GetDirectoryName(file);
                Debug.Log("File: " + file);
                Debug.Log("name: " + name);
                if (!IsSaved(name))
                {
                    Debug.Log("This file is not used, deleting");
                    File.Delete(file);
                }
            }
        }

        public static bool IsSaved(string id)
        {
            if (id == "Entropea")
                return true;

            List<string> worlds = ReadSavedWorlds();
            if (worlds == null || !worlds.Any(w => w.Contains(id)))
                return false;
            else return true;
        }

        private static List<string> ReadSavedWorlds()
        {
            List<string> list = new List<string>();
            string filePath = System.IO.Path.Combine(ModAssets.ModPath, "config", "savedWorlds.json");

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    list = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}.");
                return null;
            }

            return list;
        }

        protected static void UpdateWorld()
        {
            ModAssets.SaveYAML(world, $"worldgen/worlds/{Name}.yaml");
            ProcGen.World pgWorld = YamlIO.LoadFile<ProcGen.World>(FilePath);
            SettingsCache.worlds.worldCache[Path] = pgWorld;
        }

        // Lazy way to convert types
        protected static void PushWorld(WorldData data, bool saveFile = true)
        {

            SerializerBuilder serializerBuilder = new SerializerBuilder();
            serializerBuilder.DisableAliases();
            string yamlData = serializerBuilder.Build().Serialize(data);

            DeserializerBuilder deserializerBuilder = new DeserializerBuilder();
            ProcGen.World content = deserializerBuilder.Build().Deserialize<ProcGen.World>(yamlData);

            SettingsCache.worlds.worldCache[Path] = content;

            if (saveFile)
                ModAssets.SaveYAML(data, $"worldgen/{Path}.yaml");
        }

        public static void GenerateTestWorld()
        {
            //SubWorldsGenerator.GenerateSubWorlds();
            world.subworldFiles.Clear();
            world.subworldFiles = new List<WeightedName>()
             {
                 new WeightedName("subworlds/sandstone/Sandstone", 1),
                 new WeightedName("subworlds/space/Space", 1),
                 new WeightedName("subworlds/ocean/Ocean", 1),
                 new WeightedName("subworlds/space/Surface", 1),
                 new WeightedName("subworlds/test/testSubWorld", 1),
                 new WeightedName("subworlds/test/starter", 1)

             };



            var sandstone = new AllowedCellsFilter
            {
                tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.Default.ToString(),
                command = ProcGen.World.AllowedCellsFilter.Command.Replace.ToString(),
                subworldNames = new List<string>()
                 {
                     "subworlds/sandstone/Sandstone"
                 }
            };

            var space = new AllowedCellsFilter
            {
                tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.AtTag.ToString(),
                command = ProcGen.World.AllowedCellsFilter.Command.Replace.ToString(),
                tag = "AtSurface",
                subworldNames = new List<string>()
                 {
                     "subworlds/space/Space"
                 }
            };

            var surface = new AllowedCellsFilter
            {
                tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag.ToString(),
                command = ProcGen.World.AllowedCellsFilter.Command.Replace.ToString(),
                tag = "AtSurface",
                minDistance = 1,
                maxDistance = 1,
                subworldNames = new List<string>()
                 {
                     "subworlds/space/Surface"
                 }
            };

            var ocean = new AllowedCellsFilter
            {
                tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag.ToString(),
                command = ProcGen.World.AllowedCellsFilter.Command.Replace.ToString(),
                tag = "AtSurface",
                minDistance = 2,
                maxDistance = 2,
                subworldNames = new List<string>()
                 {
                     "subworlds/ocean/Ocean"
                 }
            };
            var test = new AllowedCellsFilter
            {
                tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag.ToString(),
                command = ProcGen.World.AllowedCellsFilter.Command.Replace.ToString(),
                tag = "AtStart",
                minDistance = 3,
                maxDistance = 3,
                subworldNames = new List<string>()
                 {
                     "subworlds/test/testSubWorld"
                 }
            };

            world.unknownCellsAllowedSubworlds.Clear();
            world.unknownCellsAllowedSubworlds = new List<AllowedCellsFilter>()
             {
                 sandstone,
                 space,
                 surface,
                 ocean,
                 test
             };

            PushWorld(world);
            generated = true;

        }

        internal static void CleanUp()
        {
            if (!File.Exists(FilePath)) return;
            try
            {
                File.Delete(FilePath);
                Log.Debuglog($"We didn't use the world after all. Removed file at {FilePath}.");
            }
            catch (Exception e)
            {
                Log.Debuglog($"Couldn't delete file at {FilePath}, {e}");
            }

            generated = false;
        }
    }
}
