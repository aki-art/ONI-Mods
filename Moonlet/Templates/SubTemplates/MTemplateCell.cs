extern alias YamlDotNetButNew;
using Moonlet.Utils;
using System;
using TemplateClasses;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.SubTemplates
{
	public class MTemplateCell : IShadowTypeBase<Cell>
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

		public Cell Convert(Action<string> log = null)
		{
			if (Element.IsNullOrWhiteSpace())
			{
				log("Element name is null");
				return null;
			}

			var elementId = ElementUtil.GetSimhashSafe(Element);
			if (elementId == SimHashes.Void)
			{
				log($"Invalid element Id: {Element}");
				return null;
			}

			return new Cell(
				LocationX.CalculateOrDefault(0),
				LocationY.CalculateOrDefault(0),
				elementId,
				Temperature.CalculateOrDefault(300),
				Mass.CalculateOrDefault(100),
				DiseaseName,
				DiseaseCount.CalculateOrDefault(0),
				PreventFoWReveal);
		}
	}
}
