using ProcGen.Noise;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class TransformerC : ShadowTypeBase<Transformer>, INoiseBase
	{
		public Transformer.TransformerType TransformerType { get; set; }
		public float Power { get; set; }
		public Vector2FC Vector { get; set; }
		public Vector2FC Rotation { get; set; } // Noise Not Included output, maps to Vector
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }

		public override Transformer Convert()
		{
			var rotation = Rotation ?? Vector;
			return new Transformer()
			{
				vector = rotation.ToVector2f(),
				power = Power,
				pos = Pos.ToVector2f(),
				transformerType = TransformerType,
				name = Name
			};
		}
	}
}
