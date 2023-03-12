using JetBrains.Annotations;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public class MapLayers
    {
        // complete world with everything in it
        public void ApplyVanillaStartWorldLayers(WorldPlan plan, SeededRandom rng, float minTemp, float maxTemp, float maxHardness)
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
                new CompositionLayer("Composition_Breathable", Mod.elementCollector.breathables, minTemp, maxTemp).Distance(0, 0),
                new CompositionLayer("Composition_Oxylite", SimHashes.OxyRock).Distance(0, 0),
                new CompositionLayer("Composition_PollutedDirt", SimHashes.ToxicSand).Distance(0, 0),
                new CompositionLayer("Composition_Algae", SimHashes.Algae).Distance(0, 0),
                new CompositionLayer("Composition_Rust", SimHashes.Rust).Distance(0, 0),
                new SingleElementFeatureLayer("Feature_OxyliteScatter", 2, 4, SimHashes.OxyRock).Distance(0, 0),
                new SingleElementFeatureLayer("Feature_Algae", 2, 4, SimHashes.OxyRock).Distance(0, 0),
                new SimpleCaveLayer("Feature_Peelakes", 4, 6, SimHashes.DirtyWater).Distance(0, 0),
                new SimpleCaveLayer("Feature_AlgaeCave", 4, 6, SimHashes.Algae).Distance(0, 0),
            }.Apply(rng, plan, 2);
        }
    }
}
