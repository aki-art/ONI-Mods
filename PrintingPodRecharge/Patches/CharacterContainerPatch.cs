using FUtility;
using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
	public class CharacterContainerPatch
    {
        [HarmonyPatch(typeof(CharacterContainer), "SetAnimator")]
        public class CharacterContainer_SetAnimator_Patch
        {
            public static void Postfix(KBatchedAnimController ___animController, MinionStartingStats ___stats)
            {
                if (DupeGenHelper2.TryGetDataForStats(___stats, out var data))
                {
                    ___animController.SetSymbolTint("snapto_hair", data.hairColor);

                    var bleachedHair = Db.Get().AccessorySlots.Hair.Lookup(data.hairOverride);
                    ___animController.GetComponent<SymbolOverrideController>()
                        .AddSymbolOverride(Db.Get().AccessorySlots.Hair.targetSymbolId,
                        bleachedHair.symbol, 
                        10);
                }
            }
        }

        [HarmonyPatch(typeof(CharacterContainer), "GenerateCharacter")]
        public class CharacterContainer_GenerateCharacter_Patch
        {
            public static void Postfix(CharacterContainer __instance)
            {
                __instance.StartCoroutine(ImmigrantScreenPatch.TintCarePackageColorCoroutine(("Details/Top/PortraitContainer/BG", __instance)));
            }
        }

        // colors traits purple for vacillator traits, or yellow for need traits
        [HarmonyPatch(typeof(CharacterContainer), "SetInfoText")]
        public class CharacterContainer_SetInfoText_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_SetSimpleTooltip = AccessTools.Method(typeof(ToolTip), "SetSimpleTooltip");
                var index = codes.FindIndex(c => c.Calls(m_SetSimpleTooltip));

                if (index == -1)
                    return codes;

                var m_SetColorForTrait = AccessTools.Method(typeof(CharacterContainer_SetInfoText_Patch), "SetColorForTrait", new [] { typeof(LocText), typeof(Trait) });

                codes.InsertRange(index + 1, new[]
                {
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Ldloc_1),
                    new CodeInstruction(OpCodes.Call, m_SetColorForTrait)
                });

                Log.PrintInstructions(codes);

                return codes;
            }

            private static void SetColorForTrait(LocText locTest, Trait trait)
            {
                if (ModAssets.needTraits.Contains(trait.Id))
                {
                    locTest.color = ModAssets.Colors.gold;
                }
                else if (ModAssets.vacillatorTraits.Contains(trait.Id))
                {
                    locTest.colorGradient = new VertexGradient()
                    {
                        topLeft = ModAssets.Colors.purple,
                        topRight = ModAssets.Colors.purple,
                        bottomLeft = ModAssets.Colors.magenta,
                        bottomRight = ModAssets.Colors.magenta,
                    };

                    locTest.enableVertexGradient = true;
                    locTest.color = Color.white;
                }
            }
        }
    }
}
