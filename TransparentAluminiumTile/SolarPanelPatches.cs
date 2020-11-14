using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace TransparentAluminium
{
    public class SolarPanelPatches
    {
        [HarmonyPatch(typeof(Generator))]
        [HarmonyPatch("WattageRating", MethodType.Getter)]
        public static class Generator_WattageRating_Patch
        {
            public static void Postfix(Generator __instance, ref float __result)
            {
                if(__instance.PrefabID() == SolarPanelRoadConfig.ID)
                {
                    float Efficiency = Traverse.Create(__instance).Property("Efficiency").GetValue<float>();
                    __result = __instance.GetComponent<UpgradeableSolarPanel>().MaxWatt * Efficiency;
                }
            }
        }
    }
}
