using LibNoiseDotNet.Graphics.Tools.Noise;
using Moonlet.Utils;
using ProcGen.Noise;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class PrimitiveC : ShadowTypeBase<Primitive>, INoiseBase
	{
		public NoisePrimitive Primative { get; set; }
		public NoiseQuality Quality { get; set; }
		public IntNumber Seed { get; set; }
		public FloatNumber Offset { get; set; }
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }

		public PrimitiveC()
		{
			Pos = new Vector2FC(0, 0);
		}

		public override Primitive Convert()
		{
			return new Primitive()
			{
				primative = Primative,
				quality = Quality,
				seed = Seed.CalculateOrDefault(0),
				offset = Offset.CalculateOrDefault(0),
				name = Name,
				pos = Pos.ToVector2f()
			};
		}
	}
}
