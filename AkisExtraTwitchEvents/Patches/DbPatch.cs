using Database;
using HarmonyLib;
using ONITwitchLib;
using ONITwitchLib.Core;
using System.Collections.Generic;
using Twitchery.Content;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Foods;
using Twitchery.Content.Defs.Meds;
using Twitchery.Content.Events;
using Twitchery.Content.Scripts;
using UnityEngine;

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
			[HarmonyPriority(Priority.Last)]
			[HarmonyPostfix]
			public static void LatePostfix()
			{
				if (!TwitchModInfo.TwitchIsPresent)
					return;

				if (Mod.Settings.EventRarityEqualizer > 0.0f)
				{
					foreach (var group in TwitchDeckManager.Instance.GetGroups())
					{
						var weights = group.GetWeights();
						if (weights != null)
						{
							foreach (var weight in weights)
							{
								var currentWeight = weight.Value;

								// don't touch special or disabled events
								if (currentWeight <= 0 || currentWeight >= 99)
									continue;

								var adjustedWeight = Mathf.CeilToInt(Mathf.Lerp(currentWeight, Consts.EventWeight.Common, Mod.Settings.EventRarityEqualizer));

								Log.Debug($"change weight of {weight.Key.FriendlyName} from {currentWeight} to {adjustedWeight}");
								group.SetWeight(weight.Key, adjustedWeight);
							}
						}
					}
				}

				HashSet<string> disableEvents = [
					"asquared31415.TwitchIntegration.RainPrefabPacu",
					"asquared31415.TwitchIntegration.RainPrefabSlickster",
					];

				// replacing Slickster Rain with TEMPORARY_SlicksterRain & Pacu
				var rainPrefabsGroup = TwitchDeckManager.Instance.GetGroup("core.rain_prefab");
				if (rainPrefabsGroup != null)
				{
					var weights = rainPrefabsGroup.GetWeights();
					if (weights != null)
					{
						foreach (var weight in weights)
						{
							if (disableEvents.Contains(weight.Key.Id))
							{
								rainPrefabsGroup.SetWeight(weight.Key, 0);
								weight.Key.AddCondition(_ => false);
							}
						}
					}
				}

				if (!Mathf.Approximately(Mod.Settings.EventsRarityModifier, 1.0f))
				{
					foreach (var group in TwitchDeckManager.Instance.GetGroups())
					{
						var weights = group.GetWeights();
						if (weights != null)
						{
							foreach (var weight in weights)
							{
								if (weight.Key.EventNamespace == "Twitchery")
									group.SetWeight(weight.Key, Mathf.RoundToInt(weight.Value * Mod.Settings.EventsRarityModifier));
							}
						}
					}
				}
			}

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
#if WEREVOLE
				RecipeBuilder.Create(ApothecaryConfig.ID, STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_LEMONADE.DESC, 40f)
					.Input(Elements.Honey.Tag, 5f)
					.Input(SimHashes.Algae.CreateTag(), 5f)
					.Input(SimHashes.Water.CreateTag(), 2f)
					.Output(WereVoleCureConfig.ID, 5f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result)
					.SortOrder(0)
					.Build();
#endif
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
