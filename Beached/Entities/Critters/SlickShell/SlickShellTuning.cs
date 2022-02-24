using System.Collections.Generic;

namespace Beached.Entities.Critters.SlickShell
{
    internal class SlickShellTuning
    {
        public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
        {
            new FertilityMonitor.BreedingChance
            {
                egg = SlickShellConfig.EGG_ID.ToTag(),
                weight = 1f
            }
        };

        public static string ON_DEATH_DROP = CrabShellConfig.ID;
        public static float MASS = 50f;
    }
}
