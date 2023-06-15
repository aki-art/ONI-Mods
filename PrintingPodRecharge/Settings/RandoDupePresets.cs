using PrintingPodRecharge.Content.Cmps;
using static PrintingPodRecharge.Settings.General;

namespace PrintingPodRecharge.Settings
{
    public class RandoDupePresets
    {
        public static BundlaData.Rando Get(RandoDupeTier tier)
        {
            switch (tier)
            {
                case RandoDupeTier.Terrible:
                    return new BundlaData.Rando()
                    {
                        MinimumSkillBudgetModifier = -8,
                        MaximumSkillBudgetModifier = 2,
                        MaximumTotalBudget = 0,
                        MaxBonusPositiveTraits = 0,
                        MaxBonusNegativeTraits = 3,
                        ChanceForVacillatorTrait = 0,
                        ChanceForNoNegativeTraits = 0
                    };
                case RandoDupeTier.Vanillaish:
                    return new BundlaData.Rando()
                    {
                        MinimumSkillBudgetModifier = 0,
                        MaximumSkillBudgetModifier = 0,
                        MaximumTotalBudget = 0,
                        MaxBonusPositiveTraits = 0,
                        MaxBonusNegativeTraits = 0,
                        ChanceForVacillatorTrait = 0,
                        ChanceForNoNegativeTraits = 0
                    };
                case RandoDupeTier.Generous:
                    return new BundlaData.Rando()
                    {
                        MinimumSkillBudgetModifier = 0,
                        MaximumSkillBudgetModifier = 13,
                        MaximumTotalBudget = 20,
                        MaxBonusPositiveTraits = 4,
                        MaxBonusNegativeTraits = 1,
                        ChanceForVacillatorTrait = 0.2f,
                        ChanceForNoNegativeTraits = 0.4f
                    };
                case RandoDupeTier.Wacky:
                    return new BundlaData.Rando()
                    {
                        MinimumSkillBudgetModifier = -13,
                        MaximumSkillBudgetModifier = 16,
                        MaximumTotalBudget = 20,
                        MaxBonusPositiveTraits = 35,
                        MaxBonusNegativeTraits = 5,
                        ChanceForVacillatorTrait = 0.3f,
                        ChanceForNoNegativeTraits = 0.2f
                    };
                case RandoDupeTier.Default:
                default:
                    return BundleLoader.bundleSettings.defaultRandoPreset;
            }
        }
    }
}
