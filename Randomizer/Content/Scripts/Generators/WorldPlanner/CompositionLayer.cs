using Randomizer.Elements;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public class CompositionLayer : Layer
    {
        private readonly SimHashes element;

        public CompositionLayer(string ID, SimHashes element) : base(ID)
        {
            this.element = element;
        }

        public CompositionLayer(string ID, ElementCollection elementCollection, float minTemp, float highTemp, float maxHardness = ElementCollector.DIG_HARDNESS.SOFT) : base(ID)
        {
            this.element = elementCollection.Get(minTemp, highTemp, maxHardness).id;
        }

        public override void Apply(SeededRandom rng, WorldPlan plan)
        {
            foreach (var biome in plan.biomes)
            {
                if (biome.distanceFromStart <= maxDistanceFromStart)
                {
                    biome.primaryElements.Add(element);
                    return;
                }
            }
        }
    }
}
