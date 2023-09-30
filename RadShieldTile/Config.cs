using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace RadShieldTile
{
	[ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
	[RestartRequired]
	public class Config
	{
		[Option("STRINGS.RADSHIELDTILE.SETTINGS.SHIELDING.TITLE", "STRINGS.RADSHIELDTILE.SETTINGS.SHIELDING.TOOLTIP")]
		[JsonProperty]
		[Limit(0, 100)]
		public float Shielding { get; set; } = 95f;

		[Option("STRINGS.RADSHIELDTILE.SETTINGS.MELTINGPOINT.TITLE", "STRINGS.RADSHIELDTILE.SETTINGS.MELTINGPOINT.TOOLTIP")]
		[JsonProperty]
		[Limit(0, 9999)]
		public float MeltingPointKelvin { get; set; } = GameUtil.GetTemperatureConvertedToKelvin(1200, GameUtil.TemperatureUnit.Celsius);
	}
}
