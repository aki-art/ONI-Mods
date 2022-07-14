using HarmonyLib;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CompactMenus.Patches
{
    public class BuildMenuBuildingsScreenPatch
    {
        [HarmonyPatch(typeof(BuildMenuBuildingsScreen), "OnSpawn")]
        public class BuildMenuBuildingsScreen_OnSpawn_Patch
        {
            public static void Prefix(BuildMenuBuildingsScreen __instance, LocText ___titleLabel, GridLayoutGroup ___gridSizer, List<KToggle> ___toggles)
            {
                ___gridSizer.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                ___gridSizer.constraintCount = 5;
                ___gridSizer.cellSize = new UnityEngine.Vector2(32f, 64f);
            }
        }


        [HarmonyPatch(typeof(BuildMenuBuildingsScreen), "Configure")]
        public class BuildMenuBuildingsScreen_Configure_Patch
        {
            public static void Postfix(BuildMenuBuildingsScreen __instance, LocText ___titleLabel, GridLayoutGroup ___gridSizer, List<KToggle> ___toggles)
            {
                foreach (var toggle in ___toggles)
                {
                    toggle.transform.localScale /= 2f;
                }
            }
        }
    }
}
