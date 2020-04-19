using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ComplexRecipe;

namespace Slag.Food
{
    class Patches
    {
        [HarmonyPatch(typeof(GourmetCookingStationConfig), "ConfigureRecipes")]
        private static class Patch_GourmetCookingStationConfig_ConfigureRecipes
        {
            private static void Postfix()
            {
                var input = new RecipeElement[] 
                {
                    new RecipeElement(NoodlesConfig.ID, 1f),
                    new RecipeElement(FishMeatConfig.ID, 1f),
                    new RecipeElement(ElementLoader.FindElementByHash(SimHashes.Algae).tag, 3f)
                };

                var output = new RecipeElement[] 
                {
                    new RecipeElement(SeafoodPastaConfig.ID, 1f)
                };

                string recipeID = ComplexRecipeManager.MakeRecipeID(GourmetCookingStationConfig.ID, input, output);

                SeafoodPastaConfig.recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
                    description = "Seafood desc",
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { GourmetCookingStationConfig.ID }
                };
            }
        }

        [HarmonyPatch(typeof(CookingStationConfig), "ConfigureRecipes")]
        private static class Patch_CookingStationConfig_ConfigureRecipes
        {
            private static void Postfix()
            {
                var input = new RecipeElement[]
                {
                    new RecipeElement(NoodlesConfig.ID, 1f),
                    new RecipeElement(SpiceNutConfig.ID, 1f)
                };

                var output = new RecipeElement[]
                {
                    new RecipeElement(SpaghettiConfig.ID, 1f)
                };

                string recipeID = ComplexRecipeManager.MakeRecipeID(GourmetCookingStationConfig.ID, input, output);

                SeafoodPastaConfig.recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
                    description = "SPaghet desc",
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { CookingStationConfig.ID }
                };
            }
        }
    }
}
