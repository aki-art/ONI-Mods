using HarmonyLib;

namespace Slag.Patches
{
    public class ComplexRecipePatch
    {
        [HarmonyPatch(typeof(ComplexRecipe), "GetUIName")]
        public static class ComplexRecipe_GetUIName_Patch
        {
            public static void Postfix(ComplexRecipe __instance, ref string __result, string ___id)
            {
                if (__instance.nameDisplay == ModAssets.slagNameDisplay)
                {
                    __result = STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_NAME
                        .Replace("{Slag}", __instance.ingredients[2].material.ProperName())
                        .Replace("{Ore}", __instance.ingredients[0].material.ProperName())
                        .Replace("{Metal}", __instance.results[0].material.ProperName());
                }
            }
        }
    }
}
