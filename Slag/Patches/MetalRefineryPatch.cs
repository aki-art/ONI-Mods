using HarmonyLib;
using Slag.Content;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Patches
{
    public class MetalRefineryPatch
    {
        // Adds recipes using Slag to Metal Refinery
        [HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate")]
        public static class MetalRefineryConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                var regolith = ElementLoader.FindElementByHash(SimHashes.Regolith);
                var slag = ElementLoader.FindElementByName("Slag");

                AddNewRecipes(regolith, slag);
                // AddSlagToOutputs(go, slag);
            }

            private static void AddSlagToOutputs(GameObject go, Element slag)
            {
                var complexFabricator = go.GetComponent<ComplexFabricator>();
                foreach (var recipe in complexFabricator.GetRecipes())
                {
                    var element = ElementLoader.GetElement(recipe.ingredients[0].material);
                    if (IsMetalOre(element))
                    {
                        recipe.results = recipe.results.AddToArray(new ComplexRecipe.RecipeElement(slag.tag, 5f));
                    }
                }
            }

            private static void AddNewRecipes(Element regolith, Element slag)
            {
                var rawMetals = ElementLoader.elements.FindAll(e => IsMetalOre(e));

                foreach (var metal in rawMetals)
                {
                    var molten = metal.highTempTransition;
                    var solid = molten.lowTempTransition;

                    if (solid != metal)
                    {
                        var input = new ComplexRecipe.RecipeElement[]
                        {
                            new ComplexRecipe.RecipeElement(metal.tag, 100f),
                            new ComplexRecipe.RecipeElement(regolith.tag, 25f),
                            new ComplexRecipe.RecipeElement(slag.tag, 10f),
                        };

                        var output = new ComplexRecipe.RecipeElement[]
                        {
                            new ComplexRecipe.RecipeElement(solid.tag, 110f),
                            new ComplexRecipe.RecipeElement(slag.tag, 25f),
                        };

                        var obsoleteID = ComplexRecipeManager.MakeObsoleteRecipeID(MetalRefineryConfig.ID, metal.tag + "_Slag");
                        var ID = ComplexRecipeManager.MakeRecipeID(MetalRefineryConfig.ID, input, output);

                        var complexRecipe = new ComplexRecipe(ID, input, output)
                        {
                            time = 40f,
                            description = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, solid.name, metal.name, slag.name, regolith.name),
                            nameDisplay = ModAssets.slagNameDisplay,
                            fabricators = new List<Tag> { MetalRefineryConfig.ID }
                        };

                        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsoleteID, ID);
                    }
                }
            }

            private static bool IsMetalOre(Element e)
            {
                return e.IsSolid &&
                    e.id != Elements.Slag &&
                    e.HasTag(GameTags.Metal) &&
                    (e.HasTag(GameTags.Ore) || e.id == SimHashes.Wolframite); // wolframite is not marked as an ore for some reason
            }
        }
    }
}
