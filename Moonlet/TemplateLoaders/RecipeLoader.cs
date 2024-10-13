using HarmonyLib;
using Moonlet.Templates;
using Moonlet.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ComplexRecipe;

namespace Moonlet.TemplateLoaders
{
	public class RecipeLoader(RecipeTemplate template, string sourceMod) : TemplateLoaderBase<RecipeTemplate>(template, sourceMod)
	{
		public ComplexRecipe recipe;

		public override void Initialize()
		{
			id = MakeNameSpacedRecipeID();
			template.Id = id;

			base.Initialize();
		}

		public override string GetTranslationKey(string partialKey) => $"STRINGS.UI.RECIPE_NAMES.{id.ToUpperInvariant()}.{partialKey}";

		public override void RegisterTranslations()
		{
			if (template.OverrideName != null)
				AddString(GetTranslationKey("NAME"), template.OverrideName);

			if (template.Description != null)
				AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}

		public override void Validate()
		{
			base.Validate();
			if (template.Results == null || template.Results.Count == 0)
			{
				Issue("Recipe requires at least one result!");
				isValid = false;
			}

			if (template.Ingredients == null || template.Ingredients.Count == 0)
			{
				Issue("Recipe requires at least one ingredient!");
				isValid = false;
			}
		}

		private bool AllIngredientsExist()
		{
			var missingItems = new HashSet<string>();

			foreach (var item in template.Ingredients)
			{
				if (!Assets.TryGetPrefab(item.Tag))
					missingItems.Add(item.Tag);
			}

			foreach (var item in template.Results)
			{
				if (!Assets.TryGetPrefab(item.Tag))
					missingItems.Add(item.Tag);
			}

			if (missingItems.Count > 0)
			{
				if (!template.Optional)
					Issue($"The following items do not exist: " + missingItems.Join());

				return false;
			}

			return true;
		}

		public void LoadContent()
		{
			if (Assets.TryGetPrefab(template.Fabricator) == null)
			{
				if (!template.Optional)
					Issue($"Fabricator {template.Fabricator} does not exist.");

				return;
			}

			if (!AllIngredientsExist())
				return;

			if (ComplexRecipeManager.Get().GetRecipe(id) != null)
			{
				var recipeCount = ComplexRecipeManager.Get().recipes.FindAll(recipe => recipe.id.StartsWith(id));

				id += $"_{recipeCount.Count + 1}";
				Info("Duplicate recipe found. Redirecting to " + id);
			}

			var firstOutput = template.Results[0];
			var description = template.Description == null
				? string.Empty
				: Strings.Get(GetTranslationKey("DESCRIPTION"));

			var overrideName = template.OverrideName == null
				? null
				: Strings.Get(GetTranslationKey("NAME")).String;

			var inputs = ShadowTypeUtil
				.CopyList<RecipeElement, RecipeTemplate.Input>(template.Ingredients, Warn)
				.ToArray();

			var outputs = ShadowTypeUtil
				.CopyList<RecipeElement, RecipeTemplate.Output>(template.Results, Warn)
				.ToArray();

			recipe = new ComplexRecipe(
				id,
				inputs,
				outputs,
				template.RadBoltCost.CalculateOrDefault(0),
				template.RadBoltProduced.CalculateOrDefault(0),
				template.GetDlcIds())
			{
				time = template.Time.CalculateOrDefault(40),
				fabricators = [template.Fabricator],
				requiredTech = template.RequiredTech,
				customName = overrideName,
				description = description,
				sortOrder = template.SortOrder.CalculateOrDefault(0),
				nameDisplay = EnumUtils.ParseOrDefault(template.NameDisplay, RecipeNameDisplay.Result)
			};
		}

		public string MakeNameSpacedRecipeID()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append(template.Fabricator ?? "");
			//stringBuilder.Append($"_{sourceMod}"); // cannot be the beginning, because the codex relies on string operation to get the fabricator ID out of a recipe, isntead of grabbinf the fabricator field for some reason.
			stringBuilder.Append("_I");

			foreach (var input in template.Ingredients)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(input.Tag ?? "");
			}

			stringBuilder.Append("_O");

			foreach (var output in template.Results)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(output.Tag ?? "");
			}

			return stringBuilder.ToString();
		}
	}
}
