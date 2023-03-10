using System;
using System.Collections.Generic;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    public class WorldPlan
    {
        public List<GeyserPlan> geysers;
        public List<BiomePlan> biomes;

        public void AddElement(SimHashes simHashes, int distanceFromStart, bool force)
        {
        }

        public void AddVent(SimHashes simHashes)
        {
        }

        public void AddPrefab(string id, int distanceFromStart)
        {

        }

        public void GeneratePrefab(string baseId, string elementId, float amountPerCycleTarget, int distanceFromStart)
        {

        }

        internal void QueueFeature(int minSize, int maxSize, ProcGen.Room.Shape shape, List<string> elements)
        {
        }

        public class BiomePlan
        {
            public List<SimHashes> primaryElements;
            public int distanceFromStart;
            public bool isStart;

            public BiomePlan()
            {
                primaryElements = new List<SimHashes>();
            }
        }

        public class GeyserPlan
        {
            public SimHashes element;
            public float perCycleTarget;
            public int distanceFromStart;
        }
    }
}
