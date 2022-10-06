using HarmonyLib;
using UnityEngine;

namespace SpookyPumpkinSO.Content.GhostPip.Spawning
{
    internal class TelepadPatch
    {
        [HarmonyPatch(typeof(HeadquartersConfig), "ConfigureBuildingTemplate")]
        public static class HeadquartersConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddComponent<GhostPipSpawner>();
            }
        }

        [HarmonyPatch(typeof(ExobaseHeadquartersConfig), "ConfigureBuildingTemplate")]
        public static class ExobaseHeadquartersConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go, Tag prefab_tag)
            {
                go.AddComponent<GhostPipSpawner>();
            }
        }
    }
}
