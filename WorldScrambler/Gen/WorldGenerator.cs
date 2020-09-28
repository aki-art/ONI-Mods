using FUtility;
using Klei.CustomSettings;
using ProcGen;

namespace WorldScrambler.Gen
{
    public static class WorldGenerator
    {
        public static RandomWorld world;
        public static SeededRandom random;

        public static void Initialize()
        {
            world = new RandomWorld();
            world.GenerateDefault();
        }

        public static void Generate()
        {
            var start = System.DateTime.UtcNow;
            var worldSeed = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
            int seed = int.Parse(worldSeed.id);
            random = new SeededRandom(seed);
            Log.Info($"Generating randomized world. seed: [{seed}]");

            SettingsCache.LoadSubworlds(world.generatedWorld.subworldFiles, null);
            Log.Info($"Generation finished in {(System.DateTime.UtcNow - start).TotalMilliseconds} ms.");
        }
    }
}
