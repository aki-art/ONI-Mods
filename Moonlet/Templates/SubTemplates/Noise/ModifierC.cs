using Moonlet.Utils;
using static ProcGen.Noise.Modifier;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class ModifierC : ShadowTypeBase<ProcGen.Noise.Modifier>, INoiseBase
	{
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }
		public ModifyType ModifyType { get; set; }
		public FloatNumber Lower { get; set; }
		public FloatNumber Upper { get; set; }
		public FloatNumber Exponent { get; set; }
		public bool Invert { get; set; }
		public FloatNumber Scale { get; set; }
		public FloatNumber Bias { get; set; }
		public Vector2FC Scale2d { get; set; }

		public ModifierC()
		{
			ModifyType = ModifyType.Abs;
			Lower = -1f;
			Upper = 1f;
			Exponent = 0.02f;
			Invert = false;
			Scale = 1f;
			Bias = 0f;
			Scale2d = new Vector2FC(1, 1);
			Pos = new Vector2FC(0, 0);
		}

		public override ProcGen.Noise.Modifier Convert()
		{
			var result = new ProcGen.Noise.Modifier
			{
				name = Name,
				pos = Pos.ToVector2f(),
				modifyType = ModifyType,
				invert = Invert,
				scale2d = Scale2d.ToVector2f()
			};

			result.lower = Lower.CalculateOrDefault(result.lower);
			result.upper = Upper.CalculateOrDefault(result.upper);
			result.exponent = Exponent.CalculateOrDefault(result.exponent);
			result.scale = Scale.CalculateOrDefault(result.scale);
			result.bias = Bias.CalculateOrDefault(result.bias);

			return result;
		}
	}
}
