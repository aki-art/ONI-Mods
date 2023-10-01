using HarmonyLib;
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
				//var restorer = go.AddOrGet<SpookyPumpkin_FacadeRestorer>();
				//restorer.playAnimCb = _ => "off";
			}
		}
	}
}
