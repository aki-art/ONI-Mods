extern alias YamlDotNetButNew;
using Moonlet.Utils;
using System;
using TemplateClasses;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.SubTemplates
{
	public class MTemplateCell : ShadowTypeBase<Cell>
	{
		public string Element { get; set; }

		public FloatNumber Mass { get; set; }

		public FloatNumber Temperature { get; set; }

		public string DiseaseName { get; set; }

		public IntNumber DiseaseCount { get; set; }

		[YamlMember(Alias = "location_x", ApplyNamingConventions = false)] // Klei inconsitent name
		public IntNumber LocationX { get; set; }

		[YamlMember(Alias = "location_y", ApplyNamingConventions = false)] // Klei inconsitent name
		public IntNumber LocationY { get; set; }

		public bool PreventFoWReveal { get; set; }

		public override Cell Convert(Action<string> log = null)
		{
			var element = ElementLoader.FindElementByName(Element);

			if (element == null)
			{
				log($"Invalid element: {Element}");
				return null;
			}

			return new Cell(
				LocationX.CalculateOrDefault(0),
				LocationY.CalculateOrDefault(0),
				element == null ? SimHashes.Vacuum : element.id,
				Temperature.CalculateOrDefault(300),
				Mass.CalculateOrDefault(100),
				DiseaseName,
				DiseaseCount.CalculateOrDefault(0),
				PreventFoWReveal);
		}
	}
}
