using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Content.Scripts.Generators.WorldPlanner
{
    internal class Planner
    {
        SeededRandom rng;

        public void Generate(SeededRandom rng)
        {
            this.rng = rng;
            EnsureOxygen();
            EnsureWater();
            EnsureFood();
            EnsureOil();
            EnsurePower();
            EnsureRocketry();
        }

        private void EnsureOxygen()
        {
            throw new NotImplementedException();
        }
    }
}
