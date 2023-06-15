using Database;
using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace PrintingPodRecharge.Patches
{
    public class AccessorizerPatch
    {

        [HarmonyPatch(typeof(FullBodyUIMinionWidget), "SetDefaultPortraitAnimator")]
        public class FullBodyUIMinionWidget_SetDefaultPortraitAnimator_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_ApplyMinionPersonality = AccessTools.DeclaredMethod(typeof(Accessorizer), "ApplyMinionPersonality");

                // find injection point
                var index = codes.FindIndex(ci => ci.Calls(m_ApplyMinionPersonality));

                if (index == -1)
                    return codes;

                index--;

                var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(FullBodyUIMinionWidget_SetDefaultPortraitAnimator_Patch), "InjectedMethod");

                // inject right after the found index
                codes.InsertRange(index, new[]
                {
                    new CodeInstruction(OpCodes.Call, m_InjectedMethod)
                });

                return codes;
            }

            private static HashedString InjectedMethod(HashedString personalityId)
            {
                return Db.Get().Personalities.TryGet(personalityId) == null ? (HashedString)"MEEP" : personalityId;
            }
        }

        [HarmonyPatch(typeof(Accessorizer), "ApplyMinionPersonality")]
        public class Accessorizer_ApplyMinionPersonality_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                var m_bodyData = AccessTools.Property(typeof(Accessorizer), "bodyData");

                var index = codes.FindIndex(ci => ci.Calls(m_bodyData.GetSetMethod())); // stores body data

                if (index == -1)
                    return codes;

                var m_ReplacementMethod = AccessTools.DeclaredMethod(typeof(Accessorizer_ApplyMinionPersonality_Patch), "AlterBody");

                codes.InsertRange(index + 1, new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, m_ReplacementMethod)
                });

                return codes;
            }

            private static void AlterBody(Accessorizer accessorizer)
            {
                if(accessorizer == null || !accessorizer.TryGetComponent(out MinionIdentity identity) || identity.personalityResourceId == null)
                    return;

                DupeGenHelper2.AlterBodyData(accessorizer, accessorizer.bodyData);
            }
        }

        [HarmonyPatch(typeof(Accessorizer), "UpdateHairBasedOnHat")]
        public class Accessorizer_UpdateHairBasedOnHat_Patch
        {
            public static void Prefix(Accessorizer __instance)
            {
                if(__instance == null) 
                    return;

                DupeGenHelper2.AlterBodyData(__instance, __instance.bodyData);
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
                __instance.UpdateHairBasedOnHat();
            }
        }
    }
}
