using System;

namespace WorldCreep.Geysers
{
    public class GeyserEntry
    {
        public string ID { get; set; }
        public string Anim { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SimHashes Element { get; set; }
        public float Temperature { get; set; }
        public int MinRatePerCycle { get; set; }
        public int MaxRatePerCycle { get; set; }
        public int MaxPressure { get; set; }
        public string Disease { get; set; } = null;
        public int DiseaseCount { get; set; } = 20000;
        public string SolidElement { get; set; } = null;
        public int SolidTemperature { get; set; } = 300;
        public int SolidMass { get; set; } = 20;
        public bool AlwaysActive { get; set; } = false;
        public Entity[] Entities { get; set; } = null;
        public float MinIterationLength { get; set; } = Tuning.Geyser.MIN_ITERATION_LENGTH;
        public float MaxIterationLength { get; set; } =  Tuning.Geyser.MAX_ITERATION_LENGTH;
        public float MinIterationPercent { get; set; } = Tuning.Geyser.MIN_ITERATION_PERCENT;
        public float MaxIterationPercent { get; set; } = Tuning.Geyser.MAX_ITERATION_PERCENT;
        public float MinYearLength { get; set; } = Tuning.Geyser.MIN_YEAR_LENGTH;
        public float MaxYearLength { get; set; } = Tuning.Geyser.MAX_YEAR_LENGTH;
        public float MinYearPercent { get; set; } = Tuning.Geyser.MIN_YEAR_PERCENT;
        public float MaxYearPercent { get; set; } = Tuning.Geyser.MAX_YEAR_PERCENT;
    }

    [Serializable]
    public class Entity
    {
        public string Id { get; set; }
        public float Weight { get; set; } = 1f;
    }
}
