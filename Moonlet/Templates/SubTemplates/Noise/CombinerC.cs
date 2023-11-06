using HarmonyLib;
using ProcGen.Noise;
using System;

namespace Moonlet.Templates.SubTemplates.Noise
{
	public class CombinerC : ShadowTypeBase<Combiner>, INoiseBase
	{
		public string Name { get; set; }
		public Vector2FC Pos { get; set; }

		public string CombineType { get; set; }

		public CombinerC()
		{
			Pos = new(0, 0);
			CombineType = Combiner.CombinerType.Add.ToString();
		}

		public override Combiner Convert(Action<string> log)
		{
			if (!Enum.TryParse(CombineType, out Combiner.CombinerType combinerType))
				log($"{CombineType} is not a valid option of a Combiner entry. Available are: {Enum.GetNames(typeof(Combiner.CombinerType)).Join()}");

			return new Combiner()
			{
				name = Name,
				pos = Pos.ToVector2f(),
				combineType = combinerType
			};
		}
	}
}
