using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace SpookyPumpkinSO.Settings
{
	[ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
	[ModInfo("Spooky Pumpkin", "assets/magicalpuftgirl3.png")]
	[RestartRequired]
	public class SPSettings
	{
		[Option("STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.ROT.TITLE", "STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.ROT.TOOLTIP")]
		[JsonProperty]
		public bool UseRot { get; set; }

		[Option("STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHOSTPIP_LIGHT.TITLE", "STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHOSTPIP_LIGHT.TOOLTIP")]
		[JsonProperty]
		public bool GhostPipLight { get; set; }

		[Option("STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHASTLY_LOOKS.TITLE", "STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHASTLY_LOOKS.TOOLTIP")]
		[JsonProperty]
		public bool UseGhastlyVisualEffect { get; set; }

		[Option("STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHASTLY_BONUS.TITLE", "STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHASTLY_BONUS.TOOLTIP")]
		[JsonProperty]
		[Limit(0, 100)]
		public float GhastlyWorkBonus { get; set; }

		[Option("STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHASTLY_STRESS_BONUS.TITLE", "STRINGS.UI.SPOOKYPUMPKIN_MODSETTINGS.GHASTLY_STRESS_BONUS.TOOLTIP")]
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
