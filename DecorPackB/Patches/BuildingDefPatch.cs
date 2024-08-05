using DecorPackB.Content.Db;
using DecorPackB.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace DecorPackB.Patches
{
	public class BuildingDefPatch
	{
		[HarmonyPatch(
			typeof(BuildingDef),
			"IsAreaClear",
			new[]
			{
				typeof(GameObject),
				typeof(int),
				typeof(Orientation),
				typeof(ObjectLayer),
				typeof(ObjectLayer),
				typeof(bool),
				typeof(string)
			},
			new[]
			{
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Out
			})]
		public class BuildingDefuildingDef_IsAreaClear_Patch
		{
			public static void Postfix(BuildingDef __instance, ref bool __result, int cell, Orientation orientation, ref string fail_reason)
			{
				if (!Grid.IsValidBuildingCell(cell))
				{
					return;
				}

				if (__instance.BuildLocationRule == ModDb.BuildLocationRules.GiantFossilRule)
				{
					__result = __result && IsValidFossilSpace(__instance, cell, orientation, ref fail_reason);
				}
			}
		}

		[HarmonyPatch(
			typeof(BuildingDef),
			"IsValidBuildLocation",
			new[]
			{
				typeof(GameObject),
				typeof(int),
				typeof(Orientation),
				typeof(bool),
				typeof(string)
			},
			new[]
			{
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Normal,
				ArgumentType.Out
			})]
		public class BuildingDefuildingDef_IsValidBuildLocation_Patch
		{
			public static void Postfix(BuildingDef __instance, ref bool __result, int cell, Orientation orientation, ref string fail_reason)
			{
				if (!Grid.IsValidBuildingCell(cell))
				{
					return;
				}

				if (__instance.BuildLocationRule == ModDb.BuildLocationRules.GiantFossilRule)
				{
					__result = IsValidFossilSpace(__instance, cell, orientation, ref fail_reason);
				}
			}
		}

		private static bool IsValidFossilSpace(BuildingDef instance, int cell, Orientation orientation, ref string fail_reason)
		{
			if (!GiantFossilCableVisualizer.isActivePreviewHangable && !IsOnFloor(instance, cell, orientation, ref fail_reason))
			{
				fail_reason = "Must be built on solid ground or hung from a ceiling";
				return false;
			}

			return true;
		}

		private static bool IsOnFloor(BuildingDef instance, int cell, Orientation orientation, ref string fail_reason)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, BuildLocationRule.OnFloor, instance.WidthInCells, instance.HeightInCells))
			{
				fail_reason = (string)global::STRINGS.UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
				return false;
			}

			return true;
		}
	}
}
