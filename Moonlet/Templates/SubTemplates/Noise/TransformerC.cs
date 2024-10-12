using Moonlet.Utils;
using ProcGen.Noise;
using System;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class TransformerC : IShadowTypeBase<Transformer>, INoiseBase
	{
		public Transformer.TransformerType TransformerType { get; set; }
		public FloatNumber Power { get; set; }
		public Vector2FC Vector { get; set; }
		public Vector2FC Rotation { get; set; } // Noise Not Included output, maps to Vector
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }

		public TransformerC()
		{
			TransformerType = Transformer.TransformerType.Displace;
			Vector = new Vector2FC(0, 0);
		}

		public Transformer Convert(Action<string> log)
		{
			var rotation = Rotation ?? Vector;
			return new Transformer()
			{
				vector = rotation.ToVector2f(),
				power = Power.CalculateOrDefault(0),
				pos = Pos.ToVector2f(),
				transformerType = TransformerType,
				name = Name
			};
		}
	}
}
