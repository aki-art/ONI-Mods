using PeterHan.PLib.Options;

namespace Twitchery
{
	[ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
	public class Config
	{
		[Option(
			"Event Rarity Multiplier",
			"Modifies the rarity of all events from this mod.\n" +
			"If set to 0, all events are effectively disabled.\n\n" +
			"Use Twitch Integration's own settings to adjust individual event weights.",
			"General")]
		[Limit(0f, 1f)]
		public float EventsRarityModifier { get; set; }

		[Option(
			"Suppress Colony Lost Popup",
			"Suppress Colony Lost message if at least one Regular Pip, Midased Duplicant, Polymorph or Werevole is still alive.",
			"General")]
		public bool SuppressColonyLostMessage { get; set; }

		[Option(
			"Twitchery.STRINGS.AETE_CONFIG.DOUBLE_TROUBLE.MAX_DUPES.LABEL",
			"Twitchery.STRINGS.AETE_CONFIG.DOUBLE_TROUBLE.MAX_DUPES.TOOLTIP",
			"Twitchery.STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TOAST")]
		[Limit(0, 300)]
		public int MaxDupes { get; set; }

		[Option(
			"Render Trail",
			"Enable the orange trail effect on Duplicant who were made Super.",
			"Twitchery.STRINGS.AETE_EVENTS.SUPERDUPE.TOAST")]
		public bool SuperDupe_RenderTrail { get; set; }

		[Option(
			"Oxygen Consumption Modifier",
			"g/s oxygen consumption modifier.",
			"Twitchery.STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TOAST")]
		[Limit(-100, 100)]
		public int DoubleTrouble_OxygenConsumptionModifier { get; set; }

		[Option(
			"Duration",
			"...",
			"Twitchery.STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TOAST")]
		[Limit(0f, float.MaxValue)]
		public float DoubleTrouble_DurationCycles { get; set; }

		[Option(
			"Duration",
			"...",
			"Twitchery.STRINGS.AETE_EVENTS.CHATRAIT.TOAST")]
		[Limit(0f, float.MaxValue)]
		public float TwitchGuest_DurationCycles { get; set; }

		[Option(
			"Solar Storm Cycles Duration",
			"...",
			"Twitchery.STRINGS.AETE_EVENTS.SOLARSTORM.TOAST")]
		[Limit(0f, float.MaxValue)]
		public float SolarStorm_Duration_Cycles { get; set; }

		[Option(
			"Kcal",
			"...",
			"Twitchery.STRINGS.AETE_EVENTS.RAD_DISH.TOAST")]
		[Limit(0f, float.MaxValue)]
		public float GiantRadish_Kcal { get; set; }

		[Option(
			"Kcal",
			"...",
			"Twitchery.STRINGS.AETE_EVENTS.PIZZADELIVERY.TOAST")]
		[Limit(0f, float.MaxValue)]
		public float Pizzabox_Kcal { get; set; }

		[Option(
			"Camera Shake",
			"...",
			"Twitchery.STRINGS.AETE_EVENTS.PIZZADELIVERY.TOAST")]
		[Limit(0f, float.MaxValue)]
		public float CameraShake { get; set; }

		public int Version { get; set; }

		public Config()
		{
			EventsRarityModifier = 1f;
			SuppressColonyLostMessage = true;

			MaxDupes = 40;
			DoubleTrouble_OxygenConsumptionModifier = -10;
			DoubleTrouble_DurationCycles = 1f;
			TwitchGuest_DurationCycles = 1f;

			GiantRadish_Kcal = 160_000f;

			Pizzabox_Kcal = 38_400f;

			SolarStorm_Duration_Cycles = 1.5f;

			SuperDupe_RenderTrail = true;
			CameraShake = 1.0f;
		}
	}
}
