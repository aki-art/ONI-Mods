﻿using HarmonyLib;
using System.Collections.Generic;

namespace SpookyPumpkin.Patches
{
    public class ImmigrationPatch
    {
        [HarmonyPatch(typeof(Immigration))]
        [HarmonyPatch("ConfigureCarePackages")]
        public static class Immigration_ConfigureCarePackages_Patch
        {
            public static void Postfix(ref CarePackageInfo[] ___carePackages)
            {
                List<CarePackageInfo> extraPackages = new List<CarePackageInfo>(___carePackages)
                {
                    new CarePackageInfo(PumpkinPlantConfig.SEED_ID, 3f, null),
                    new CarePackageInfo(Foods.PumkinPieConfig.ID, 3f, null),
                    new CarePackageInfo(Foods.ToastedPumpkinSeedConfig.ID, 12f, null),
                    new CarePackageInfo(Foods.PumpkinConfig.ID, 5f, null)
                };

                ___carePackages = extraPackages.ToArray();
            }
        }
    }
}