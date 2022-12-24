using HarmonyLib;
using PrintingPodRecharge.Content;

namespace PrintingPodRecharge.Patches
{
    public class ModifierSetPatch
    {
        [HarmonyPatch(typeof(ModifierSet), "Initialize")]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                PEffects.Register(__instance);
            }
        }
    }
}