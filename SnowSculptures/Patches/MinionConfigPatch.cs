using FUtility;
using HarmonyLib;
using SnowSculptures.Content;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace SnowSculptures.Patches
{
    public class MinionConfigPatch
    {

        [HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
        public class MinionConfig_CreatePrefab_Patch
        {
            public static void Postfix(GameObject __result)
            {
                SnowBeam.AddSnapOn(__result);
            }
        }


        [HarmonyPatch(typeof(MinionConfig), "SetupLaserEffects")]
        public class MinionConfig_SetupLaserEffects_Patch
        {
            public static void Postfix(GameObject prefab)
            {
                SnowBeam.AddLaserEffect(prefab);
            }

/*            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                foreach(var code in codes)
                {
                    Log.Debuglog($"{code.opcode} - {code.operand}");
                }
                var index = codes.FindLastIndex(c => c.Is(OpCodes.Stloc_S, 4)); 

                if (index == -1)
                {
                    Log.Debuglog("NO INDEX");
                    return codes;
                }

                var m_SetupLaserEffect = AccessTools.Method(typeof(SnowBeam), "SetupLaserEffect", new[] { typeof(MinionConfig.LaserEffect[]).MakeByRefType() });

                codes.InsertRange(index + 1, new[] {
                    new CodeInstruction(OpCodes.Ldloca, 4), // ref array
                    new CodeInstruction(OpCodes.Call, m_SetupLaserEffect)
                });

                Log.Debuglog("PATCHED");
                Log.PrintInstructions(codes);

                return codes;
            }*/
        }
    }
}
