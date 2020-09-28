using KSerialization.Converters;
using Newtonsoft.Json;
using ProcGen;

namespace Entropea.Gen
{
    public class WeightedTemperature : IWeighted
    {
        public Temperature.Range Range { get; private set; }
        public float weight { get; set; }

        public WeightedTemperature() { }

        public WeightedTemperature(Temperature.Range range, float weight) : this()
        {
            Range = range;
            this.weight = weight;
        }
    }
}
