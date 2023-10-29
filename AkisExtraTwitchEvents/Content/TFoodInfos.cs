using System.Collections.Generic;
using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Foods;

namespace Twitchery.Content
{
	public class TFoodInfos
	{
		public const float JELLO_KCAL_PER_KG = 400f; // has to be very low, because this is spammed
		public const float RADISH_KCAL_PER_KG = 800f;
		public const float PIZZA_KCAL_PER_KG = 3200f;

		public static EdiblesManager.FoodInfo jello = new(
				Elements.Jello.ToString(),
				DlcManager.VANILLA_ID,
				JELLO_KCAL_PER_KG * 1000f,
				3,
				255.15f,
				277.15f,
				2400f,
				false);

		public static EdiblesManager.FoodInfo honeyPopsicle = new EdiblesManager.FoodInfo(
				Elements.FrozenHoney.ToString(),
				DlcManager.VANILLA_ID,
				80 * 1000f,
				4,
				255.15f,
				277.15f,
				2400f,
				false)
			.AddEffects(new List<string>()
			{
				TEffects.HONEY
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

		public static EdiblesManager.FoodInfo rawRadish = new EdiblesManager.FoodInfo(
				RawRadishConfig.ID,
				DlcManager.VANILLA_ID,
				RADISH_KCAL_PER_KG * 1000f,
				TUNING.FOOD.FOOD_QUALITY_MEDIOCRE,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true)
			.AddEffects(new List<string>()
			{
				TEffects.RADISH_STRENGTH
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

		public static EdiblesManager.FoodInfo cookedRadish = new EdiblesManager.FoodInfo(
				CookedRadishConfig.ID,
				DlcManager.VANILLA_ID,
				1800_000f,
				TUNING.FOOD.FOOD_QUALITY_AMAZING,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true)
			.AddEffects(new List<string>()
			{
				TEffects.RADISH_STRENGTH
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

		public static EdiblesManager.FoodInfo pizza = new EdiblesManager.FoodInfo(
				PizzaConfig.ID,
				DlcManager.VANILLA_ID,
				PIZZA_KCAL_PER_KG * 1000f,
				TUNING.FOOD.FOOD_QUALITY_MORE_WONDERFUL,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true)
			.AddEffects(new List<string>()
			{
				"GoodEats" // Soul Food
			}, DlcManager.AVAILABLE_ALL_VERSIONS);

		public static EdiblesManager.FoodInfo granolaBar = new EdiblesManager.FoodInfo(
				GranolaBarConfig.ID,
				DlcManager.VANILLA_ID,
				700_000f,
				TUNING.FOOD.FOOD_QUALITY_GOOD,
				TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true);

		public static EdiblesManager.FoodInfo goopParfait = new EdiblesManager.FoodInfo(
				GoopParfaitConfig.ID,
				DlcManager.VANILLA_ID,
				2000_000f,
				TUNING.FOOD.FOOD_QUALITY_GREAT,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.QUICK,
				true)
			.AddEffects(new List<string>()
			{
				TEffects.SUGARHIGH
			}, DlcManager.AVAILABLE_ALL_VERSIONS);
	}
}
