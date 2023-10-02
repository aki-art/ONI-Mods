using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;
using UnityEngine;

namespace SpookyPumpkinSO.Patches
{
	public class LuxuryBedConfigPatch
	{
		[HarmonyPatch(typeof(LuxuryBedConfig), "ConfigureBuildingTemplate")]
		public class LuxuryBedConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				go.AddOrGet<SpookyPumpkin_FacadeRestorer>();
			}
		}
	}
}
