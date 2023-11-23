using System.Collections.Generic;
using static ComplexRecipe;
using static ComplexRecipe.RecipeElement;

namespace FUtility
{
	public class RecipeBuilder
	{
		private string fabricator;
		private float time;
		private RecipeNameDisplay nameDisplay;
		private string description;

		private List<RecipeElement> inputs;
		private List<RecipeElement> outputs;

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

		public RecipeBuilder Input(Tag tag, float amount, bool inheritElement = true)
		{
			inputs.Add(new RecipeElement(tag, amount, inheritElement));
			return this;
		}

		public RecipeBuilder Output(Tag tag, float amount, TemperatureOperation tempOp = TemperatureOperation.AverageTemperature)
		{
			outputs.Add(new RecipeElement(tag, amount, tempOp));
			return this;
		}

		public RecipeBuilder FacadeOutput(Tag tag, float amount, string facadeID = "", bool storeElement = false, TemperatureOperation tempOp = TemperatureOperation.AverageTemperature)
		{
			outputs.Add(new RecipeElement(tag, amount, tempOp, facadeID, storeElement));
			return this;
		}

		public ComplexRecipe Build(string facadeID = "")
		{
			var i = inputs.ToArray();
			var o = outputs.ToArray();

			string recipeID = facadeID.IsNullOrWhiteSpace() ? ComplexRecipeManager.MakeRecipeID(fabricator, i, o) : ComplexRecipeManager.MakeRecipeID(fabricator, i, o, facadeID);

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