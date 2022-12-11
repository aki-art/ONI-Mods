using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace PrintingPodRecharge.Patches
{
    public class AccessorizerPatch
    {
        [HarmonyPatch(typeof(Accessorizer), "ApplyMinionPersonality")]
        public class Accessorizer_ApplyMinionPersonality_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                var m_bodyData = AccessTools.Property(typeof(Accessorizer), "bodyData");

                var index = codes.FindIndex(ci => ci.Calls(m_bodyData.GetSetMethod())); // stores body data

                if (index == -1)
                {
                    return codes;
                }

                var m_ReplacementMethod = AccessTools.Method(typeof(DupeGenHelper2), "AlterBodyData", new[]
                {
                    typeof(Accessorizer),
                    typeof(KCompBuilder.BodyData)
                });

                codes.InsertRange(index + 1, new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, m_bodyData.GetGetMethod()),
                    new CodeInstruction(OpCodes.Call, m_ReplacementMethod)
                });

                return codes;
            }
        }


        [HarmonyPatch(typeof(Accessorizer), "UpdateHairBasedOnHat")]
        public class Accessorizer_UpdateHairBasedOnHat_Patch
        {
            public static void Prefix(Accessorizer __instance)
            {
                DupeGenHelper2.AlterBodyData(__instance, __instance.bodyData);
            }
        }

        [HarmonyPatch(typeof(Accessorizer), "OnSpawn")]
        public class Accessorizer_OnSpawn_Patch
        {
            public static void Postfix(Accessorizer __instance)
            {
                DupeGenHelper2.AlterBodyData(__instance, __instance.bodyData);
            }
        }
    }
}
