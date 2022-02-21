using CrittersDropBones.Effects;
using HarmonyLib;

namespace CrittersDropBones.Patches
{
    public class ModifierSetPatch
    {
        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                CDBEffects.RegisterAll(__instance);
            }
        }
    }
}
