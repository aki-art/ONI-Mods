using Moonlet.Entities.Commands;
using Moonlet.Entities.ComponentTypes;
using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class GeyserData
	{
		public string Id { get; set; }

		public string Animation { get; set; }

		public string Element { get; set; }

		public bool Generic { get; set; }

		public int Width { get; set; } = 3;

		public int Height { get; set; } = 4;

		public string TintFx { get; set; }

		public string[] TintSymbols { get; set; }

		public string Type { get; set; }

		public float TemperatureCelsius { get; set; } = 90f;

		public float MinRatePerCycle { get; set; } = 2000f;

		public float MaxRatePerCycle { get; set; } = 4000f;

		public float MaxPressure { get; set; } = 500f;

		public float MinIterationLength { get; set; } = 60f;

		public float MaxIterationLength { get; set; } = 1140f;

		public float MinIterationPercent { get; set; } = 0.1f;

		public float MaxIterationPercent { get; set; } = 0.9f;

		public float MinYearLength { get; set; } = 15000f;

		public float MaxYearLength { get; set; } = 135000f;

		public float MinYearPercent { get; set; } = 0.4f;

		public float MaxYearPercent { get; set; } = 0.8f;

		public float GeyserTemperature { get; set; } = 372.15f;

		public string Disease { get; set; }

		public float DiseaseCount { get; set; }

		public string DlcId { get; set; }

		public string[] RequiredMods { get; set; }

		public GeyserData()
		{
			TintSymbols = new[]
			{
				"gasframes",
				"leak_ceiling"
			};
		}
	}
}
