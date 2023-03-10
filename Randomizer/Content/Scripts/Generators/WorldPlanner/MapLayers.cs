using JetBrains.Annotations;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public class MapLayers
    {
        public void ApplyStartWorldLayers(WorldPlan plan, SeededRandom rng, float minTemp, float maxTemp, float maxHardness)
        {
            new RandomFillLayer("Composition_RandomFill")
                .Apply(rng, plan);
            new CompositionLayer("Composition_Metal", Mod.elementCollector.metalOres, minTemp, maxTemp)
                .Apply(rng, plan);
            new CompositionLayer("Composition_Iron", Mod.elementCollector.irons, minTemp, maxTemp)
                .Distance(2, 6)
                .Apply(rng, plan);
            new LayerGroup("TemporaryOxygen")
            {
                new CompositionLayer("Composition_Breathable", Mod.elementCollector.breathables, minTemp, maxTemp)
                    .Distance(0, 0),
                new CompositionLayer("Composition_Oxylite", SimHashes.OxyRock)
                    .Distance(0, 0),
                new CompositionLayer("Composition_PollutedDirt", SimHashes.ToxicSand)
                    .Distance(0, 0),
            }.Apply(rng, plan, 2);
        }
    }
}
