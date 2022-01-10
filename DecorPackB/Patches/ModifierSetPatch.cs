using HarmonyLib;

namespace DecorPackB.Patches
{
    public class ModifierSetPatch
    {
        [HarmonyPatch(typeof(ModifierSet))]
        [HarmonyPatch(nameof(ModifierSet.Initialize))]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                foreach (Klei.AI.Effect effect in ModAssets.Effects.GetEffectsList())
                {
                    __instance.effects.Add(effect);
                }
            }
        }
    }
}
