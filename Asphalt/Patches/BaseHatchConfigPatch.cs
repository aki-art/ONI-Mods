using HarmonyLib;
using System.Collections.Generic;

namespace Asphalt.Patches
{
	public class BaseHatchConfigPatch
	{
		// Allow hatches to eat bitumen
		[HarmonyPatch(typeof(BaseHatchConfig), nameof(BaseHatchConfig.BasicRockDiet))]
		public static class HatchConfig_BasicRockDiet_Patch
		{
			public static void Postfix(List<Diet.Info> __result)
			{
				__result[0].consumedTags.Add(SimHashes.Bitumen.CreateTag());
			}
		}
	}
}
