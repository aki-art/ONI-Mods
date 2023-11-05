using HarmonyLib;
using Moonlet.Utils;
using ProcGen.Noise;
using System;
using static ProcGen.Noise.Filter;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class FilterC : ShadowTypeBase<Filter>, INoiseBase
	{
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }
		public string Filter { get; set; }
		public FloatNumber Frequency { get; set; }
		public FloatNumber Lacunarity { get; set; }
		public IntNumber Octaves { get; set; }
		public FloatNumber Offset { get; set; }
		public FloatNumber Gain { get; set; }
		public FloatNumber Exponent { get; set; }
		public FloatNumber Scale { get; set; }
		public FloatNumber Bias { get; set; }

		public FilterC()
		{
			Pos = new Vector2FC(0, 0);
		}

		public override Filter Convert(Action<string> log = null)
		{
			var filter = NoiseFilter.RidgedMultiFractal;

			if (!Filter.IsNullOrWhiteSpace())
			{
				if (!Enum.TryParse(Filter, out filter))
					log($"{Filter} is not a valid NoiseFilter. Options are: {Enum.GetNames(typeof(NoiseFilter)).Join()}");
			}

			return new Filter()
			{
				pos = Pos.ToVector2f(),
				filter = filter,
				frequency = Frequency.CalculateOrDefault(0.1f),
				lacunarity = Lacunarity.CalculateOrDefault(3f),
				octaves = Octaves.CalculateOrDefault(0),
				offset = Offset.CalculateOrDefault(1f),
				gain = Gain.CalculateOrDefault(1f),
				exponent = Exponent.CalculateOrDefault(0.9f),
				scale = Scale.CalculateOrDefault(1f),
				bias = Bias.CalculateOrDefault(0f)
			};
		}
	}
}
