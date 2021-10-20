using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpookyPumpkinSO.GhostPip.Spawning
{
    class TelepadPatch
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
