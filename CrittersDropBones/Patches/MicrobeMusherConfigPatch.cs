using CrittersDropBones.Items;
using HarmonyLib;
using System.Collections.Generic;
using TUNING;
using static ComplexRecipe;

namespace CrittersDropBones.Patches
{
    public class MicrobeMusherConfigPatch
    {
        [HarmonyPatch(typeof(MicrobeMusherConfig), "ConfigureRecipes")]
        public static class Patch_MicrobeMusherConfig_ConfigureRecipes
        {
            public static void Postfix()
            {
                var input = new RecipeElement[]
                {
                    new RecipeElement(FishMeatConfig.ID, 1f),
                    new RecipeElement(FishBoneConfig.ID, 1f),
                    new RecipeElement(ColdWheatConfig.SEED_ID, 2f)
                };

                var output = new RecipeElement[]
                {
                    new RecipeElement(SurimiConfig.ID, 1f)
                };

                var recipeID = ComplexRecipeManager.MakeRecipeID(MicrobeMusherConfig.ID, input, output);

                SurimiConfig.recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = FOOD.RECIPES.STANDARD_COOK_TIME,
                    description = STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SURIMI.DESC,
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { MicrobeMusherConfig.ID }
                };
            }
        }
    }
}
