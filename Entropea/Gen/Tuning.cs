using ProcGen;
using System.Collections.Generic;

namespace Entropea.Gen
{
    public class Tuning
    {
        public static List<WeightedTemperature> temperatures = new List<WeightedTemperature>()
            {
                new WeightedTemperature(Temperature.Range.ExtremelyCold, 1),
                new WeightedTemperature(Temperature.Range.VeryCold, 3),
                new WeightedTemperature(Temperature.Range.Cold, 4),
                new WeightedTemperature(Temperature.Range.Cool, 4),
                new WeightedTemperature(Temperature.Range.Mild, 5),
                new WeightedTemperature(Temperature.Range.Room, 5),
                new WeightedTemperature(Temperature.Range.HumanWarm, 4),
                new WeightedTemperature(Temperature.Range.HumanHot, 4),
                new WeightedTemperature(Temperature.Range.Hot, 3),
                new WeightedTemperature(Temperature.Range.VeryHot, 2),
                new WeightedTemperature(Temperature.Range.ExtremelyHot, 1)
            };
    }
}
