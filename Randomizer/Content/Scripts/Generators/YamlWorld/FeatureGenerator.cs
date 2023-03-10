using ProcGen;
using Randomizer.Elements;

namespace Randomizer.Content.Scripts.Generators.YamlWorld
{
    internal class FeatureGenerator : ProceduralGenerator<FeatureSettings>
    {
        public FeatureGenerator(SeededRandom rng, int minSize, int maxSize, ProcGen.Room.Shape shape, ElementComposition composition) : base(rng, "features", "feature")
        {
            var liquid = composition.elements[ElementCollector.LIQUID];
            bool lake = liquid != null && rng.RandomValue() < 0.5f;

            var feature = new FeatureSettings()
            {
                tags = new() {  WorldGenTags.AllowExceedNodeBorders.ToString() },
                blobSize = new MinMax(minSize, maxSize),
                shape = shape,
                borders = new() { rng.RandomRange(1, 2), rng.RandomRange(1, 2) },
                ElementChoiceGroups = new() 
                {
                    {
                        "RoomCenterElements",
                        new ()
                        {
                            choices = new()
                            {
                                new WeightedSimHash(composition.elements[ElementCollector.GAS].id.ToString(), 1f)
                            }
                        }
                    },
                    {
                        "RoomBorderChoices0",
                        new ()
                        {
                            choices = new()
                            {
                                new WeightedSimHash(composition.elements[ElementCollector.MINERAL1].id.ToString(), 1f),
                                new WeightedSimHash(composition.elements[ElementCollector.METAL1].id.ToString(), 0.5f),
                            },
                            selectionMethod = ProcGen.Room.Selection.HorizontalSlice
                        }
                    },
                    {
                        "RoomBorderChoices1",
                        new ()
                        {
                            choices = new()
                            {
                                new WeightedSimHash(composition.elements[ElementCollector.MINERAL1].id.ToString(), 1f)
                            }
                        }
                    }
                }
            };

            if(lake)
            {
                feature.tags.Add(WorldGenTags.Wet.ToString());
                feature.ElementChoiceGroups["RoomCenterElements"].choices.Add(new WeightedSimHash(liquid.id.ToString(), 0.5f));
            }

            WriteYaml(feature, name);
        }
    }
}
