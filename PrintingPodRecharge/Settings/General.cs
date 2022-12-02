using FUtility.SaveData;

namespace PrintingPodRecharge.Settings
{
    public class General : IUserSetting
    {
        public float RandomDupeReplaceChance { get; set; } = 0f;

        public float GetActualRandomReplaceChance() => Mod.errorOverrides == null ? RandomDupeReplaceChance : Mod.randoOverrideChance;

        public float RefundBioInkKg { get; set; } = 1f;

        public bool RefundActiveInk { get; set; } = true;

        public bool RefundeInk { get; set; } = true;

        public bool UIDupePreviews { get; set; } = true;

        public bool DebugTools { get; set; } = false;

        public bool ColoredMeeps { get; set; } = true;

        public int SettingVersion { get; set; } = 1;

        public bool TwitchIntegrationContent { get; set; } = true;

        public RandoDupeTier RandoDupePreset { get; set; } = RandoDupeTier.Default;

        public enum RandoDupeTier
        {
            Terrible,
            Vanillaish,
            Default,
            Generous,
            Wacky
        }
    }
}
