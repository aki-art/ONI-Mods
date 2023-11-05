using ProcGen.Noise;
using System;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class CombinerC : ShadowTypeBase<Combiner>, INoiseBase
	{
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }

		public CombinerC()
		{
			Pos = new(0, 0);
		}

		public override Combiner Convert(Action<string> log)
		{
			return new Combiner()
			{
				name = Name,
				pos = Pos.ToVector2f()
			};
		}
	}
}
