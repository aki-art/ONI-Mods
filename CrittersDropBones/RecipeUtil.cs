using System.Collections.Generic;

namespace CrittersDropBones
{
    public class RecipeUtil
    {
        public static void ConfigureRecipes(string ID)
        {
            foreach (var recipe in Mod.Recipes.Recipes)
            {
                var inputs = GetTags(recipe.Inputs);
                var outputs = GetTags(recipe.Outputs);

                if (DlcManager.IsDlcListValidForCurrentContent(recipe.Dlc))
                {
                    if (inputs != null && outputs != null)
                    {
                        CreateRecipe(ID, inputs.ToArray(), outputs.ToArray(), recipe.Description);
                    }
                }
            }
        }

        private static List<ComplexRecipe.RecipeElement> GetTags(FRecipeElement[] elements)
        {
            var tags = new List<ComplexRecipe.RecipeElement>();

            foreach (var element in elements)
            {
                if (Assets.TryGetPrefab(element.ID) != null && element.Amount > 0)
                {
                    tags.Add(new ComplexRecipe.RecipeElement(element.ID, element.Amount));
                }
                else
                {
                    return null;
                }
            }

            return tags;
        }

        public static ComplexRecipe CreateRecipe(string fabricatorID, ComplexRecipe.RecipeElement[] input, ComplexRecipe.RecipeElement[] output, string description, float time = 40f)
        {
            var recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, input, output);

            var desc = description;

            if (Strings.TryGet(description, out var str))
            {
                desc = str;
            }

            var recipe = new ComplexRecipe(recipeID, input, output)
            {
                time = time,
                description = desc,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag> { TagManager.Create(fabricatorID) }
            };

            return recipe;
        }

        public class FRecipe
        {
            public string Description { get; set; }

            public string[] Dlc { get; set; } = DlcManager.AVAILABLE_ALL_VERSIONS;

            public float Time { get; set; } = 40f;

            public int SortOrder { get; set; } = 1;

            public FRecipeElement[] Inputs { get; set; }

            public FRecipeElement[] Outputs { get; set; }
        }

        public class FRecipeElement
        {
            public FRecipeElement(Tag iD, float amount)
            {
                ID = iD.ToString();
                Amount = amount;
            }

            public string ID { get; set; }

            public float Amount { get; set; }
        }


    }
}
