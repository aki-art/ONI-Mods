using HarmonyLib;
using System.Collections.Generic;

namespace Beached.Patches
{
    public class GasLiquidExposureMonitorPatch
    {
        [HarmonyPatch(typeof(GasLiquidExposureMonitor), "InitializeCustomRates")]
        public class GasLiquidExposureMonitor_InitializeCustomRates_Patch
        {
            public static void Postfix(Dictionary<SimHashes, float> ___customExposureRates)
            {
                if (___customExposureRates is null)
                {
                    return;
                }

                ___customExposureRates[Elements.SaltyOxygen] = -0.25f;
                ___customExposureRates[Elements.Mucus] = 0f;
            }
        }
    }
}
