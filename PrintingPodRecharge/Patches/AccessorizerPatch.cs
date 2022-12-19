﻿using Database;
using HarmonyLib;
using PrintingPodRecharge.Cmps;
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

                //var f_clothingItems = AccessTools.Field(typeof(Accessorizer), "clothingItems");

                var m_ReplacementMethod = AccessTools.Method(typeof(DupeGenHelper2), "AlterBodyData", new[]
                {
                    typeof(Accessorizer),
                    typeof(KCompBuilder.BodyData),
                    //typeof(List<ResourceRef<ClothingItemResource>>)
                });

                codes.InsertRange(index + 1, new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, m_bodyData.GetGetMethod()),
                    //new CodeInstruction(OpCodes.Ldarg_0),
                    //new CodeInstruction(OpCodes.Ldfld, f_clothingItems),
                    new CodeInstruction(OpCodes.Call, m_ReplacementMethod)
                });

                return codes;
            }
        }

        [HarmonyPatch(typeof(Accessorizer), "UpdateHairBasedOnHat")]
        public class Accessorizer_UpdateHairBasedOnHat_Patch
        {
            public static void Prefix(Accessorizer __instance)//, List<ResourceRef<ClothingItemResource>> ___clothingItems)
            {
                DupeGenHelper2.AlterBodyData(__instance, __instance.bodyData);//, ___clothingItems);
            }
        }

        [HarmonyPatch(typeof(Accessorizer), "OnSpawn")]
        public class Accessorizer_OnSpawn_Patch
        {
            public static void Prefix(Accessorizer __instance)
            {
                CustomDupe.UpdateIdentity(__instance.GetComponent<MinionIdentity>());
            }

            public static void Postfix(Accessorizer __instance)
            {
                //DupeGenHelper2.AlterBodyData(__instance, __instance.bodyData);
                __instance.UpdateHairBasedOnHat();
            }
        }
    }
}
