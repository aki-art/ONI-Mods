using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace PrintingPodRecharge.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }

            public static void Postfix()
            {
                // gene shuffler traits were marked as negative for some reason. Possibly an oversight.
                foreach (var trait in DUPLICANTSTATS.GENESHUFFLERTRAITS)
                {
                    Db.Get().traits.Get(trait.id).PositiveTrait = true;
                }

                for (var i = 0; i < Mod.Recipes.BioInks.Count; i++)
                {
                    var recipe = Mod.Recipes.BioInks[i];
                    var inputs = recipe.Inputs.Select(input => new ComplexRecipe.RecipeElement(input.ID, input.Amount)).ToArray();
                    var outputs = recipe.Outputs.Select(output => new ComplexRecipe.RecipeElement(output.ID, output.Amount)).ToArray();

                    CreateRecipe(CraftingTableConfig.ID, inputs, outputs, recipe.Description, 40f, i);
                }
            }

            public static ComplexRecipe CreateRecipe(string fabricatorID, ComplexRecipe.RecipeElement[] input, ComplexRecipe.RecipeElement[] output, string description, float time, int sortOrderOffset)
            {
                var recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, input, output);

                var desc = Strings.TryGet(description, out var result) ? result.String : description;

                var recipe = new ComplexRecipe(recipeID, input, output)
                {
                    time = time,
                    description = desc,
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                    fabricators = new List<Tag>
                    {
                        TagManager.Create(fabricatorID)
                    },
                    sortOrder = sortOrderOffset + 30
                };

                return recipe;
            }
        }
    }
}
