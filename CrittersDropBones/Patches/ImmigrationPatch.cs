using CrittersDropBones.Items;
using HarmonyLib;
using System.Collections.Generic;

namespace CrittersDropBones.Patches
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
                    new CarePackageInfo(SurimiConfig.ID, 6f, null)
                };

                ___carePackages = extraPackages.ToArray();
            }
        }
    }
}
