using ProcGen;
using System;
using System.Collections.Generic;

namespace WorldScrambler.Gen.Mirrors
{
    [Serializable]
    class World
    {

        public string filePath;

        public string name { get; set; }

        public string description { get; set; }

        public string coordinatePrefix { get; set; }

        public string spriteName { get; set; }

        public int difficulty { get; set; }

        public int tier { get; set; }

        public bool disableWorldTraits { get; set; }

        public ProcGen.World.Skip skip { get; set; }

        public bool noStart { get; set; }

        public serializableVector2I worldsize { get; set; }

        public DefaultSettings defaultsOverrides { get; set; }

        public ProcGen.World.LayoutMethod layoutMethod { get; set; }

        public List<WeightedName> subworldFiles { get; set; }

        public List<AllowedCellsFilter> unknownCellsAllowedSubworlds { get; set; }

        public string startSubworldName { get; set; }

        public string startingBaseTemplate { get; set; }

        public MinMax startingBasePositionHorizontal { get; set; }

        public MinMax startingBasePositionVertical { get; set; }

        public Dictionary<string, int> globalFeatureTemplates { get; set; }

        public Dictionary<string, int> globalFeatures { get; set; }

        [Serializable]
        public class serializableVector2I
        {
            public int X { get; set; }
            public int Y { get; set; }
            public serializableVector2I(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [Serializable]
        public class AllowedCellsFilter
        {
            public AllowedCellsFilter()
            {
                temperatureRanges = new List<Temperature.Range>();
                zoneTypes = new List<SubWorld.ZoneType>();
                subworldNames = new List<string>();
            }

            public string tagcommand { get; set; }

            public string tag { get; set; }

            public int minDistance { get; set; }

            public int maxDistance { get; set; }

            public int distCmp { get; set; }

            public string command { get; set; }

            public List<Temperature.Range> temperatureRanges { get; set; }
            public List<SubWorld.ZoneType> zoneTypes { get; set; }

            public List<string> subworldNames { get; set; }
        }
    }
}
