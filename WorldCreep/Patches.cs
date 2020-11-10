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
                    SeismicGrid.Initialize();
                    SaveLoader.Instance.gameObject.AddOrGet<WorldEventScheduler>();
                }
            }
        }
    }
}