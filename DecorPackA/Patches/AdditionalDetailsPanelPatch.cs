using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace DecorPackA.Patches
{
	public class AdditionalDetailsPanelPatch
	{
		// Makes the buildings info panel show the correct thermal conductivity for stained glass tiles
		public static void Patch(Harmony harmony)
		{
			var original = AccessTools.Method(typeof(AdditionalDetailsPanel), "RefreshDetailsPanel");
			var transpiler = AccessTools.Method(typeof(AdditionalDetailsPanel_RefreshDetails_Patch), "Transpiler", new Type[]
			{
				typeof(ILGenerator), typeof(IEnumerable<CodeInstruction>)
			});

			harmony.Patch(original, null, null, new HarmonyMethod(transpiler));
		}

		public static class AdditionalDetailsPanel_RefreshDetails_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var insulation = typeof(AdditionalDetailsPanelPatch).GetMethod("GetExtraInsulation");
				var selectedTarget = AccessTools.Field(typeof(TargetScreen), "selectedTarget");
				var thermalConductivity = AccessTools.Field(typeof(Element), "thermalConductivity");

				var codes = orig.ToList();
				var index = codes.FindIndex(c => c.operand is FieldInfo m && m == thermalConductivity);

				if (index > -1)
				{
					index++;
					codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
					codes.Insert(index++, new CodeInstruction(OpCodes.Ldfld, selectedTarget));
					codes.Insert(index++, new CodeInstruction(OpCodes.Call, insulation));
				}

				return codes;
			}
		}

		public static float GetExtraInsulation(float tc, GameObject obj)
		{
			if (obj != null && obj.TryGetComponent(out DyeInsulator insulator))
				return insulator.Modifier * tc;

			return tc;
		}
	}
}
