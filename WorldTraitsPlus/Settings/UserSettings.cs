using static MathUtil;

namespace WorldTraitsPlus.Settings
{
    public class UserSettings
    {
        public MinMax WorldEventDelay { get; set; } = new MinMax(0, 5);
        public int EarthquakeSafeRadius { get; set; } = 22;
        public MinMax EarthquakeMagnitude { get; set; } = new MinMax(0.2f, 0.9f);
        public MinMax EarthquakeDuration { get; set; } = new MinMax(60, 120);
        public MinMax EclipseDuration { get; set; } = new MinMax(60, 120);
    }
}
