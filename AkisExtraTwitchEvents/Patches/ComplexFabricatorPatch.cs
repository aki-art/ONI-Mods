using HarmonyLib;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class ComplexFabricatorPatch
	{
		public static float temperature = GameUtil.GetTemperatureConvertedToKelvin(-40, GameUtil.TemperatureUnit.Celsius);

		[HarmonyPatch(typeof(ComplexFabricator), "SpawnOrderProduct")]
		public class ComplexFabricator_SpawnOrderProduct_Patch
		{
			public static void Postfix(ComplexRecipe recipe, List<GameObject> __result)
			{
				if (recipe == null)
					return;

				if (recipe.id == AkisTwitchEvents.frozenHoneyRecipeID)
				{
					foreach (GameObject item in __result)
					{
						if (item.TryGetComponent(out PrimaryElement primaryElement))
							primaryElement.SetTemperature(temperature);
					}
				}
			}
		}
	}
}
