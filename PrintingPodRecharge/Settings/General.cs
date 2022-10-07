using FUtility.SaveData;

namespace PrintingPodRecharge.Settings
{
    public class General : IUserSetting
    {
        public float RandomDupeReplaceChance { get; set; } = 0f;

        public float GetActualRandomReplaceChance() => Mod.errorOverrides == null ? RandomDupeReplaceChance : Mod.randoOverrideChance;

        public float RefundBioInkKg { get; set; } = 1f;

        public bool RefundActiveInk { get; set; } = true;

        public bool DebugTools { get; set; } = false;
    }
}
