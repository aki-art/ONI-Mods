using Database;
using Klei.AI;
using Moonlet.Templates;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.TemplateLoaders
{
	public class SpiceLoader(SpiceTemplate template, string sourceMod) : TemplateLoaderBase<SpiceTemplate>(template, sourceMod)
	{
		public void LoadContent(Spices spices)
		{
			var ingredients = new List<Spice.Ingredient>();
			if (template.Ingredients != null)
			{
				foreach (var ingredient in template.Ingredients)
				{
					ingredients.Add(new Spice.Ingredient()
					{
						IngredientSet = [ingredient.Tag],
						AmountKG = ingredient.Amount
					});
				}
			}

			var effect = template.Effects?[0].Create("Spices");
			var foodEffect = template.FoodEffect?[0].Create("Spices");
			var calorieModifier = template.KCalModifier != 0 ? new AttributeModifier(Db.Get().Amounts.Calories.Id, template.KCalModifier, "Spices") : null;

			new Spice(spices,
				template.Id,
				[.. ingredients],
				template.Color,
				Color.white,
				foodEffect,
				effect,
				template.Icon,
				template.DlcIds).CalorieModifier = calorieModifier;
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESC"), template.Description);
		}

		public override string GetTranslationKey(string partialKey) => $"STRINGS.ITEMS.SPICES.{template.Id.ToUpperInvariant()}.{partialKey}";
	}
}
