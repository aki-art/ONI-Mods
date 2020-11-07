using FUtility;
using Harmony;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
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


            [HarmonyPatch(typeof(HeadquartersConfig), "ConfigureBuildingTemplate")]
            public static class HeadquartersConfig_ConfigureBuildingTemplate_Patch
            {
                public static void Prefix()
                {

                }

                public static void Postfix(GameObject go)
                {
                    go.AddComponent<EarthQuake>();
                }
            }


            [HarmonyPatch(typeof(World), "OnSpawn")]
            public static class World_OnSpawn_Patch
            {
                public static void Postfix()
                {
                    //SaveLoader.Instance.gameObject.AddOrGet<WorldEventScheduler>();
                    //SaveLoader.Instance.gameObject.AddOrGet<PerWorldData>();
                    /*                    var eq = SaveGame.Instance.gameObject.GetComponent<EarthQuake>();
                                        if (eq != null)
                                            UnityEngine.GameObject.DestroyImmediate(eq);*/
                    //Assets.GetPrefab(HeadquartersConfig.ID).AddComponent<EarthQuake>();
                    //SaveGame.Instance.gameObject.AddOrGet<EarthQuake>();
                    //GameUtil.GetTelepad().AddOrGet<EarthQuake>();
                    //TraitsManager traitsManager = go.AddComponent<TraitsManager>();
                    // WorldEventManager worldEventManager = go.AddComponent<WorldEventManager>();
                    //WorldEventClock worldEventclock = go.AddComponent<WorldEventClock>();

                }
            }
        }
    }
}