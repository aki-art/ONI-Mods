﻿using FUtility.SaveData;
using SnowSculptures.Content.Buildings;
using System.Collections.Generic;

namespace SnowSculptures.Settings
{
    public class Config : IUserSetting
    {
        public int SnowMachineMaxParticles { get; set; } = 200;

        public HashSet<string> GlassCaseSealables { get; set; } = new HashSet<string>()
        {
            SnowSculptureConfig.ID,
            IceSculptureConfig.ID
        };

        public RangedValue SnowMachineDecor { get; set; } = new RangedValue()
        {
            Range = 3,
            Amount = 10
        };

        public PowerConfig SnowMachinePower { get; set; } = new PowerConfig()
        {
            ExhaustKilowattsWhenActive = .2f,
            EnergyConsumptionWhenActive = 2f,
            SelfHeatKilowattsWhenActive = 0f
        };

        public SculptureConfig Snowman { get; set; } = new SculptureConfig();

        public class SculptureConfig
        {
            public RangedValue BaseDecor { get; set; } = new RangedValue()
            {
                Range = 4,
                Amount = 10
            };

            public int BadSculptureDecorBonus { get; set; } = 5;

            public int MediocreSculptureDecorBonus { get; set; } = 10;

            public int GeniousSculptureDecorBonus { get; set; } = 15;
        }

        public class RangedValue
        {
            public int Range { get; set; }

            public int Amount { get; set; }
        }

        public class PowerConfig
        {
            public float ExhaustKilowattsWhenActive { get; set; }

            public float EnergyConsumptionWhenActive { get; set; }

            public float SelfHeatKilowattsWhenActive { get; set; }
        }
    }
}
