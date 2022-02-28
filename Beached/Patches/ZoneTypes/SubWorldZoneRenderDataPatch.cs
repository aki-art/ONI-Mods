using HarmonyLib;
using System;
using UnityEngine;

namespace Beached.Patches.ZoneTypes
{
    internal class SubWorldZoneRenderDataPatch
    {
        [HarmonyPatch(typeof(SubworldZoneRenderData), "GenerateTexture")]
        public static class SubworldZoneRenderData_GenerateTexture_Patch
        {
            public static void Prefix(SubworldZoneRenderData __instance)
            {
                Array.Resize(ref __instance.zoneColours, __instance.zoneColours.Length + 3);
                __instance.zoneColours[(int)ModAssets.ZoneTypes.beach] = new Color32(211, 186, 157, 0);
                __instance.zoneColours[(int)ModAssets.ZoneTypes.depths] = Color.black;
                __instance.zoneColours[(int)ModAssets.ZoneTypes.bamboo] = Color.green;
            }
        }
    }
}
