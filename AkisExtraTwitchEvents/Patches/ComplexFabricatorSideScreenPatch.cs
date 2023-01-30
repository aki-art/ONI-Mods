using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
    public class ComplexFabricatorSideScreenPatch
    {
        [HarmonyPatch(typeof(ComplexFabricatorSideScreen), "AnyRecipeRequirementsDiscovered")]
        public class ComplexFabricatorSideScreen_AnyRecipeRequirementsDiscovered_Patch
        {
            public static void Postfix(ComplexRecipe recipe, ref bool __result)
            {
                if (recipe.id == AkisTwitchEvents.pizzaRecipeID)
                {
                    __result = AkisTwitchEvents.Instance == null || AkisTwitchEvents.Instance.hasUnlockedPizzaRecipe;
                }
            }
        }
    }
}
