namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public class SingleElementFeatureLayer : Layer
    {
        private readonly int minSize;
        private readonly int maxSize;
        private readonly SimHashes element;

        public SingleElementFeatureLayer(string ID, int minSize, int maxSize, SimHashes element) : base(ID)
        {
            this.minSize = minSize;
            this.maxSize = maxSize;
            this.element = element;
        }

        public override void Apply(SeededRandom rng, WorldPlan plan)
        {
            plan.QueueFeature(minSize, maxSize, ModUtil.GetRandomShape(rng, false), new() { element.ToString() });
        }
    }
}
