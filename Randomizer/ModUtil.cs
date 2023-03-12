using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer
{
    internal class ModUtil
    {
        private static HashSet<ProcGen.Room.Shape> roomShapes = new()
        {
            ProcGen.Room.Shape.Blob,
            ProcGen.Room.Shape.Circle,
            ProcGen.Room.Shape.TallThin,
            ProcGen.Room.Shape.ShortWide,
            ProcGen.Room.Shape.Oval,
            ProcGen.Room.Shape.Splat
        };

        private static HashSet<ProcGen.Room.Shape> featureShapes = new()
        {
            ProcGen.Room.Shape.Blob,
            ProcGen.Room.Shape.Circle,
            ProcGen.Room.Shape.TallThin,
            ProcGen.Room.Shape.ShortWide,
            ProcGen.Room.Shape.Oval,
            ProcGen.Room.Shape.Line
        };

        public static ProcGen.Room.Shape GetRandomShape(SeededRandom rng, bool cave)
        {
            return GetSeededRandom(cave ? roomShapes : featureShapes, rng);
        }

        public static T GetSeededRandom<T>(IEnumerable<T> list, SeededRandom rng)
        {
            var index = rng.RandomRange(0, list.Count() - 1);
            return list.ElementAt(index);
        }
    }
}
