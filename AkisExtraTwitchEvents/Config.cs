using PeterHan.PLib.Options;

namespace Twitchery
{
	[ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
	[RestartRequired]
	public class Config
	{
		// ---------------------------- GENERAL ----------------------------------------------

		[Option(
			"STRINGS.AETE_CONFIG.GENERIC.RARITY.TITLE",
			"STRINGS.AETE_CONFIG.GENERIC.RARITY.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.A_GENERAL")]
		[Limit(0f, 1f)]
		public float EventsRarityModifier { get; set; }

		[Option(
			"STRINGS.AETE_CONFIG.GENERIC.BIAS.TITLE",
			"STRINGS.AETE_CONFIG.GENERIC.BIAS.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.A_GENERAL")]
		[Limit(0f, 1f)]
		public float EventRarityEqualizer { get; set; }

		[Option(
			"STRINGS.AETE_CONFIG.GENERIC.COLONY_LOST.TITLE",
			"STRINGS.AETE_CONFIG.GENERIC.COLONY_LOST.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.A_GENERAL")]
		public bool SuppressColonyLostMessage { get; set; }

		// ---------------------------- DUPLICANT SPAWNING ----------------------------------------------

		[Option(
			"STRINGS.AETE_CONFIG.DOUBLE_TROUBLE.MAX_DUPES.LABEL",
			"STRINGS.AETE_CONFIG.DOUBLE_TROUBLE.MAX_DUPES.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.I_DUPLICANTS")]
		[Limit(0, 300)]
		public int MaxDupes { get; set; }

		[Option(
			"STRINGS.AETE_CONFIG.DUPLICANTS.LIFETIME.TITLE",
			"STRINGS.AETE_CONFIG.DUPLICANTS.LIFETIME.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.I_DUPLICANTS")]
		[Limit(1f, float.MaxValue)]
		public float DoubleTrouble_DurationCycles { get; set; }

		[Option(
			"STRINGS.AETE_CONFIG.DUPLICANTS.OXYGEN.TITLE",
			"STRINGS.AETE_CONFIG.DUPLICANTS.OXYGEN.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.I_DUPLICANTS")]
		public int DoubleTrouble_OxygenConsumptionModifier { get; set; }

		// ---------------------------- VISUAL ----------------------------------------------

		[Option(
			"STRINGS.AETE_CONFIG.VISUAL.SHAKE.TITLE",
			"STRINGS.AETE_CONFIG.VISUAL.SHAKE.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.E_VISUAL")]
		[Limit(0f, 1.0f)]
		public float CameraShake { get; set; }

		[Option(
			"STRINGS.AETE_CONFIG.VISUAL.TRAIL.TITLE",
			"STRINGS.AETE_CONFIG.VISUAL.TRAIL.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.E_VISUAL")]
		public bool SuperDupe_RenderTrail { get; set; }

		// ---------------------------- WORLD EVENTS ----------------------------------------------

		[Option(
			"STRINGS.AETE_CONFIG.WORLD_EVENTS.SOLAR_STORM_DURATION.TITLE",
			"STRINGS.AETE_CONFIG.WORLD_EVENTS.SOLAR_STORM_DURATION.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.Q_WORLDEVENTS")]
		[Limit(0f, float.MaxValue)]
		public float SolarStorm_Duration_Cycles { get; set; }

		// ---------------------------- FOOD -------------------------------------------------------
		[Option(
			"STRINGS.AETE_CONFIG.FOOD.RADISH.TITLE",
			"STRINGS.AETE_CONFIG.FOOD.RADISH.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.M_FOOD")]
		[Limit(0f, float.MaxValue)]
		public float GiantRadish_Kcal { get; set; }

		[Option(
			"STRINGS.AETE_CONFIG.FOOD.PIZZA.TITLE",
			"STRINGS.AETE_CONFIG.FOOD.PIZZA.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.M_FOOD")]
		[Limit(0f, float.MaxValue)]
		public float Pizzabox_Kcal { get; set; }

		[Option(
			"STRINGS.AETE_CONFIG.FOOD.FROZEN_HONEY.TITLE",
			"STRINGS.AETE_CONFIG.FOOD.FROZEN_HONEY.TOOLTIP",
			"STRINGS.AETE_CONFIG.CATEGORIES.M_FOOD")]
		[Limit(0f, float.MaxValue)]
		public float FrozenHoney_Kcal { get; set; }

		public int Version { get; set; }

		public Config()
		{
			EventsRarityModifier = 1f;
			SuppressColonyLostMessage = true;

			MaxDupes = 40;
			DoubleTrouble_OxygenConsumptionModifier = -50;
			DoubleTrouble_DurationCycles = 1f;

			GiantRadish_Kcal = 160_000f;

			Pizzabox_Kcal = 38_400f;

			SolarStorm_Duration_Cycles = 1f;

			SuperDupe_RenderTrail = true;
			CameraShake = 1.0f;
			EventRarityEqualizer = 0.5f;
			FrozenHoney_Kcal = 80.0f;
		}
	}
}
