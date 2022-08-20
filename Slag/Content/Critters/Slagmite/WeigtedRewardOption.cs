using ProcGen;

namespace Slag.Content.Critters.Slagmite
{
    public class WeigtedRewardOption : IWeighted
    {
        public float weight { get; set; }
        public Tag itemTag;
        public float totalMass;

        public WeigtedRewardOption(Tag itemTag, float totalMass, float weight = 1f)
        {
            this.itemTag = itemTag;
            this.totalMass = totalMass;
            this.weight = weight;
        }

        public WeigtedRewardOption(SimHashes item, float totalMass, float weight = 1f) : this(item.CreateTag(), totalMass, weight)
        {

        }
    }
}
