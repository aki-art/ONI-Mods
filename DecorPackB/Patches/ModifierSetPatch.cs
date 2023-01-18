using DecorPackB.Content;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class ModifierSetPatch
    {
        [HarmonyPatch(typeof(ModifierSet), "Initialize")]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                DPEffects.Register(__instance);
            }
        }
    }
}
