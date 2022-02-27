using HarmonyLib;
using System;
using UnityEngine;

namespace Beached.Patches
{
    internal class SubWorldZoneRenderDataPatch
    {
        [HarmonyPatch(typeof(SubworldZoneRenderData), "GenerateTexture")]
        public static class SubworldZoneRenderData_GenerateTexture_Patch
        {
            public static void Prefix(SubworldZoneRenderData __instance)
            {
                var beachIndex = (int)ModAssets.ZoneTypes.beach;
                var depthsIndex = (int)ModAssets.ZoneTypes.depths;

                Array.Resize(ref __instance.zoneColours, __instance.zoneColours.Length + 2);
                __instance.zoneColours[beachIndex] = new Color32(211, 186, 157, 0);
                __instance.zoneColours[depthsIndex] = Color.black;
            }
        }
    }
}
