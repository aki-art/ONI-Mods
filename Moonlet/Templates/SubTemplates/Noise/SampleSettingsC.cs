using Moonlet.Utils;
using ProcGen.Noise;
using System;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class SampleSettingsC : ShadowTypeBase<SampleSettings>, INoiseBase
	{
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }
		public FloatNumber Zoom { get; set; }
		public bool Normalise { get; set; }
		public bool Seamless { get; set; }
		public Vector2FC LowerBound { get; set; }
		public Vector2FC UpperBound { get; set; }

		public SampleSettingsC()
		{
			Pos = new Vector2FC(0, 0);
			LowerBound = new Vector2FC(2, 2);
			UpperBound = new Vector2FC(4, 4);
			Normalise = true;
		}

		public override SampleSettings Convert(Action<string> log)
		{
			var result = new SampleSettings
			{
				upperBound = UpperBound.ToVector2f(),
				lowerBound = LowerBound.ToVector2f(),
				seamless = Seamless,
				normalise = Normalise,
				pos = Pos.ToVector2f(),
				zoom = Zoom.CalculateOrDefault(0.001f)
			};

			return result;
		}
	}
}
