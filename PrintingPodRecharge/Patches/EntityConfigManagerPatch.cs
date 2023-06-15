using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Items;
using System.Collections.Generic;
using System.Linq;

namespace PrintingPodRecharge.Patches
{
    public class EntityConfigManagerPatch
    {
        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Postfix()
            {
                for (var i = 0; i < Mod.Recipes.BioInks.Count; i++)
                {
                    var recipe = Mod.Recipes.BioInks[i];

                    if (recipe.Outputs[0].ID == BioInkConfig.MEDICINAL && !Mod.otherMods.IsDiseasesExpandedHere)
                        continue;

                    var inputs = recipe.Inputs.Select(input => new ComplexRecipe.RecipeElement(input.ID, input.Amount)).ToArray();
                    var outputs = recipe.Outputs.Select(output => new ComplexRecipe.RecipeElement(output.ID, output.Amount)).ToArray();

                    CreateRecipe(CraftingTableConfig.ID, inputs, outputs, recipe.Description, 40f, i);
                }
            }

            public static void CreateRecipe(string fabricatorID, ComplexRecipe.RecipeElement[] input, ComplexRecipe.RecipeElement[] output, string description, float time, int sortOrderOffset)
            {
                foreach (var item in input)
                {
                    if (Assets.TryGetPrefab(item.material) == null)
                    {
                        Log.Debuglog("tried adding recipe but the ingredient is not available: " + item.material);
                        return;
                    }
                }

                var recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, input, output);

                var desc = Strings.TryGet(description, out var result) ? result.String : description;

                new ComplexRecipe(recipeID, input, output)
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
            }
        }
    }
}
