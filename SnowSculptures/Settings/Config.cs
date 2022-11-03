using FUtility.SaveData;

namespace SnowSculptures.Settings
{
    public class Config : IUserSetting
    {
        public SculptureConfig Snowman { get; set; } = new SculptureConfig();

        public bool PreventInstantDebugMelt { get; set; } = true;

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
