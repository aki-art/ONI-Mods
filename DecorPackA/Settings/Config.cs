using FUtility.SaveData;

namespace DecorPackA.Settings
{
	public class Config : IUserSetting
	{
		public GlassTilesConfig GlassTile { get; set; } = new GlassTilesConfig();

		public GlassSculpturesConfig GlassSculpture { get; set; } = new GlassSculpturesConfig();

		public MoodLampConfig MoodLamp { get; set; } = new MoodLampConfig();


		public class GlassSculpturesConfig
		{
			public RangedValue BaseDecor { get; set; } = new RangedValue()
			{
				Range = 8,
				Amount = 20
			};

			public int BadSculptureDecorBonus { get; set; } = 5;

			public int MediocreSculptureDecorBonus { get; set; } = 10;

			public int GeniousSculptureDecorBonus { get; set; } = 15;
		}

		public class MoodLampConfig
		{
			public bool VibrantColors { get; set; } = true;

			public bool DisableLightRays { get; set; } = false;

			public RangedValue Lux { get; set; } = new RangedValue()
			{
				Range = 3,
				Amount = 400
			};

			public RangedValue Decor { get; set; } = new RangedValue()
			{
				Range = 4,
				Amount = 25
			};

			public PowerConfig PowerUse = new()
			{
				ExhaustKilowattsWhenActive = .5f,
				EnergyConsumptionWhenActive = 6f,
				SelfHeatKilowattsWhenActive = 0f
			};
		}

		public class GlassTilesConfig
		{
			public bool UseDyeTC { get; set; } = true;

			public bool NerfAbyssalite { get; set; } = true;

			public bool DisableColorShiftEffect { get; set; } = false;

			public bool InsulateConstructionStorage { get; set; } = true;

			public float DyeRatio { get; set; } = 0.5f;

			public float SpeedBonus { get; set; } = 1.25f;

			public RangedValue Decor { get; set; } = new RangedValue()
			{
				Range = 2,
				Amount = 10
			};
		}

		public class RangedValue
		{
			public int Range { get; set; }

			public int Amount { get; set; }
		}

		public class PowerConfig
		{
			public float ExhaustKilowattsWhenActive { get; set; }

			public float EnergyConsumptionWhenActive { get; set; }

			public float SelfHeatKilowattsWhenActive { get; set; }
		}
	}
}
