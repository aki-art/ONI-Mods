using HarmonyLib;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class GasLiquidExposureMonitorPatch
	{
		[HarmonyPatch(typeof(GasLiquidExposureMonitor), nameof(GasLiquidExposureMonitor.InitializeCustomRates))]
		public class GasLiquidExposureMonitor_InitializeCustomRates_Patch
		{
			public static void Postfix(Dictionary<SimHashes, float> ___customExposureRates)
			{
				if (___customExposureRates != null)
					Mod.elementsLoader.SetExposureValues(___customExposureRates);
			}
		}
	}
}
