using HarmonyLib;
using Klei.AI;
using System;

namespace Beached.Patches
{
    public class MinionVitalsPanelPatch
    {
        [HarmonyPatch(typeof(MinionVitalsPanel), "Init")]
        public static class MinionVitalsPanel_Init_Patch
        {
            public static void Prefix(MinionVitalsPanel __instance)
            {
                Traverse.Create(__instance)
                    .Method("AddAmountLine", new Type[] { typeof(Amount), typeof(Func<AmountInstance, string>) })
                    .GetValue(Amounts.Moisture, null);
            }
        }
    }
}
