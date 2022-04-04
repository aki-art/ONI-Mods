using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace SpookyPumpkin
{
    public class MinionVitalsPanelPatch
    {
        // Fixes the display what fertilizer is needed for a plant, so it supports non-element fertilizers
        // TODO: what if another mod does this?
        [HarmonyPatch(typeof(MinionVitalsPanel), "GetFertilizationLabel")]
        public static class MinionVitalsPanel_GetFertilizationLabel_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                MethodInfo getElement = AccessTools.Method(typeof(ElementLoader), "GetElement", new Type[] { typeof(Tag) });
                MethodInfo getProperName = AccessTools.Method(typeof(GameTagExtensions), "ProperNameStripLink", new Type[] { typeof(Tag) });

                List<CodeInstruction> codes = orig.ToList();
                int index = codes.FindIndex(c => c.operand is MethodInfo m && m == getElement);

                if (index == -1)
                {
                    Log.Warning("Could not patch fertilization labels, another mod may have interfered. \n" +
                        "Rot as a fertilizer may not correctly display on info panels. (This is is only visual)");
                    return codes;
                }

                codes[index] = new CodeInstruction(OpCodes.Call, getProperName);
                codes.RemoveAt(index + 1);

                return codes;
            }
        }
    }
}
