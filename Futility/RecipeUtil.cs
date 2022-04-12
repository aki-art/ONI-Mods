using System.Collections.Generic;
using static ComplexRecipe;

namespace FUtility
{
    public class RecipeBuilder
    {
        private string fabricator;
        private float time;
        private RecipeNameDisplay nameDisplay;
        string description;

        List<RecipeElement> inputs;
        List<RecipeElement> outputs;

        public static RecipeBuilder Create(string fabricatorID, string description, float time)
        {
            var builder = new RecipeBuilder
            {
                fabricator = fabricatorID,
                description = description,
                time = time,
                nameDisplay = RecipeNameDisplay.IngredientToResult,
                inputs = new List<RecipeElement>(),
                outputs = new List<RecipeElement>()
            };

            return builder;
        }

        public RecipeBuilder NameDisplay(RecipeNameDisplay nameDisplay)
        {
            this.nameDisplay = nameDisplay;
            return this;
        }

        public RecipeBuilder Input(Tag tag, float amount)
        {
            inputs.Add(new RecipeElement(tag, amount));
            return this;
        }

        public RecipeBuilder Output(Tag tag, float amount)
        {
            outputs.Add(new RecipeElement(tag, amount));
            return this;
        }

        public ComplexRecipe Build()
        {
            var i = inputs.ToArray();
            var o = outputs.ToArray();

            string recipeID = ComplexRecipeManager.MakeRecipeID(fabricator, i, o);

            return new ComplexRecipe(recipeID, i, o)
            {
                time = time,
                description = description,
                nameDisplay = nameDisplay,
                fabricators = new List<Tag> { fabricator }
            };
        }
    }
}
