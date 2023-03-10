using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    internal class CaveLayer : Layer
    {
        public CaveLayer(ProcGen.Room.Shape shape, int minSize, int maxSize) { }

        public override void Apply(SeededRandom rng, WorldPlan plan)
        {
            throw new NotImplementedException();
        }
    }
}
