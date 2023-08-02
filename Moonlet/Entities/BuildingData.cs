using System;

namespace Moonlet.Entities
{
	public class BuildingData : EntityData
	{
		public string Category { get; set; }

		public string SubCategory { get; set; }

		public string Neighbor { get; set; }

		public bool IsAfterNeighbor { get; set; }

		public MaterialData[] Materials { get; set; }

		public float ExhaustKilowattsWhenActive { get; set; }

		public float SelfHeatKilowattsWhenActive { get; set; }

		public int HitPoints { get; set; }

		public string OverlayMode { get; set; }

		public float ConstructionTime { get; set; }

		public float MeltingPointKelvin { get; set; }

		public bool RequiresFoundation { get; set; }

		public PowerData PowerOutlet {get; set; }

		public PowerData PowerInlet {get; set; }

		public ConduitData ConduitIn {get; set; }

		public ConduitData ConduitOut {get; set; }

		public GeneratorData Generator {get; set; }

		public BuildLocationRule BuildLocationRule { get; set; }
		
		public BuildingData()
		{
			ConstructionTime = TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1;
			MeltingPointKelvin = TUNING.BUILDINGS.MELTING_POINT_KELVIN.TIER1;
			HitPoints = TUNING.BUILDINGS.HITPOINTS.TIER2;
			SubCategory = "uncategorized";
			BuildLocationRule = BuildLocationRule.Anywhere;
			RequiresFoundation = true;
		}

		[Serializable]
		public class MaterialData
		{
			public string Material { get; set; }

			public float Mass { get; set; }
		}

		[Serializable]
		public class PowerData
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
