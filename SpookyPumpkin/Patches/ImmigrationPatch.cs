using HarmonyLib;
using SpookyPumpkinSO.Content.Foods;
using SpookyPumpkinSO.Content.Plants;

namespace SpookyPumpkinSO.Patches
{
	public class ImmigrationPatch
	{
		[HarmonyPatch(typeof(Immigration), "ConfigureCarePackages")]
		public static class Immigration_ConfigureCarePackages_Patch
		{
			public static void Postfix(Immigration __instance)
			{
				__instance.carePackages.Add(new(PumpkinPlantConfig.SEED_ID, 3f, null));
				__instance.carePackages.Add(new(PumpkinPieConfig.ID, 2f, null));
				__instance.carePackages.Add(new(ToastedPumpkinSeedConfig.ID, 12f, null));
				__instance.carePackages.Add(new(PumpkinConfig.ID, 5f, null));
			}
		}
	}
}
