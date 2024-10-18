using Moonlet.Templates.EntityTemplates;
using System;
using TUNING;

namespace Moonlet.Templates
{
	public class BuildingTemplate : EntityTemplate
	{
		public string Category { get; set; }

		public string[] DlcIds { get; set; }

		public string SubCategory { get; set; }

		public string After { get; set; }

		public string Before { get; set; }

		public string ResearchCategory { get; set; }

		public bool IsAfterNeighbor { get; set; }

		public bool Prioritizable { get; set; }

		public bool DisallowBuildingByPlayer { get; set; }

		public MaterialData[] Materials { get; set; }

		public float ExhaustKilowattsWhenActive { get; set; }

		public float SelfHeatKilowattsWhenActive { get; set; }

		public float EnergyConsumptionWhenActive { get; set; }

		public int HitPoints { get; set; }

		public string OverlayMode { get; set; }

		public float ConstructionTime { get; set; }

		public float MeltingPointKelvin { get; set; }

		public bool RequiresFoundation { get; set; }

		public PowerSocketData PowerOutlet { get; set; }

		public PowerSocketData PowerInlet { get; set; }

		public ConduitData ConduitIn { get; set; }

		public ConduitData ConduitOut { get; set; }

		public GeneratorData Generator { get; set; }

		public BuildLocationRule BuildLocationRule { get; set; }

		public float? PowerConsumption { get; set; }

		public string AudioCategory { get; set; }

		public string AudioSize { get; set; }

		public BuildingTemplate() : base()
		{
			ConstructionTime = TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1;
			MeltingPointKelvin = TUNING.BUILDINGS.MELTING_POINT_KELVIN.TIER1;
			HitPoints = TUNING.BUILDINGS.HITPOINTS.TIER2;
			SubCategory = "uncategorized";
			BuildLocationRule = BuildLocationRule.Anywhere;
			RequiresFoundation = true;
			DlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
			AudioCategory = AUDIO.CATEGORY.METAL;
			AudioSize = AUDIO.SIZE.MEDIUM;
			DisallowBuildingByPlayer = false;
		}

		[Serializable]
		public class MaterialData
		{
			public string Material { get; set; }

			public float Mass { get; set; }
		}

		[Serializable]
		public class PowerSocketData
		{
			public int X { get; set; }

			public int Y { get; set; }

			public bool Required { get; set; }
		}

		[Serializable]
		public class ConduitData
		{
			public int X { get; set; }

			public int Y { get; set; }

			public ConduitType Type { get; set; }
		}

		[Serializable]
		public class GeneratorData
		{
			public float Wattage { get; set; }

			public float BaseCapacity { get; set; }
		}
	}
}
