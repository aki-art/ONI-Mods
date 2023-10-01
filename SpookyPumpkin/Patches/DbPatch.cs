using FUtility;
using HarmonyLib;
using SpookyPumpkinSO.Content;
using SpookyPumpkinSO.Content.Equipment;
using SpookyPumpkinSO.Content.Foods;
using SpookyPumpkinSO.Content.Plants;
using TUNING;

namespace SpookyPumpkinSO.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				ModAssets.LateLoadAssets();
				BuildingFacadesPatch.Patch(Mod.harmonyInstance);
			}

			public static void Postfix()
			{
				AddFoodRecipes();
				AddCostumeRecipes();
				ModDb.OnDbInit();
			}

			private static void AddCostumeRecipes()
			{
				AddCostumeRecipe(SPEquippableFacades.SKELLINGTON);
				AddCostumeRecipe(SPEquippableFacades.SCARECROW);
				AddCostumeRecipe(SPEquippableFacades.VAMPIRE);
			}

			private static void AddFoodRecipes()
			{
				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DESC, FOOD.RECIPES.STANDARD_COOK_TIME)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Input(ColdWheatConfig.SEED_ID, 3f)
					.Input(RawEggConfig.ID, 0.3f)
					.Input(PumpkinConfig.ID, 2f)
					.Output(PumpkinPieConfig.ID, 1f)
					.Build();

				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.SP_TOASTEDPUMPKINSEED.DESC, FOOD.RECIPES.SMALL_COOK_TIME)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Input(PumpkinPlantConfig.SEED_ID, 2f)
					.Input(TableSaltConfig.ID, 0.001f)
					.Output(ToastedPumpkinSeedConfig.ID, 1f)
					.Build();
			}

			private static void AddCostumeRecipe(string facadeID)
			{
				RecipeBuilder
					.Create(ClothingAlterationStationConfig.ID, global::STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC, TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_FABTIME)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)

					.Input(FunkyVestConfig.ID, 1f, false)
					.Input(BasicFabricConfig.ID, 3f)

					.FacadeOutput(HalloweenCostumeConfig.ID, 1f, facadeID)

					.Build(facadeID);
			}
		}
	}
}
