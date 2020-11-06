using FUtility;
using Harmony;
using Newtonsoft.Json;
using System.IO;
using WorldCreep.Settings;
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

            [HarmonyPatch(typeof(World), "OnSpawn")]
            public static class World_OnSpawn_Patch
            {
                public static void Postfix()
                {
                    SaveGame.Instance.gameObject.AddComponent<WorldEventScheduler>();
                    SaveGame.Instance.gameObject.AddComponent<PerWorldData>();
                    SaveGame.Instance.gameObject.AddComponent<EarthQuake>();
                    //TraitsManager traitsManager = go.AddComponent<TraitsManager>();
                   // WorldEventManager worldEventManager = go.AddComponent<WorldEventManager>();
                    //WorldEventClock worldEventclock = go.AddComponent<WorldEventClock>();

                }
            }
        }
    }
}