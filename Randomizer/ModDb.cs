using System;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Randomizer
{
    public class ModDb
    {
        public const int RANDOM_CLUSTER_CATEGORY = 1113654; // random number. if you are another modder copying this code, please change this to any other number
        public const int USERSAVED_CLUSTER_CATEGORY = 1113655; // similarly

        public static List<ZoneType> zoneTypesNoSpace = new();

        public static void Initialize()
        {
            foreach (ZoneType zoneType in Enum.GetValues(typeof(ZoneType)))
            {
                if (zoneType != ZoneType.Space)
                {
                    zoneTypesNoSpace.Add(zoneType);
                }
            }
        }

        public static List<string> prefix = new()
        {
            "cute", "wonderful", "precious", "lush", "dry", "spatious", "chaotic", "ordinary", "unusual", "common", "epic", "yucky", "arid", "circuitus",
            "complicated", "wild", "perfect", "squalid", "serene", "loving", "festering", "loud", "eternal", "philosophical", "fearless"
        };

        public static List<string> nameparts = new()
        {
            "pip", "tail", "moo", "pacu", "squirrel", "shovevole", "lettuce", "cuddle", "tropical", "wheeze", "wort", "lice", "crab", "pincher",
            "pokeshell", "drecko", "meep", "shinebug", "divergent", "sweetle", "slug", "plugslug", "beeta", "hatch", "puft", "glitterpuft", "puftprince",
            "slickster", "morb"
        };

        public static List<string> suffix = new()
        {
            "lands", "mountains", "plains", "caverns", "caves", "rivers", "islands", "place", "terrain", "garden", "forest"
        };

        public static T GetSeededRandom<T>(List<T> list, SeededRandom rng)
        {
            var index = rng.RandomRange(0, list.Count - 1);
            return list[index];
        }
    }
}
