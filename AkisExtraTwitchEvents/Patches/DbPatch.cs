using Database;
using HarmonyLib;
using Twitchery.Content;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Foods;
using Twitchery.Content.Defs.Meds;
using Twitchery.Content.Events;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class DbPatch
	{

		[HarmonyPatch(typeof(Deaths), MethodType.Constructor, [typeof(ResourceSet)])]
		public class TargetType_TargetMethod_Patch
		{
			public static void Postfix(Deaths __instance)
			{
				TDeaths.Register(__instance);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix(Db __instance)
			{
				TEmotes.Register(__instance.Emotes.Minion);
				TStatusItems.Register(__instance.MiscStatusItems);

				TDb.Init(__instance);

				TwitchEvents.OnDbInit();

				RecipeBuilder.Create(ApothecaryConfig.ID, STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_LEMONADE.DESC, 40f)
					.Input(LemonConfig.ID, 1f)
					.Input(SimHashes.Water.CreateTag(), 2f)
					.Output(LemonadeConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.SortOrder(0)
					.Build();

				if (Mod.isSgtChaosHere)
				{
					RecipeBuilder.Create(ApothecaryConfig.ID, STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_MELONADE.DESC, 40f)
						.Input(LemonConfig.ID, 1f)
						.Input("ITCE_Inverse_Water", 2f)
						.Output(MelonadeConfig.ID, 1f)
						.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
						.SortOrder(1)
						.Build();
				}

				if (DlcManager.FeatureRadiationEnabled())
				{
					AkisTwitchEvents.radDishRecipeID = RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_COOKEDRADISH.DESC, 40f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Input(RawRadishConfig.ID, 1f)
					.Input(SimHashes.UraniumOre.CreateTag(), 10f)
					.Output(CookedRadishConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Build().id;
				}

				AkisTwitchEvents.pizzaRecipeID = RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_PIZZA.DESC, 40f)
					.Input(ColdWheatBreadConfig.ID, 1f)
					.Input(SpiceNutConfig.ID, 2f)
					.Input(PrickleFruitConfig.ID, 2f)
					.Input(MeatConfig.ID, 1f)
					.Output(PizzaConfig.ID, 1f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Build().id;

				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_MACANDCHEESE.DESC, 40f)
					.Input(Elements.Macaroni.Tag, 50f)
					.Input(SimHashes.Milk.CreateTag(), 20f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Output(MacAndCheeseConfig.ID, 1f)
					.Build();

				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_GOOPPARFAIT.DESC, 40f)
					.Input(Elements.PinkSlime.Tag, 10f)
					.Input(RawEggConfig.ID, 0.5f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Output(GoopParfaitConfig.ID, 1f)
					.Build();

				RecipeBuilder.Create(ApothecaryConfig.ID, STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_LEMONADE.DESC, 40f)
					.Input(Elements.Honey.Tag, 5f)
					.Input(SimHashes.Water.CreateTag(), 2f)
					.Output(WereVoleCureConfig.ID, 5f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.SortOrder(0)
					.Build();
				/*
								AkisTwitchEvents.frozenHoneyRecipeID = RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ELEMENTS.AETE_FROZENHONEY.DESC, 40f)
									.Input(Elements.Honey.Tag, 100f)
									.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
									.Output(Elements.FrozenHoney.Tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
									.Build().id;
				*/
				/*				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ELEMENTS.AETE_RASPBERRYJAM.DESC, 40f)
									.Input(PrickleFruitConfig.ID, 5f)
									.Input(SimHashes.Sucrose.CreateTag(), 50f)
									.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
									.Output(Elements.RaspberryJam.Tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
									.Build();*/

			}
		}
	}
}
