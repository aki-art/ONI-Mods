using FUtility;
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
				TDb.Init();

				TwitchEvents.OnDbInit();

				if (DlcManager.FeatureRadiationEnabled())
				{
					AkisTwitchEvents.radDishRecipeID = RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_COOKEDRADISH.DESC, 40f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.Input(RawRadishConfig.ID, 1f)
					.Input(SimHashes.UraniumOre.CreateTag(), 10f)
					.Output(CookedRadishConfig.ID, 1f)
					.Build().id;
				}

				AkisTwitchEvents.pizzaRecipeID = RecipeBuilder.Create(GourmetCookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_PIZZA.DESC, 40f)
					.Input(ColdWheatBreadConfig.ID, 1f)
					.Input(SpiceNutConfig.ID, 2f)
					.Input(PrickleFruitConfig.ID, 2f)
					.Input(MeatConfig.ID, 1f)
					.Output(PizzaConfig.ID, 1f)
					.Build().id;

				RecipeBuilder.Create(CookingStationConfig.ID, STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_GOOP_PARFAIT.DESC, 40f)
					.Input(Elements.PinkSlime.Tag, 5f)
					.Input(RawEggConfig.ID, 2f)
					.Output(GoopParfaitConfig.ID, 1f)
					.Build();
			}
		}
	}
}
