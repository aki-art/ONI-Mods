using DecorPackA.DPBuilding.StainedGlassTile;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace DecorPackA.Patches
{
    class AdditionalDetailsPanelPatch
    {
        // Makes the buildings info panel show the correct thermal conductivity for stained glass tiles
        [HarmonyPatch(typeof(AdditionalDetailsPanel), "RefreshDetails")]
        public static class AdditionalDetailsPanel_RefreshDetails_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var insulation = typeof(AdditionalDetailsPanelPatch).GetMethod("GetExtraInsulation");
                var selectedTarget = AccessTools.Field(typeof(TargetScreen), "selectedTarget");
                var thermalConductivity = AccessTools.Field(typeof(Element), "thermalConductivity");

                var codes = orig.ToList();
                var index = codes.FindIndex(c => c.operand is FieldInfo m && m == thermalConductivity);
                //var index = 290;

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
            {
                return insulator.Modifier * tc;
            }
            else return tc;
        }
    }
}
