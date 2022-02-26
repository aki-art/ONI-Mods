using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Beached.Patches
{
    internal class PropertyTexturesPatch
    {
        [HarmonyPatch(typeof(PropertyTextures), "UpdateDanger")]
        public static class PropertyTextures_UpdateDanger_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                MethodInfo GetDangerForElement = AccessTools.Method(typeof(PropertyTextures_UpdateDanger_Patch), "GetDangerForElement", new Type[] { typeof(int), typeof(int) });

                List<CodeInstruction> codes = orig.ToList();

                var index = codes.FindIndex(ci => ci.operand is int i && i == (int)SimHashes.Oxygen); /// 34 ldc.i4 SimHashes.Oxygen

                if (index == -1)
                {
                    return codes;
                }

                index += 3;

                // byte.maxValue is loaded to the stack
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldloc_2)); // load num to the stack
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, GetDangerForElement)); // GetDangerForElement(byte.maxValue, num)

                Log.PrintInstructions(codes);

                return codes;
            }

            // Calling with existing value so there is a possibility for other mods to also add their own values
#pragma warning disable IDE0051 // Remove unused private members
            private static byte GetDangerForElement(int existingValue, int cell)
            {
                return (Grid.Element[cell].id == Elements.SaltyOxygen) ? (byte)0 : (byte)existingValue;
            }
#pragma warning restore IDE0051 // Remove unused private members
        }
    }
}
