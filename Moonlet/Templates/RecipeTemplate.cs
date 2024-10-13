using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using static ComplexRecipe.RecipeElement;

namespace Moonlet.Templates
{
	public class RecipeTemplate : BaseTemplate
	{
		public string Fabricator { get; set; }

		public string Description { get; set; }

		public string OverrideName { get; set; }

		public FloatNumber Time { get; set; }

		public string RequiredTech { get; set; }

		public IntNumber SortOrder { get; set; }

		public IntNumber RadBoltCost { get; set; }

		public IntNumber RadBoltProduced { get; set; }

		[Doc("", typeof(ComplexRecipe.RecipeNameDisplay))]
		public string NameDisplay { get; set; }

		public List<Input> Ingredients { get; set; }

		public List<Output> Results { get; set; }

		[Serializable]
		public class Input : IShadowTypeBase<ComplexRecipe.RecipeElement>
		{
			public string Tag { get; set; }

			public FloatNumber Amount { get; set; }

			public ComplexRecipe.RecipeElement Convert(Action<string> log = null)
			{
				return new ComplexRecipe.RecipeElement(Tag, Amount);
			}
		}

		public class Output : IShadowTypeBase<ComplexRecipe.RecipeElement>
		{
			public string Tag { get; set; }

			public FloatNumber Amount { get; set; }

			[Doc("", typeof(TemperatureOperation))]
			public string TemperatureOperaton { get; set; }

			public string FacadeId { get; set; }

			public bool? InheritElement { get; set; }

			public bool? StoreResult { get; set; }

			public ComplexRecipe.RecipeElement Convert(Action<string> log = null)
			{
				var result = new ComplexRecipe.RecipeElement(Tag, Amount)
				{
					storeElement = StoreResult.GetValueOrDefault(),
					inheritElement = InheritElement.GetValueOrDefault(),
					facadeID = FacadeId,
					temperatureOperation = EnumUtils.ParseOrDefault(TemperatureOperaton, TemperatureOperation.AverageTemperature)
				};

				return result;
			}
		}
	}
}
