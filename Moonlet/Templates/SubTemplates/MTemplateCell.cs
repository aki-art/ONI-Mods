using Moonlet.Utils;
using System;
using TemplateClasses;

namespace Moonlet.Templates.SubTemplates
{
	public class MTemplateCell : ShadowTypeBase<Cell>
	{
		public string Element { get; set; }

		public FloatNumber Mass { get; set; }

		public FloatNumber Temperature { get; set; }

		public string DiseaseName { get; set; }

		public IntNumber DiseaseCount { get; set; }

		public IntNumber Location_x { get; set; }

		public IntNumber Location_y { get; set; }

		public bool PreventFoWReveal { get; set; }

		public override Cell Convert(Action<string> log = null)
		{
			var element = ElementLoader.FindElementByName(Element);

			return new Cell(
				Location_x.CalculateOrDefault(0),
				Location_y.CalculateOrDefault(0),
				element == null ? SimHashes.Vacuum : element.id,
				Temperature.CalculateOrDefault(300),
				Mass.CalculateOrDefault(100),
				DiseaseName,
				DiseaseCount.CalculateOrDefault(0),
				PreventFoWReveal);
		}
	}
}
