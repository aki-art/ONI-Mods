using System;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public abstract class Layer
    {
        public string ID;
        public int minDistanceFromStart;
        public int maxDistanceFromStart;

        Func<SeededRandom, WorldPlan, bool> conditionFn;

        public Layer(string ID)
        {
            this.ID = ID;
        }

        public Layer Distance(int min, int max)
        {
            minDistanceFromStart = min;
            maxDistanceFromStart = max;

            return this;
        }

        public Layer AddCondition(Func<SeededRandom, WorldPlan, bool> fn)
        {
            this.conditionFn = fn;
            return this;
        }

        public virtual bool Condition(SeededRandom random, WorldPlan plan) => conditionFn == null || true;

        public abstract void Apply(SeededRandom rng, WorldPlan plan);
    }
}
