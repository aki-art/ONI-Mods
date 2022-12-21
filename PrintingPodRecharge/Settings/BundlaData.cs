using FUtility;

namespace PrintingPodRecharge.Settings
{
    public class BundlaData
    {
        public Rando defaultRandoPreset;

        public Rando ActiveRando(MinionStartingStats stats)
        {
            if(Mod.errorOverrides != null)
            {
                Log.Debuglog("rolling an ERR0R dupe");
                return Mod.errorOverrides;
            }

            return RandoDupePresets.Get(Mod.Settings.RandoDupePreset);
        }

        public Vacillating vacillating;
        public Egg egg;

        public class Egg
        {
            public int EggCycle { get; set; } = 225;

            public int RainbowEggCycle { get; internal set; } = 250;
        }

        public class Rando
        {
            public int MinimumSkillBudgetModifier { get; set; }

            public int MaximumSkillBudgetModifier { get; set; }

            public int MaximumTotalBudget { get; set; }

            public int MaxBonusPositiveTraits { get; set; }

            public int MaxBonusNegativeTraits { get; set; }

            public float ChanceForVacillatorTrait { get; set; }

            public float ChanceForNoNegativeTraits { get; set; }
        }

        public class Vacillating
        {
            public int ExtraSkillBudget { get; set; }
        }
    }
}
