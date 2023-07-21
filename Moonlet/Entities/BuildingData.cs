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

		public int HitPoints { get; set; }

		public float ConstructionTime { get; set; }

		public float MeltingPointKelvin { get; set; }

		public bool RequiresFoundation { get; set; }

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
	}
}
