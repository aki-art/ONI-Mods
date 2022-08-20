﻿using FUtility.SaveData;

namespace Slag.Settings
{
    public class Config : IUserSetting
    {
        public float InsulatedWindowTCMultiplier { get; set; } = 0.1f;

        public float MiteorEventCooldownInCycles { get; set; } = 30;

        public float RegolithToSlagMeltingRatio { get; set; } = 0.3f;
    }
}
