using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Slag.Tweaks
{
    class MetalRefinery
    {
        // Changes names of recipees
        [HarmonyPatch(typeof(ComplexRecipe), "GetUIName")]
        public static class ComplexRecipe_GetUIName_Patch
        {
            public static void Postfix(ref string __result, string ___id)
            {
                if(___id.StartsWith(MetalRefineryConfig.ID) && ___id.Contains("Slag"))
                {
                    __result = "Slag enhanced " + __result;
                }
            }
        }

        private static Sprite SlagUIIcon = Def.GetUISprite(ElementLoader.FindElementByName("Slag").tag).first;

        [HarmonyPatch(typeof(ComplexFabricatorSideScreen), "Initialize")]
        public static class ComplexFabricatorSideScreen_Initialize_Patch
        {
            public static void Postfix(ComplexFabricator target, GameObject ___recipeGrid)
            {
                if(target.name == MetalRefineryConfig.ID + "Complete")
                {
                    foreach (Transform child in ___recipeGrid.transform)
                    {
                        var title = child.gameObject.GetComponentInChildren<LocText>();
                        if(title != null)
                        {
                            if(title.text.Contains("Slag enhanced"))
                            {
                                var img = child.gameObject.GetComponentsInChildrenOnly<Image>();
                                if (img != null && img.Length >= 3)
                                {
                                    Image slagIcon = UnityEngine.Object.Instantiate(img[2], img[2].transform);
                                    slagIcon.rectTransform.localScale = Vector3.one * .8f;
                                    slagIcon.sprite = SlagUIIcon;
                                }
                            }
                        }
                    }
                }
            }
        }
        

        // Adds recipes using Slag to Metal Refinery
        [HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate")]
        public static class MetalRefineryConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix()
            {
                var regolith = ElementLoader.FindElementByHash(SimHashes.Regolith);
                var slag = ElementLoader.FindElementByName("Slag");

                List<Element> rawMetal = ElementLoader.elements.FindAll((Element e) => e.IsSolid && e.HasTag(GameTags.Metal));
                foreach (Element element in rawMetal)
                {
                    Element pure_molten = element.highTempTransition;
                    Element pure_solid_element = pure_molten.lowTempTransition;
                    if (pure_solid_element != element)
                    {
                        ComplexRecipe.RecipeElement[] input = new ComplexRecipe.RecipeElement[]
                        {
                            new ComplexRecipe.RecipeElement(element.tag, 100f),
                            new ComplexRecipe.RecipeElement(regolith.tag, 25f),
                            new ComplexRecipe.RecipeElement(slag.tag, 10f),
                        };

                        ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[]
                        {
                            new ComplexRecipe.RecipeElement(pure_solid_element.tag, 110f),
                            new ComplexRecipe.RecipeElement(slag.tag, 25f),
                        };

                        string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID(MetalRefineryConfig.ID, element.tag + "_Slag");
                        string id = ComplexRecipeManager.MakeRecipeID(MetalRefineryConfig.ID, input, output);

                        ComplexRecipe complexRecipe = new ComplexRecipe(id, input, output)
                        {
                            time = 40f,
                            description = string.Format(SlagStrings.STRINGS.BUILINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, pure_solid_element.name, element.name, slag.name, regolith.name),
                            nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                            fabricators = new List<Tag> { TagManager.Create(MetalRefineryConfig.ID) }
                        };

                        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, id);
                    }
                }
            }
        }
    }
}
