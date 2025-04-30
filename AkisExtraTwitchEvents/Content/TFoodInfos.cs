using Twitchery.Content.Defs;
using Twitchery.Content.Defs.Foods;
using static EdiblesManager;

namespace Twitchery.Content
{
	public class TFoodInfos
	{
		public const float JELLO_KCAL_PER_KG = 400f; // has to be very low, because this is spammed
		public const float MACARONI_KCAL_PER_KG = 16f;
		public const float RADISH_KCAL_PER_KG = 800f;
		public const float PIZZA_KCAL_PER_KG = 3200f;

		public static FoodInfo jello = new(
				Elements.Jello.ToString(),
				JELLO_KCAL_PER_KG * 1000f,
				3,
				255.15f,
				277.15f,
				2400f,
				false);

		public static FoodInfo macaroni = new(
				Elements.Macaroni.ToString(),
				MACARONI_KCAL_PER_KG * 1000f,
				1,
				255.15f,
				277.15f,
				2400f,
				false);

		public static FoodInfo honeyPopsicle = new FoodInfo(
				Elements.FrozenHoney.ToString(),
				80 * 1000f,
				4,
				255.15f,
				277.15f,
				2400f,
				false)
			.AddEffects([TEffects.HONEY]);

		public static FoodInfo rawRadish = new FoodInfo(
				RawRadishConfig.ID,
				RADISH_KCAL_PER_KG * 1000f,
				TUNING.FOOD.FOOD_QUALITY_MEDIOCRE,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true)
			.AddEffects([TEffects.RADISH_STRENGTH]);

		public static FoodInfo cookedRadish = new FoodInfo(
				CookedRadishConfig.ID,
				1800_000f,
				TUNING.FOOD.FOOD_QUALITY_AMAZING,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true,
				DlcManager.EXPANSION1)
			.AddEffects([TEffects.RADISH_STRENGTH], DlcManager.EXPANSION1);

		public static FoodInfo pizza = new FoodInfo(
				PizzaConfig.ID,
				PIZZA_KCAL_PER_KG * 1000f,
				TUNING.FOOD.FOOD_QUALITY_MORE_WONDERFUL,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true)
			.AddEffects(["GoodEats"]);

		public static FoodInfo granolaBar = new FoodInfo(
				GranolaBarConfig.ID,
				700_000f,
				TUNING.FOOD.FOOD_QUALITY_GOOD,
				TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true);

		public static FoodInfo goopParfait = new FoodInfo(
				GoopParfaitConfig.ID,
				2000_000f,
				TUNING.FOOD.FOOD_QUALITY_GREAT,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.QUICK,
				true)
			.AddEffects([TEffects.SUGARHIGH]);

		public static FoodInfo lemon = new FoodInfo(
				LemonConfig.ID,
				800_000f,
				TUNING.FOOD.FOOD_QUALITY_GOOD,
				GameUtil.GetTemperatureConvertedToKelvin(-18.15f, GameUtil.TemperatureUnit.Celsius),
				GameUtil.GetTemperatureConvertedToKelvin(4f, GameUtil.TemperatureUnit.Celsius),
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true)
			.AddEffects([TEffects.LEMON]);

		public static FoodInfo macNCheese = new FoodInfo(
				MacAndCheeseConfig.ID,
				3200_000f,
				TUNING.FOOD.FOOD_QUALITY_AMAZING,
				TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true)
			.AddEffects([TEffects.COMFORT_FOOD]);

		public static FoodInfo beachedAstrobar = new(
				BeachedAstrobarConfig.ID,
				1000_000f,
				TUNING.FOOD.FOOD_QUALITY_AMAZING,
				TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
				TUNING.FOOD.SPOIL_TIME.VERYSLOW,
				false);
	}
}
