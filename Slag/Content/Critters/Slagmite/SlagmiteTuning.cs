using System.Collections.Generic;
using TUNING;

namespace Slag.Content.Critters.Slagmite
{
    public class SlagmiteTuning
    {
        public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
        {
            new FertilityMonitor.BreedingChance
            {
                egg = SlagmiteConfig.EGG_ID,
                weight = 1f
            }
        };

        public static float STANDARD_CALORIES_PER_CYCLE = 100000f;
        public static float STANDARD_STARVE_CYCLES = 10f;
        public static float STANDARD_STOMACH_SIZE = STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;
        public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;
        public static float EGG_MASS = 0.33f;

        public class BASE
        {
            public static float KG_ORE_EATEN_PER_CYCLE = 70f;
            public static float CALORIES_PER_KG_OF_ORE = STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;
            public static float CALORIES_PER_KG_OF_ORE_POOR = STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE * 0.5f;
            public static float MIN_POOP_SIZE_IN_KG = 25f;
            public static int EGG_SORT_ORDER = 0;
        }
    }
}
