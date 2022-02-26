using System;
using FUtility;
using HarmonyLib;
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

                Array.Resize(ref __instance.zoneColours, __instance.zoneColours.Length + 1);
                __instance.zoneColours[beachIndex] = new Color32(221, 141, 89, 2);
            }
        }

        /*
        [HarmonyPatch(typeof(Assets), "Load")]
        public static class test
        {
            public static void Postfix()
            {
                var backWallArrayShader = Assets.instance.ShaderAssets.Find(s => s.name == "Klei/BackWallArray");
                if(backWallArrayShader != null)
                {
                    Log.Debuglog("backWallArrayShader EXISTS");
                }
                
            }
        }*/
    }
}
