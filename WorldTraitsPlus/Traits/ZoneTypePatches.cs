/*using Harmony;
using ProcGen;
using System;
using System.Linq;
using UnityEngine;
using static ProcGen.SubWorld;

namespace WorldTraitsPlus.Traits
{
    class ZoneTypePatches
    {
        static ZoneType lushZone;
        static int lushIndex;

        [HarmonyPatch(typeof(GroundRenderer), "OnPrefabInit")]
        public static class GroundRenderer_OnPrefabInit_Patch
        {
            public static void Postfix(ref GroundMasks.BiomeMaskData[] ___biomeMasks)
            {
                GroundMasks.BiomeMaskData data = ___biomeMasks.FirstOrDefault(b => b.name == "boggymarsh");
                Array.Resize(ref ___biomeMasks, lushIndex + 1);
                ___biomeMasks[lushIndex] = ___biomeMasks.FirstOrDefault(b => b.name == "boggymarsh");
                Traverse.Create(___biomeMasks[lushIndex]).Method("Regenerate").GetValue();
            }
        }

        // forcing our own enum into the subworlds zonetype
        [HarmonyPatch(typeof(OfflineWorldGen), "Generate")]
        public static class OfflineWorldGen_Generate_Patch
        {
            public static void Prefix()
            {
                lushIndex = Enum.GetNames(typeof(ZoneType)).Length;
                lushZone = (ZoneType)lushIndex;
                typeof(SubWorld).GetProperty("zoneType").SetValue(SettingsCache.subworlds["subworlds/Lush"], lushZone, null);
            }
        }

        // defining zone color
        [HarmonyPatch(typeof(SubworldZoneRenderData), "GenerateTexture")]
        public static class SubworldZoneRenderData_GenerateTexture_Patch
        {
            public static void Prefix(SubworldZoneRenderData __instance)
            {
                Array.Resize(ref __instance.zoneColours, lushIndex + 1);
                __instance.zoneColours[lushIndex] = new Color32(96, 179, 71, 1);
            }
        }
    }
}
*/