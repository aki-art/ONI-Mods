using System.Collections.Generic;

namespace Moonlet
{
	public class ComplexRecipeExtension
	{
		private static Dictionary<string, ComplexRecipeExtension> complexRecipesExtra = new();
		public HashedString icon;
		public string name;
		public string description;

		public static ComplexRecipeExtension Get(string recipeId)
		{
			return complexRecipesExtra.TryGetValue(recipeId, out var result) ? result : null;
		}

		public static bool TryGet(string recipeId, out ComplexRecipeExtension extension)
			=> complexRecipesExtra.TryGetValue(recipeId, out extension);

		public static void Register(string recipeId, string icon, string name, string description)
		{
			complexRecipesExtra[recipeId] = new ComplexRecipeExtension()
			{
				icon = icon,
				name = name, 
				description = description
			};
		}
	}
}
