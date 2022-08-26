using FUtility.SaveData;
using Slag.Content.Critters.Slagmite;
using System.Collections.Generic;

namespace Slag.Settings
{
    public class MiteRewards : IUserSetting
    {
        public List<WeightedRewardOption> Rewards { get; set; }

        public void SanitizeRewards()
        {
            Rewards.RemoveAll(r => ElementLoader.GetElement(r.id) == null);
        }
    }

    public class SlagmiteRewards : MiteRewards
    {
        public SlagmiteRewards()
        {
            Rewards = new List<WeightedRewardOption>()
            {
                new WeightedRewardOption(SimHashes.IronOre, 200f),
                new WeightedRewardOption(SimHashes.Cuprite, 200f),
                new WeightedRewardOption(SimHashes.Wolframite, 200f, 0.5f),
                new WeightedRewardOption(SimHashes.AluminumOre, 200f, 1f),
                new WeightedRewardOption(SimHashes.Cobaltite, 200f, 1f),
                new WeightedRewardOption("BismuthOre", 200f, 1f)
            };
        }
    }

    public class GleamiteRewards : MiteRewards
    {
        public GleamiteRewards()
        {
            Rewards = new List<WeightedRewardOption>()
            {
                new WeightedRewardOption(SimHashes.Iron, 200f),
                new WeightedRewardOption(SimHashes.Copper, 200f),
                new WeightedRewardOption(SimHashes.Wolframite, 200f, 0.5f),
                new WeightedRewardOption(SimHashes.Steel, 200f, 0.2f),
                new WeightedRewardOption(SimHashes.Aluminum, 200f, 1f),
                new WeightedRewardOption(SimHashes.TempConductorSolid, 50f, 0.05f),
                new WeightedRewardOption(SimHashes.Tungsten, 70f, 0.1f),
                new WeightedRewardOption(SimHashes.Niobium, 70f, 0.1f),
                new WeightedRewardOption(SimHashes.Cobalt, 200f, 1f),
                new WeightedRewardOption("Bismuth", 200f, 1f)
            };
        }
    }
}
