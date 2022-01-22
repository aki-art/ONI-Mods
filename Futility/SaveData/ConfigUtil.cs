namespace FUtility.SaveData
{
    public class ConfigUtil
    {
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
