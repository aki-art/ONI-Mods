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
			public static void Postfix(Immigration __instance)
			{
                Traverse traverse = Traverse.Create(__instance).Field("carePackages");
                List<CarePackageInfo> list = traverse.GetValue() as List<CarePackageInfo>;

                list.Add(new(PumpkinPlantConfig.SEED_ID, 3f, () => DiscoveredResources.Instance.IsDiscovered(PumpkinPlantConfig.SEED_ID)));
                list.Add(new(PumpkinPieConfig.ID, 2f, () => DiscoveredResources.Instance.IsDiscovered(PumpkinPlantConfig.SEED_ID)));
				list.Add(new(ToastedPumpkinSeedConfig.ID, 12f, () => DiscoveredResources.Instance.IsDiscovered(PumpkinPlantConfig.SEED_ID)));
				list.Add(new(PumpkinConfig.ID, 6f, () => DiscoveredResources.Instance.IsDiscovered(PumpkinPlantConfig.SEED_ID)));

                traverse.SetValue(list);
            }
		}
	}
}
