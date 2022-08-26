using Newtonsoft.Json;
using ProcGen;
using System;

namespace Slag.Content.Critters.Slagmite
{
    [Serializable]
    public class WeightedRewardOption : IWeighted
    {
        public float weight { get; set; }
        public string id;
        public float totalMass;

        [JsonConstructor]
        public WeightedRewardOption(string itemTag, float totalMass, float weight = 1f)
        {
            id = itemTag;
            this.totalMass = totalMass;
            this.weight = weight;
        }

        public WeightedRewardOption(SimHashes item, float totalMass, float weight = 1f) : this(item.ToString(), totalMass, weight)
        {

        }
    }
}
