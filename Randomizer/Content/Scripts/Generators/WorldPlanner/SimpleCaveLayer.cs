using System.Collections.Generic;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public class SimpleCaveLayer : Layer
    {
        private readonly int minSize;
        private readonly int maxSize;
        private readonly SimHashes insideElement;

        public SimpleCaveLayer(string ID, int minSize, int maxSize, SimHashes insideElement) : base(ID)
        {
            this.minSize = minSize;
            this.maxSize = maxSize;
            this.insideElement = insideElement;
        }

        public override void Apply(SeededRandom rng, WorldPlan plan)
        {
            plan.QueueFeature(minSize, maxSize, ModUtil.GetRandomShape(rng, true), new List<string>() { insideElement.ToString() });
        }
    }
}
