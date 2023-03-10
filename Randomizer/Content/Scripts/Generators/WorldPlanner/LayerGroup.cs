using ProcGen;
using System.Collections.Generic;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public class LayerGroup : List<LayerGroup.WeightedLayer>
    {
        private readonly string ID;

        public LayerGroup(string ID) => this.ID = ID;

        public void Apply(SeededRandom rng, WorldPlan world, int amount = 1, bool allowDuplicates = false)
        {
            for(int i = 0; i < amount; i++)
            {
                if(Count == 0)
                {
                    Log.Warning($"Trying to roll for te {i}th layer, but all options ran out: {ID}");
                    return;
                }

                var choice = this.GetWeightedRandom(rng);
                choice.layer.Apply(rng, world);

                if(!allowDuplicates)
                {
                    Remove(choice);
                }
            }
        }

        public void Add(Layer layer, float weight = 1f)
        {
            Add(new WeightedLayer(layer, weight));
        }

        public class WeightedLayer : IWeighted
        {
            public float weight { get; set; }
            public Layer layer;

            public WeightedLayer(Layer layer, float weight = 1f)
            {
                this.layer = layer;
                this.weight = weight;
            }
        }
    }
}
