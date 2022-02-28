using HarmonyLib;
using System;
using UnityEngine;

namespace DecorPackB.Patches
{
    internal class BuildingDefPatch
    {
        [HarmonyPatch( 
            typeof(BuildingDef), 
            "IsAreaClear",
            new Type[] { typeof(GameObject), typeof(int), typeof(Orientation), typeof(ObjectLayer), typeof(ObjectLayer), typeof(bool),  typeof(string) },
            new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal,  ArgumentType.Normal,  ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal,  ArgumentType.Out})]
        public class BuildingDefuildingDef_IsAreaClear_Patch
        {
            public static void Postfix(BuildingDef __instance, ref bool __result, int cell, Orientation orientation, ref string fail_reason)
            {
                if (!Grid.IsValidBuildingCell(cell))
                {
                    return;
                }

                if (__instance.BuildLocationRule == ModAssets.BuildLocationRules.OnAnyWall)
                {
                    __result = __result && CheckIfNextToWall(__instance, cell, orientation, ref fail_reason);
                }
            }
        }

        [HarmonyPatch(
            typeof(BuildingDef),
            "IsValidBuildLocation",
            new Type[] { typeof(GameObject), typeof(int), typeof(Orientation), typeof(string) },
            new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out })]
        public class BuildingDefuildingDef_IsValidBuildLocation_Patch
        {
            public static void Postfix(BuildingDef __instance, ref bool __result, int cell, Orientation orientation, ref string fail_reason)
            {
                if (!Grid.IsValidBuildingCell(cell))
                {
                    return;
                }

                if (__instance.BuildLocationRule == ModAssets.BuildLocationRules.OnAnyWall)
                {
                    __result = CheckIfNextToWall(__instance, cell, orientation, ref fail_reason);
                }
            }
        }

        private static bool CheckIfNextToWall(BuildingDef __instance, int cell, Orientation orientation, ref string fail_reason)
        {
            // If the building is not next to a wall
            if (!IsNextToWall(__instance.WidthInCells, __instance.HeightInCells, cell, orientation))
            {
                fail_reason = STRINGS.UI.TOOLTIPS.HELP_BUILDLOCATION_ON_ANY_WALL;
                return false;
            }

            return true;
        }

        // TODO: custom logic that is more efficient
        private static bool IsNextToWall(int width, int height, int cell, Orientation orientation)
        {
            return BuildingDef.CheckFoundation(cell, orientation, BuildLocationRule.OnFloor, width, height) ||
                BuildingDef.CheckFoundation(cell, orientation, BuildLocationRule.OnCeiling, width, height) ||
                BuildingDef.CheckWallFoundation(cell, width, height, true) ||
                BuildingDef.CheckWallFoundation(cell, width, height, false);
        }
    }
}
