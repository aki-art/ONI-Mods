using FUtility.SaveData;

namespace AETNTweaks.Settings
{
    public class Config : IUserSetting
    {
        public float PulseFrequency { get; set; } = 10f;

        public float PyrositeActivityDuration { get; set; } = 5f;

        public int PyrositeAttachRadius { get; internal set; } = 4;
    }
}
