namespace Moonlet.Templates.EntityTemplates
{
	public abstract class BasePlantTemplate : EntityTemplate
	{
		public string CropId { get; set; }

		public string PlanterDirection { get; set; } = SingleEntityReceptacle.ReceptacleDirection.Top.ToString();

		public float TemperatureLethalLow { get; set; }

		public float TemperatureLethalHigh { get; set; }

		public DecorEntry DecorAlive { get; set; }

		public DecorEntry DecorWilted { get; set; }

		public float TemperatureWarningLow { get; set; }

		public float TemperatureWarningHigh { get; set; }

		public bool PressureSensitive { get; set; } = true;

		public float PressureLethalLow { get; set; }

		public float PressureLethalHigh { get; set; }

		public float PressureWarningLow { get; set; }

		public float PressureWarningHigh { get; set; }

		public float RadiationLethalLow { get; set; }

		public float RadiationLethalHigh { get; set; }

		public string[] SafeElements { get; set; }

		public bool CanDrown { get; set; }

		public bool CanTinker { get; set; }

		public bool RequiresSolidTile { get; set; }

		public bool ShouldGrowOld { get; set; }

		public float MaxAge { get; set; }

		public SeedInfo Seed { get; set; }

		public class SeedInfo
		{
			public int Count { get; set; }

			public string Id { get; set; }

			public int SortOrder { get; set; }

			public SeedInfo()
			{
				Count = 1;
			}
		}

		public BasePlantTemplate() : base()
		{
			Element = SimHashes.Creature.ToString();

			TemperatureLethalLow = 218.15f;
			TemperatureLethalHigh = 398.15f;

			TemperatureWarningLow = 283.15f;
			TemperatureWarningHigh = 303.15f;

			PressureWarningLow = 0.15f;
			PressureWarningHigh = float.PositiveInfinity;

			PressureLethalLow = 0;
			PressureLethalHigh = float.PositiveInfinity;

			RadiationLethalLow = 0;
			RadiationLethalHigh = 2200;

			Seed = new();

			MaxAge = 2200;

			ShouldGrowOld = true;
			CanDrown = true;
			CanTinker = true;
			RequiresSolidTile = true;

			Mass = 10.0f;
		}
	}
}
