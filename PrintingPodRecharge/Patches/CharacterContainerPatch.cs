using HarmonyLib;
using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Patches
{
    public class CharacterContainerPatch
    {
        [HarmonyPatch(typeof(CharacterContainer), "SetAnimator")]
        public class CharacterContainer_SetAnimator_Patch
        {
            public static void Postfix(KBatchedAnimController ___animController, MinionStartingStats ___stats)
            {
                if (HairDye.rolledHairs.TryGetValue(___stats, out var hairColor))
                {
                    ___animController.SetSymbolTint("snapto_hair", hairColor);
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
    }
}
