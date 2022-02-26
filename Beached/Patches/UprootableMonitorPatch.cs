using Beached.Entities.Plants.Bamboo;
using HarmonyLib;

namespace Beached.Patches
{
    internal class UprootableMonitorPatch
    {
        [HarmonyPatch(typeof(UprootedMonitor), "IsSuitableFoundation")]
        public static class ToppleMonitor_IsSuitableFoundation_Patch
        {
            public static bool Prefix(UprootedMonitor __instance, int cell, ref bool __result)
            {
                if (__instance is ToppleMonitor toppleMonitor)
                {
                    return toppleMonitor.IsSuitableFoundation(cell, out __result, toppleMonitor);
                }

                return true;
            }
        }
    }
}
