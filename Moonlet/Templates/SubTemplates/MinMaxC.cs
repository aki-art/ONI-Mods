using Moonlet.Utils;
using ProcGen;

namespace Moonlet.Templates.SubTemplates
{
	public class MinMaxC
	{
		public TemperatureNumber Min { get; set; }

		public TemperatureNumber Max { get; set; }

		public MinMax ToMinMax()
		{
			return new MinMax(Min.CalculateOrDefault(0), Max.CalculateOrDefault(0));
		}

		public Temperature ToTemperature()
		{
			return new Temperature()
			{
				min = Min.CalculateOrDefault(0),
				max = Max.CalculateOrDefault(0)
			};
		}
	}
}
