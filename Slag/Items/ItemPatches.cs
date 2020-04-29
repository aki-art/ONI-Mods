using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slag.Items
{
    class ItemPatches
    {
        [HarmonyPatch(typeof(RockCrusherConfig), "ConfigureBuildingTemplate")]
        public static class RockCrusherConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix()
            {
                var sandstone = ElementLoader.FindElementByHash(SimHashes.SandStone);

                ComplexRecipe.RecipeElement[] input = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(sandstone.tag, 10f),
                };

                ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(MysteryOreConfig.ID, 110f),
                };

                string id = ComplexRecipeManager.MakeRecipeID(MetalRefineryConfig.ID, input, output);

                new ComplexRecipe(id, input, output)
                {
                    time = 40f,
                    description = string.Format("test"),
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag> { TagManager.Create(RockCrusherConfig.ID) }
                };
            }
        }
    }
}
