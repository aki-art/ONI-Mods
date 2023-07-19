using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Patches
{
	public class ComplexRecipePatch
	{
		[HarmonyPatch(typeof(SelectedRecipeQueueScreen), "GetIngredientDescriptions")]
		public class SelectedRecipeQueueScreen_GetIngredientDescriptions_Patch
		{
			public static bool Prefix(ComplexRecipe recipe, ref List<SelectedRecipeQueueScreen.DescriptorWithSprite> __result)
			{
				if (ComplexRecipeExtension.TryGet(recipe.id, out var extra))
				{
					var icon = Assets.GetSprite(extra.icon);

					var ingredientDescriptions = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>()
					{
						new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor("test name", "test desc"), new Tuple<Sprite, Color>(icon, Color.white))
					};

					__result = ingredientDescriptions;

					return false;
				}

				return true;
			}
		}

		[HarmonyPatch(typeof(ComplexRecipe), "GetUIIcon")]
		public class ComplexRecipe_GetUIIcon_Patch
		{
			public static bool Prefix(ComplexRecipe __instance, ref Sprite __result)
			{
				if (ComplexRecipeExtension.TryGet(__instance.id, out var extra))
				{
					__result = Assets.GetSprite(extra.icon);
					return false;
				}

				return true;
			}
		}
	}
}
