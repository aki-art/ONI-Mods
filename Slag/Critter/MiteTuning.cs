using System;
using System.Collections.Generic;
using TUNING;

namespace Slag.Critter
{
    class MiteTuning
	{
		public static float STANDARD_CALORIES_PER_CYCLE = 700000f;
		public static float STANDARD_STARVE_CYCLES = 10f;
		public static float STANDARD_STOMACH_SIZE = MiteTuning.STANDARD_CALORIES_PER_CYCLE * MiteTuning.STANDARD_STARVE_CYCLES;
		public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;
		public static float EGG_MASS = 2f;

		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
		{
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteEgg".ToTag(),
				weight = 1f
			},
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteHardEgg".ToTag(),
				weight = 0.02f
			},
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteVeggieEgg".ToTag(),
				weight = 0.02f
			}
		};

		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HARD = new List<FertilityMonitor.BreedingChance>
		{
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteEgg".ToTag(),
				weight = 0.32f
			},
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteHardEgg".ToTag(),
				weight = 0.65f
			},
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteMetalEgg".ToTag(),
				weight = 0.02f
			}
		};

		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_VEGGIE = new List<FertilityMonitor.BreedingChance>
		{
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteEgg".ToTag(),
				weight = 0.33f
			},
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteVeggieEgg".ToTag(),
				weight = 0.67f
			}
		};

		public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_METAL = new List<FertilityMonitor.BreedingChance>
		{
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteEgg".ToTag(),
				weight = 0.11f
			},
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteHardEgg".ToTag(),
				weight = 0.22f
			},
			new FertilityMonitor.BreedingChance
			{
				egg = "MiteMetalEgg".ToTag(),
				weight = 0.67f
			}
		};
	}
}
