using HarmonyLib;
using Moonlet.Loaders;
using System.Collections.Generic;
using System.Linq;

namespace Moonlet.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				PlanScreen.iconNameMap.Add(HashCache.Get().Add(ModDb.BuildingCategories.POIS), "icon_errand_build");
				ModDb.OnDbInitialize();
			}

			[HarmonyPostfix]
			[HarmonyPriority(Priority.VeryLow)]
			public static void LatePostfix()
			{
				/*                FUtility.RecipeBuilder.Create(RockCrusherConfig.ID, "test", 10f)
									.Input("StarterMetal", 10f)
									.Output(CookedFishConfig.ID, 1f)
									.Build();*/

				var test = FUtility.RecipeBuilder.Create(RockCrusherConfig.ID, "test", 10f)
					.Input("Solids_NickelOre", 10f)
					.Output(CookedFishConfig.ID, 1f)
					.Build();

				ComplexRecipeExtension.Register(test.id, "test", "Nickel", "TODO");

				var beverages = new List<Tuple<Tag, string>>();

				foreach (var beverage in SharedElementsLoader.beverages)
					beverages.Add(new Tuple<Tag, string>(beverage.Key, beverage.Value[0]));

				WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS = WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS
					.AddRangeToArray(beverages.ToArray())
					.ToArray();
			}
		}
	}
}
