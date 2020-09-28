using ProcGen;

namespace Slag.Critter
{
    public class WeightedMetalOption : IWeighted
	{
		public SimHashes element { get; private set; }
		public float weight { get; set; }
		public WeightedMetalOption(SimHashes element)
		{
			this.element = element;
			weight = 1f;
		}

		public WeightedMetalOption(SimHashes element, float weight)
		{
			this.element = element;
			this.weight = weight;
		}
	}
}
