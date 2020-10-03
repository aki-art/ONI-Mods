using FUtility;
using Harmony;
using UnityEngine;

namespace ModularStorage
{
    class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                ModAssets.ModPath = path;
                Log.PrintVersion();
            }
        }

        [HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
        public static class Database_BuildingStatusItems_CreateStatusItems_Patch
        {
            public static void Postfix()
            {
                ModAssets.MakeStatusItems();
            }
        }

        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnSpawn_Patch
        {
            public static void Postfix()
            {
                Game.Instance.gameObject.AddComponent<ModularStorageManager>();
            }
        }


        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                Buildings.RegisterBuildings(
                    typeof(buildings.TestModuleConfig),
                    typeof(buildings.DebugControllerConfig)
                    );
            }
        }

    }
}