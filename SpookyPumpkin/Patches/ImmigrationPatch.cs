using HarmonyLib;
using SpookyPumpkinSO.Content.Foods;
using SpookyPumpkinSO.Content.Plants;
using System.Collections.Generic;

namespace SpookyPumpkinSO.Patches
{
	public class ImmigrationPatch
	{
		[HarmonyPatch(typeof(Immigration), "ConfigureCarePackages")]
		public static class Immigration_ConfigureCarePackages_Patch
		{
			public static void Postfix(ref CarePackageInfo[] ___carePackages)
			{
				var extraPackages = new List<CarePackageInfo>(___carePackages)
				{
					new CarePackageInfo(PumpkinPlantConfig.SEED_ID, 3f, null),
					new CarePackageInfo(PumpkinPieConfig.ID, 2f, null),
					new CarePackageInfo(ToastedPumpkinSeedConfig.ID, 12f, null),
					new CarePackageInfo(PumpkinConfig.ID, 5f, null)
				};

				___carePackages = extraPackages.ToArray();
			}
		}
	}
}
