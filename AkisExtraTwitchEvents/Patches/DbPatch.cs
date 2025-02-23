using HarmonyLib;
using Twitchery.Content;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Foods;
using Twitchery.Content.Events;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix(Db __instance)
			{
				TEmotes.Register(__instance.Emotes.Minion);
				TStatusItems.Register(__instance.MiscStatusItems);
				TDb.Init(__instance);

				TwitchEvents.OnDbInit();

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

				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_GOOPPARFAIT.DESC, 40f)
					.Input(Elements.PinkSlime.Tag, 5f)
					.Input(RawEggConfig.ID, 2f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Output(GoopParfaitConfig.ID, 1f)
					.Build();

				AkisTwitchEvents.frozenHoneyRecipeID = RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ELEMENTS.AETE_FROZENHONEY.DESC, 40f)
					.Input(Elements.Honey.Tag, 100f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Output(Elements.FrozenHoney.Tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
					.Build().id;

				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ELEMENTS.AETE_RASPBERRYJAM.DESC, 40f)
					.Input(PrickleFruitConfig.ID, 5f)
					.Input(SimHashes.Sucrose.CreateTag(), 50f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Output(Elements.RaspberryJam.Tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
					.Build();

			}
		}
	}
}
