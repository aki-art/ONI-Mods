using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrittersDropBones.Patches
{
    internal class Test
    {
        [HarmonyPatch(typeof(AllResourcesScreen), "SpawnCategoryRow")]
        public static class Patch_MicrobeMusherConfig_ConfigureRecipes
        {
            public static void Prefix(AllResourcesScreen __instance, Tag categoryTag)
            {
                Log.Debuglog(categoryTag, "pre");
            }

            public static void Postfix(AllResourcesScreen __instance, Tag categoryTag)
            {
                Log.Debuglog(categoryTag, "UNITS ------------------------------------------------ ");
                foreach (var unit in __instance.units)
                {
                    //Log.Debuglog(unit.Key, unit.Value);
                }

                foreach (Tag tag in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(categoryTag))
                {
                    Log.Debuglog("    ", tag);
                }
            }
        }
    }
}
