using FUtility;
using HarmonyLib;
using SpookyPumpkinSO.Content;
using SpookyPumpkinSO.Content.Equipment;
using SpookyPumpkinSO.Content.Foods;
using SpookyPumpkinSO.Content.Plants;
using System.Collections.Generic;
using TUNING;
using static ComplexRecipe;

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
				Integration.TwitchMod.TwitchEvents.OnDbInit();
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

			[HarmonyPatch(typeof(FoodDehydratorConfig), "ConfigureRecipes")]
			public static class FoodDehydrator_ConfigureRecipes
			{
				public static void Postfix()
				{
					var foodInfo = SPFoodInfos.pumpkinPie;
					var material = DehydratedPumpkinPieConfig.ID;

					RecipeElement[] input = new RecipeElement[2]
					{
						new RecipeElement(foodInfo, 6000000f / foodInfo.CaloriesPerUnit),
						new RecipeElement(SimHashes.Polypropylene.CreateTag(), 12f)
					};
					RecipeElement[] output = new RecipeElement[2]
					{
						new RecipeElement(material, 6f, RecipeElement.TemperatureOperation.Dehydrated),
						new RecipeElement(SimHashes.Water.CreateTag(), 6f, RecipeElement.TemperatureOperation.Heated)
					};

					new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(FoodRehydratorConfig.ID, input, output), input, output)
					{
						time = 250f,
						nameDisplay = RecipeNameDisplay.Custom,
						customName = string.Format((string)global::STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RECIPE_NAME, foodInfo.Name),
						description = string.Format((string)global::STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RESULT_DESCRIPTION, foodInfo.Name),
						fabricators = new List<Tag>()
						{
							FoodRehydratorConfig.ID
						},
						sortOrder = 28
					};
				}
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
