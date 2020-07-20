using Harmony;
using System.Collections.Generic;

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
                     new ComplexRecipe.RecipeElement(ElementLoader.FindElementByName("Slag").tag, 10f),
                };

                ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(MysteryOreConfig.ID, 10f),
                };

                string id = ComplexRecipeManager.MakeRecipeID(MetalRefineryConfig.ID, input, output);

                new ComplexRecipe(id, input, output)
                {
                    time = 40f,
                    description = string.Format("test"),
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag> { TagManager.Create(RockCrusherConfig.ID) }
                };

                ComplexRecipe.RecipeElement[] input2 = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(ElementLoader.FindElementByName("Slag").tag, 10f),
                };

                ComplexRecipe.RecipeElement[] output2 = new ComplexRecipe.RecipeElement[]
                {
                     new ComplexRecipe.RecipeElement(MysteryMetalConfig.ID, 10f),
                };

                string id2 = ComplexRecipeManager.MakeRecipeID(MetalRefineryConfig.ID, input2, output2);

                new ComplexRecipe(id2, input2, output2)
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
