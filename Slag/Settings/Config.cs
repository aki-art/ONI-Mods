using FUtility.SaveData;
using Slag.Content.Critters.BrittleDrill;
using System.Collections.Generic;

namespace Slag.Settings
{
    public class Config : IUserSetting
    {
        public float InsulatedWindowTCMultiplier { get; set; } = 0.1f;

        public float MiteorEventCooldownInCycles { get; set; } = 30;

        public float RegolithToSlagMeltingRatio { get; set; } = 0.3f;

        public BrittleDrillSettings BrittleDrill { get; set; } = new BrittleDrillSettings();

        public class BrittleDrillSettings
        {
            public float LifeTime { get; set; } = 80f;

            public float Hp { get; set; } = 20f;

            public float DeltaKcalPerCycle { get; set; } = -4800f;

            public float MinProductInKcal { get; set; } = 2400f;

            public float StarveInCycles { get; set; } = 10f;

            public float KcalPerKgOfSand { get; set; } = 1f;

            public float KcalPerKgOfRegolith { get; set; } = 0.5f;
        }
    }
}
