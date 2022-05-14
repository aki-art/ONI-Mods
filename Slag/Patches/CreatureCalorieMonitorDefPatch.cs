using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Patches
{
    public class CreatureCalorieMonitorDefPatch
    {
        [HarmonyPatch(typeof(CreatureCalorieMonitor.Def), "GetDescriptors")]
        public class CreatureCalorieMonitor_Def_GetDescriptors_Patch
        {
            public static void Postfix(GameObject obj, ref List<Descriptor> __result)
            {
                if (obj.PrefabID() == HatchMetalConfig.ID)
                {
                    __result.Add(new Descriptor(
                        STRINGS.UI.DIET.EXTRA_PRODUCE
                            .Replace("{slag}", STRINGS.ELEMENTS.SLAG.NAME),
                        STRINGS.UI.DIET.EXTRA_PRODUCE_TOOLTIP
                            .Replace("{slag}", STRINGS.ELEMENTS.SLAG.NAME)
                            .Replace("{percent}", GameUtil.GetFormattedPercent(0.25f * 100f)))); // TODO: calculate percent
                }
            }
        }
    }
}
