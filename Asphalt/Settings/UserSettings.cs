using System;

namespace Asphalt.Settings
{
    [Serializable]
    public class UserSettings
    {
        public UserSettings()
        {
            SpeedMultiplier = Tuning.DEFAULT_SPEED_MULTIPLIER;
            UseSafeFolder = true;
        }

        public float SpeedMultiplier { get; set; }
        public bool UseSafeFolder { get; set; }
    }
}