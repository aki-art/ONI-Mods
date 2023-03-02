using GravitasBigStorage.Content;
using HarmonyLib;
using UnityEngine;

namespace GravitasBigStorage.Patches
{
    internal class LonelyMinionHouseConfigPatch
    {

        [HarmonyPatch(typeof(LonelyMinionHouseConfig), "ConfigureBuildingTemplate")]
        public class LonelyMinionHouseConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddOrGet<Analyzable>();
            }
        }
    }
}
