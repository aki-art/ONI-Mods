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



            [HarmonyPatch(typeof(Db), "Initialize")]
            public static class Db_Initialize_Patch
            {
                public static void Postfix()
                {
                    ModAssets.LateLoadAssets();
                }
            }


            [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
            public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
            {
                public static void Prefix()
                {
                    FUtility.Buildings.RegisterBuildings(
                        typeof(Buildings.SeismoGraphConfig),
                        typeof(Buildings.SeismicStabilizerConfig));
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
                    SeismicGrid.Initialize(SaveLoader.Instance.worldDetailSave.globalWorldSeed);
                    SaveLoader.Instance.gameObject.AddOrGet<WorldEventScheduler>();
                    SaveLoader.Instance.gameObject.AddOrGet<WorldDamager>();
                }
            }
        }
    }
}