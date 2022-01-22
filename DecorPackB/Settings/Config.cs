using FUtility.SaveData;
using System.Collections.Generic;

namespace DecorPackB.Settings
{
    public class Config : IUserSetting
    {
        public GlassTilesConfig GlassTile { get; set; } = new GlassTilesConfig();

        public FossilDisplayConfig FossilDisplay { get; set; } = new FossilDisplayConfig();

        public MoodLampConfig MoodLamp { get; set; } = new MoodLampConfig();

        // public bool SaveExternally { get; set; }

        public class FossilDisplayConfig
        {
            public Dictionary<SimHashes, float> FossilSources = new Dictionary<SimHashes, float>() {
                { SimHashes.Fossil, 0.05f },
                { SimHashes.Ice, 0.01f },
                { SimHashes.DirtyIce, 0.01f },
                { SimHashes.Sand, 0.01f },
                { SimHashes.Dirt, 0.005f },
                { SimHashes.SandStone, 0.005f }
            };

            public float FossileNoduleFromFossilChance_SpacedOut { get; set; } = 0.01f;

            public float FossileNoduleFromFossilChance_VanillaOrClassic { get; set; } = 0.05f;

            public RangedValue BaseDecor { get; set; } = new RangedValue()
            {
                Range = 8,
                Amount = 20
            };

            public RangedValue BadResearchBonus { get; set; } = new RangedValue()
            {
                Range = 8,
                Amount = 2
            };

            public RangedValue MediocreResearchBonus { get; set; } = new RangedValue()
            {
                Range = 8,
                Amount = 6
            };

            public RangedValue GiantFossilResearchBonus { get; set; } = new RangedValue()
            {
                Range = 16,
                Amount = 12
            };

            public int BadDecorBonus { get; set; } = 5;

            public int MediocreDecorBonus { get; set; } = 10;

            public int GiantFossilDecorBonus { get; set; } = 15;
        }

        public class MoodLampConfig
        {
            public bool VibrantColors { get; set; } = true;

            public RangedValue Lux { get; set; } = new RangedValue()
            {
                Range = 3,
                Amount = 400
            };

            public RangedValue Decor { get; set; } = new RangedValue()
            {
                Range = 4,
                Amount = 25
            };

            public PowerConfig PowerUse = new PowerConfig()
            {
                ExhaustKilowattsWhenActive = .5f,
                EnergyConsumptionWhenActive = 6f,
                SelfHeatKilowattsWhenActive = 0f
            };
        }

        public class GlassTilesConfig
        {
            public bool UseDyeTC { get; set; } = true;

            public float DyeRatio { get; set; }

            public float SpeedBonus { get; set; } = 1.25f;

            public RangedValue Decor { get; set; } = new RangedValue()
            {
                Range = 2,
                Amount = 10
            };
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
