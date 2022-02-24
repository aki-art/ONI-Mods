using HarmonyLib;

namespace Beached.Patches
{
    public class ModifierSetPatch
    {
        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                foreach (var effect in ModAssets.Effects.GetEffectsList())
                {
                    __instance.effects.Add(effect);
                }
            }
        }
    }
}
