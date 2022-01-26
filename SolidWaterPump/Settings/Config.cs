using FUtility;
using FUtility.SaveData;

namespace SolidWaterPump.Settings
{
    public class Config : IUserSetting
    {
        public RangedValue Decor { get; set; } = new RangedValue()
        {
            Range = 0,
            Amount = 0
        };

        public string[] ConstructionMaterial { get; set; } = TUNING.MATERIALS.ALL_MINERALS;

        public float[] ConstructionMass { get; set; } = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;

        public float ConstructionTime { get; set; } = TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2;

        public class RangedValue
        {
            public int Range { get; set; }

            public int Amount { get; set; }
        }

        public static void Sanitize(Config config)
        {
            if (config.ConstructionMaterial.Length != config.ConstructionMass.Length)
            {
                Log.Warning($"Construction Material and Construction Mass have mismatched counts: {config.ConstructionMaterial.Length} materials and {config.ConstructionMass.Length} mass was given.");
                config.ConstructionMaterial = TUNING.MATERIALS.ALL_MINERALS;
                config.ConstructionMass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            }
        }
    }
}
