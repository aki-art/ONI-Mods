//using Harmony;
using HarmonyLib;
using System.Collections.Generic;
using TUNING;
using static ComplexRecipe;

namespace SpookyPumpkinSO.Foods
{
    class FoodPatches
    {
        [HarmonyPatch(typeof(CookingStationConfig), "ConfigureRecipes")]
        public static class Patch_CookingStationConfig_ConfigureRecipes
        {
            public static void Postfix()
            {
                AddPumpkinPieRecipe();
                AddToastedSeedsRecipe();
            }

            private static void AddPumpkinPieRecipe()
            {
                var input = new RecipeElement[]
                {
                    new RecipeElement(ColdWheatConfig.SEED_ID, 3f),
                    new RecipeElement(RawEggConfig.ID, 0.3f),
                    new RecipeElement(PumpkinConfig.ID, 2f)
                };

                var output = new RecipeElement[]
                {
                    new RecipeElement(PumkinPieConfig.ID, 1f)
                };

                string recipeID = ComplexRecipeManager.MakeRecipeID(GourmetCookingStationConfig.ID, input, output);

                PumkinPieConfig.recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = FOOD.RECIPES.STANDARD_COOK_TIME,
                    description = STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DESC,
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { CookingStationConfig.ID }
                };
            }

            private static void AddToastedSeedsRecipe()
            {
                var input = new RecipeElement[]
                {
                    new RecipeElement(PumpkinPlantConfig.SEED_ID, 2f),
                    new RecipeElement(TableSaltConfig.ID, 0.001f)
                };

                var output = new RecipeElement[]
                {
                    new RecipeElement(ToastedPumpkinSeedConfig.ID, 1f)
                };

                string recipeID = ComplexRecipeManager.MakeRecipeID(GourmetCookingStationConfig.ID, input, output);

                PumkinPieConfig.recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = FOOD.RECIPES.SMALL_COOK_TIME,
                    description = STRINGS.ITEMS.FOOD.SP_TOASTEDPUMPKINSEED.DESC,
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { CookingStationConfig.ID }
                };
            }
        }
    }
}
