using HarmonyLib;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class WorldgenSettingsPatch
	{
		[HarmonyPatch(typeof(WorldGenSettings), MethodType.Constructor, [
			typeof(WorldPlacement),
			typeof(int),
			typeof(List<string>),
			typeof(List<string>),
			typeof(bool),
		])]
		public class WorldGenSettings_Ctor_Patch
		{
			public static void Prefix(WorldPlacement placement, int seed, List<string> worldTraits)
			{
				SwapTraits(placement.world, worldTraits);
			}
		}

		[HarmonyPatch(typeof(WorldGenSettings), MethodType.Constructor, [
			typeof(string),
			typeof(List<string>),
			typeof(List<string>),
			typeof(bool),
		])]
		public class WorldGenSettings_Ctor_Patch2
		{
			public static void Prefix(string worldName, List<string> worldTraits)
			{
				SwapTraits(worldName, worldTraits);
			}
		}

		private static void SwapTraits(string worldId, List<string> worldTraits)
		{
			foreach (var swap in Mod.traitSwaps)
			{
				if (swap.worldId == worldId)
				{
					if (worldTraits.Contains(swap.originalTrait))
					{
						worldTraits.Remove(swap.originalTrait);
						worldTraits.Add(swap.replacementTrait);
					}
				}
			}
		}
	}
}
