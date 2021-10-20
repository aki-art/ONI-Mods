using System.Collections.Generic;
using static ComplexRecipe;

namespace RockGrinder
{
    public class RecipeUtil
    {
        public static bool grindCritters;
        public static ComplexRecipe CreateRecipe(string ID, Tag input, float inputMass, RecipeElement[] result, string description)
        {
            var ingredients = new RecipeElement[]
            {
                new RecipeElement(input, inputMass)
            };

            string recipeID = ComplexRecipeManager.MakeRecipeID(ID, ingredients, result);

            ComplexRecipe recipe = new ComplexRecipe(recipeID, ingredients, result)
            {
                time = 30f,
                description = string.Format(description, ingredients[0].material.ProperNameStripLink(), result[0].material.ProperNameStripLink()),
                nameDisplay = RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag>()
                {
                    TagManager.Create(ID)
                }
            };

            return recipe;
        }
    }
}
