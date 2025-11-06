using ProcGen;
using System.Collections.Generic;
using Twitchery.Content;

namespace Twitchery.Utils
{
	public class MiscUtil
	{
		public static List<WeighedElementOption> dangerousLiquids =
		[
			new WeighedElementOption(SimHashes.MoltenAluminum),
				new WeighedElementOption(SimHashes.MoltenIron),
				new WeighedElementOption(SimHashes.MoltenGlass),
				new WeighedElementOption(SimHashes.MoltenGold),
				new WeighedElementOption(SimHashes.MoltenLead),
				new WeighedElementOption(SimHashes.MoltenUranium),
				new WeighedElementOption(SimHashes.NuclearWaste, 600.0f),
				new WeighedElementOption(SimHashes.Steam, 400.0f),
				new WeighedElementOption(SimHashes.RockGas),
				new WeighedElementOption(Elements.PinkSlime, mass: 10000),
				new WeighedElementOption("ITCE_CreepyLiquid"),
				new WeighedElementOption("ITCE_Inverse_Water", mass: 4000),
				new WeighedElementOption("Beached_SulfurousWater")
		];

		public static void PostElementsLoaded()
		{
			dangerousLiquids.RemoveAll(o => ElementLoader.FindElementByTag(o.id) == null);
		}

		public struct WeighedElementOption(string id, float temperature = -1, float mass = -1, float weight = 1.0f) : IWeighted
		{

			public float weight { get; set; } = weight;
			public string id = id;
			public float temperature = temperature;
			public float mass = mass;
			public int count = 1;

			public WeighedElementOption(SimHashes id, float temperature = -1, float mass = -1, float weight = 1.0f) : this(id.ToString(), temperature, mass, weight)
			{
			}

			public static WeighedElementOption Prefab(string id, int count = 1, float temperature = -1, float mass = -1, float weight = 1.0f)
			{
				var result = new WeighedElementOption(id, temperature, mass, weight)
				{
					count = count
				};

				return result;
			}
		}
	}
}
