using Moonlet.Utils;
using System;
using static ProcGen.Noise.Modifier;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class ModifierC : IShadowTypeBase<ProcGen.Noise.Modifier>, INoiseBase
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
			Scale2d = new Vector2FC(1, 1);
			Pos = new Vector2FC(0, 0);
		}

		public ProcGen.Noise.Modifier Convert(Action<string> log)
		{
			var result = new ProcGen.Noise.Modifier
			{
				name = Name,
				pos = Pos.ToVector2f(),
				modifyType = ModifyType,
				invert = Invert,
				scale2d = Scale2d.ToVector2f(),
				lower = Lower.CalculateOrDefault(-1),
				upper = Upper.CalculateOrDefault(1),
				exponent = Exponent.CalculateOrDefault(0.02f),
				scale = Scale.CalculateOrDefault(1f),
				bias = Bias.CalculateOrDefault(0f)
			};

			return result;
		}
	}
}
