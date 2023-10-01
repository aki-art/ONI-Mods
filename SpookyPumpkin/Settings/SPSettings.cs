using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace SpookyPumpkinSO.Settings
{
	[ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
	[ModInfo("Spooky Pumpkin", "assets/magicalpuftgirl3.png")]
	[RestartRequired]
	public class SPSettings
	{
		[Option("SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.ROT.TITLE", "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.ROT.TOOLTIP")]
		[JsonProperty]
		public bool UseRot { get; set; }

		[Option("SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHOSTPIP_LIGHT.TITLE", "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHOSTPIP_LIGHT.TOOLTIP")]
		[JsonProperty]
		public bool GhostPipLight { get; set; }

		[Option("SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_LOOKS.TITLE", "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_LOOKS.TOOLTIP")]
		[JsonProperty]
		public bool UseGhastlyVisualEffect { get; set; }

		[Option("SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_BONUS.TITLE", "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_BONUS.TOOLTIP")]
		[JsonProperty]
		[Limit(0, 100)]
		public float GhastlyWorkBonus { get; set; }

		[Option("SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_STRESS_BONUS.TITLE", "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_STRESS_BONUS.TOOLTIP")]
		[JsonProperty]
		[Limit(0, 100)]
		public float GhastlyStressBonus { get; set; }

		public SPSettings()
		{
			UseRot = true;
			GhostPipLight = true;
			GhastlyWorkBonus = 10f;
			GhastlyStressBonus = 5f;
			UseGhastlyVisualEffect = true;
		}
	}
}
