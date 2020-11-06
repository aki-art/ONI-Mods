using FUtility;
using Harmony;
using System.Collections.Generic;
using UnityEngine;
using WorldTraitsPlus.Traits;
using WorldTraitsPlus.Traits.WorldEvents;

namespace WorldTraitsPlus
{
    class GenericPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Log.PrintVersion();
                ModAssets.Initialize();
            }
        }


        [HarmonyPatch(typeof(GlobalAssets), "OnPrefabInit")]
        public static class GlobalAssets_OnPrefabInit_Patch
        {
            public static void Postfix(Dictionary<string, string> ___SoundTable)
            {
/*                foreach (var item in ___SoundTable)
                {
                    Debug.Log($"{item.Key} | {item.Value}");
                }*/
            }
        }


        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnSpawn_Patch
        {
            public static void Postfix()
            {
                GameObject go = new GameObject();
                TraitsManager traitsManager = go.AddComponent<TraitsManager>();
                WorldEventManager worldEventManager = go.AddComponent<WorldEventManager>();
                //WorldEventClock worldEventclock = go.AddComponent<WorldEventClock>();

                traitsManager.Initialize();
                worldEventManager.Initialize();
                worldEventManager.ScheduleNext();
            }
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }
        }

        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public static class Game_DestroyInstances_Patch
        {
            public static void Prefix()
            {
                TraitsManager.DestroyInstance();
            }
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }
    }
}
