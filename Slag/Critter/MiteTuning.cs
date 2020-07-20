using System;
using System.Collections.Generic;
using TUNING;
using Utils;

namespace Slag.Critter
{
	public class MiteTuning
	{
		public static float STANDARD_CALORIES_PER_CYCLE = 700000f;
		public static float STANDARD_STARVE_CYCLES = 10f;
		public static float STANDARD_STOMACH_SIZE = STANDARD_CALORIES_PER_CYCLE * STANDARD_STARVE_CYCLES;
		public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;
		public static float EGG_MASS = 2f;

		public static Dictionary<string, Dictionary<string, Dictionary<string, float>>> moltWeights;

		public class Rewards
		{ 
			public class Gleamite
			{
				public static List<WeightedMetalOption> worthless = GetMetalRewards(MoltType.mysteryMetal, MoltTier.worthless);
				public static List<WeightedMetalOption> lackluster = GetMetalRewards(MoltType.mysteryMetal, MoltTier.lackluster);
				public static List<WeightedMetalOption> mediocre = GetMetalRewards(MoltType.mysteryMetal, MoltTier.mediocre);
				public static List<WeightedMetalOption> exquisite = GetMetalRewards(MoltType.mysteryMetal, MoltTier.exquisite);
			}
			public class Slagmite
			{
				public static List<WeightedMetalOption> worthless = GetMetalRewards(MoltType.mysteryOre, MoltTier.worthless);
				public static List<WeightedMetalOption> lackluster = GetMetalRewards(MoltType.mysteryOre, MoltTier.lackluster);
				public static List<WeightedMetalOption> mediocre = GetMetalRewards(MoltType.mysteryOre, MoltTier.mediocre);
				public static List<WeightedMetalOption> exquisite = GetMetalRewards(MoltType.mysteryOre, MoltTier.exquisite);
			}
		}

		public enum MoltTier
		{
			worthless,
			lackluster,
			mediocre,
			exquisite
		}

		public enum MoltType
		{
			mysteryMetal,
			mysteryOre
		}


		public static List<WeightedMetalOption> GetMetalRewards(MoltType type, MoltTier tier)
		{
			if (moltWeights == null)
			{
				Log.Warning("Molt weights have not been assigned yet!");
				return new List<WeightedMetalOption>();
			}

			var moltType = type.ToString();
			var moltTier = tier.ToString();

			var options = new List<WeightedMetalOption>();
			var mysteryMetals = moltWeights[moltType][moltTier];

			foreach (var kvp in mysteryMetals)
			{
				if (Enum.TryParse(kvp.Key, out SimHashes simHash))
				{
					options.Add(new WeightedMetalOption(simHash, kvp.Value));
				}
			}

			if (options.Count == 0)
			{
				Log.Warning("Molt metal rewards could not be loaded. Was the .json incorrectly edited?");
			}
			return options;
		}
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
			}
		};

	}
}
