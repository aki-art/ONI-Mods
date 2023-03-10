using System.Collections.Generic;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    internal class OxygenModifiers
    {
        public Layer
            oxylite = new("OxygenModifier_Oxylite", OxyliteStart, Layer.True, 0, 3),
            oxygenVent = new("OxygenModifier_Vent", (rng, plan) => plan.AddVent(RandomOxygen(rng)), Layer.True, 1, 1),
            oxyfern = new("OxygenModifier_Oxyfern", (rng, plan) => plan.AddPrefab(OxyfernConfig.ID, 1), Layer.True, 2, 0),
            generatePuft = new("OxygenModifier_Puft", CreatePuft, Layer.True, 2, 0),
            peeLakes = new("OxygenModifier_PollutedWaterLakes", PeeLakes, Layer.True, 0, 4);

        public static List<string> breathables = new()
        {
            SimHashes.Oxygen.ToString(),
            SimHashes.ContaminatedOxygen.ToString(),
        };

        public static void Initialize()
        {
            if(Mod.isBeachedHere)
            {
                breathables.Add("SaltyOxygen");
            }
        }

        private static void OxyliteStart(SeededRandom rng, WorldPlan plan)
        {
            plan.AddElement(SimHashes.OxyRock, 0, true);
        }

        private static void CreatePuft(SeededRandom rng, WorldPlan plan)
        {
            plan.GeneratePrefab(PuftConfig.ID, RandomOxygen(rng), 500, 3);
        }

        private static string RandomOxygen(SeededRandom rng)
        {
            return ModDb.GetSeededRandom(breathables, rng);
        }

        private static void PeeLakes(SeededRandom rng, WorldPlan plan)
        {
            ProcGen.Room.Shape shape = rng.RandomRange(0, 3) switch
            {
                0 => ProcGen.Room.Shape.Circle,
                1 => ProcGen.Room.Shape.Oval,
                2 => ProcGen.Room.Shape.ShortWide,
                3 or _ => ProcGen.Room.Shape.TallThin
            };

            plan.QueueFeature(3, 7, shape, new List<string>() { RandomOxygen(rng), null });
        }

    }
}
