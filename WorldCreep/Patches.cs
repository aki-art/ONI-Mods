using FUtility;
using Harmony;
using WorldCreep.WorldEvents;

namespace WorldCreep
{
    class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                ModAssets.ModPath = path;
                Log.PrintVersion();
                ModSettings.Load();
                ModSettings.SaveGlobalEventSettings();
            }


            [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
            public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
            {
                public static void Prefix()
                {
                    FUtility.Buildings.RegisterBuildings(typeof(Buildings.SeismoGraphConfig));
                }
            }

            [HarmonyPatch(typeof(World), "OnSpawn")]
            public static class World_OnSpawn_Patch
            {
                public static void Postfix()
                {
                    var worldSeed = CustomGameSettings.Instance.GetCurrentQualitySetting(Klei.CustomSettings.CustomGameSettingConfigs.WorldgenSeed);
                    int seed = int.Parse(worldSeed.id);
                    //random = new SeededRandom(seed);
                    SeismicGrid.Initialize(seed);
                    SaveLoader.Instance.gameObject.AddOrGet<WorldEventScheduler>();
                }
            }
        }
    }
}