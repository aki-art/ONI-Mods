/*using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrueTiles.Patches
{
    internal class TEst
    {

        [HarmonyPatch(typeof(PropertyTextures), "UpdateDanger")]
        public static class PropertyTextures_UpdateDanger_Patch2
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                MethodInfo GetDangerForElement = AccessTools.Method(typeof(PropertyTextures_UpdateDanger_Patch2), "GetDangerForElement", new Type[] { typeof(int), typeof(int) });

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
            private static byte GetDangerForElement(int existingValue, int cell)
            {
                return (Grid.Element[cell].id == SimHashes.Steam) ? (byte)0 : (byte)existingValue;
            }
        }
    }
}*/
