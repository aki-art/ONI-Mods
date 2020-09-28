using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Entropea.WorldTraits
{
    class CometPatches
    {
        [HarmonyPatch(typeof(Comet), "Sim33ms")]
        public static class Comet_Sim33ms_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                var index = codes.FindIndex(ci => ci.operand is int i && i == (int)SimHashes.CarbonDioxide);
                var gameObject = AccessTools.Property(typeof(Comet), "gameObject").GetGetMethod();
                var name = AccessTools.Property(typeof(GameObject), "name").GetGetMethod();
                var stringEquals = AccessTools.Method(typeof(string), "Equals", new Type[] { typeof(string) });

                var COLabel = generator.DefineLabel();
                var startLabel = generator.DefineLabel();

                codes[index].labels.Add(COLabel);
                codes[index + 1].labels.Add(startLabel);
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, gameObject));
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, name));
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldstr, IceCometConfig.ID)); 
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, stringEquals));
                codes.Insert(index++, new CodeInstruction(OpCodes.Brfalse, COLabel));
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4, (int)SimHashes.Steam));
                codes.Insert(index++, new CodeInstruction(OpCodes.Br, startLabel));

                foreach (var codeInstruction in codes)
                {
                    Debug.Log(codeInstruction);
                }

                return codes;
            }
        }
    }
}
