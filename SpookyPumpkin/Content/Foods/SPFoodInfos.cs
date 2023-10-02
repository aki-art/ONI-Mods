using System.Collections.Generic;

namespace SpookyPumpkinSO.Content.Foods
{
	public class SPFoodInfos
	{
		public static EdiblesManager.FoodInfo pumpkinPie = new EdiblesManager.FoodInfo(
				PumpkinPieConfig.ID,
				DlcManager.VANILLA_ID,
				6_000_000f,
				TUNING.FOOD.FOOD_QUALITY_WONDERFUL,
				TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true).AddEffects(new List<string>
				{
					SPEffects.HOLIDAY_SPIRIT
				}, DlcManager.AVAILABLE_ALL_VERSIONS);

		public static EdiblesManager.FoodInfo pumpkin = new EdiblesManager.FoodInfo(
				PumpkinConfig.ID,
				DlcManager.VANILLA_ID,
				600_000f,
				TUNING.FOOD.FOOD_QUALITY_AWFUL,
				TUNING.FOOD.DEFAULT_PRESERVE_TEMPERATURE,
				TUNING.FOOD.DEFAULT_ROT_TEMPERATURE,
				TUNING.FOOD.SPOIL_TIME.DEFAULT,
				true);

		public static EdiblesManager.FoodInfo toastedPumpkinSeeds = new EdiblesManager.FoodInfo(
				ToastedPumpkinSeedConfig.ID,
				DlcManager.VANILLA_ID,
				800_000f,
				TUNING.FOOD.FOOD_QUALITY_GOOD,
				TUNING.FOOD.HIGH_PRESERVE_TEMPERATURE,
				TUNING.FOOD.HIGH_ROT_TEMPERATURE,
				TUNING.FOOD.SPOIL_TIME.VERYSLOW,
				false);
	}
}
