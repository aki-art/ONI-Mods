using Harmony;
using System.Collections.Generic;

namespace Slag.Tweaks
{
    class CarePackages
    {
        [HarmonyPatch(typeof(Immigration))]
        [HarmonyPatch("ConfigureCarePackages")]
        public static class Immigration_ConfigureCarePackages_Patch
        {
            public static void Postfix(ref CarePackageInfo[] ___carePackages)
            {
                var extraPackages = new List<CarePackageInfo>(___carePackages)
                {
                    new CarePackageInfo(ElementLoader.FindElementByName("Slag").tag.ToString(), 2500f, null)
                };

                ___carePackages = extraPackages.ToArray();
            }
        }
    }
}
