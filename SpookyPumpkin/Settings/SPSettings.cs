using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace SpookyPumpkinSO.Settings
{
    [ModInfo("Spooky Pumpkin", "assets/magicalpuftgirl3.png")]
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    public class SPSettings
    {
        [Option(
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.ROT.TITLE",
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.ROT.TOOLTIP")]
        [JsonProperty]
        public bool UseRot { get; set; }

        [Option(
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHOSTPIP_LIGHT.TITLE",
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHOSTPIP_LIGHT.TOOLTIP")]
        [JsonProperty]
        public bool GhostPipLight { get; set; }

        [Option(
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_BONUS.TITLE",
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_BONUS.TOOLTIP")]
        [JsonProperty]
        public float GhastlyWorkBonus { get; set; }

        [Option(
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_BONUS.TITLE",
            "SpookyPumpkinSO.STRINGS.UI.MODSETTINGS.GHASTLY_BONUS.TOOLTIP")]
        [JsonProperty]
        public bool UseGhastlzVisualEffect { get; set; }

        public SPSettings()
        {
            UseRot = true;
            GhostPipLight = true;
            GhastlyWorkBonus = 0.1f;
            UseGhastlzVisualEffect = true;
        }
    }
}
