using HarmonyLib;
using SpookyPumpkinSO.Content.GhostPip.Spawning;
using UnityEngine;

namespace SpookyPumpkinSO.Patches
{
	public class ExobaseHeadquartersConfigPatch
	{

		[HarmonyPatch(typeof(ExobaseHeadquartersConfig), "ConfigureBuildingTemplate")]
		public static class ExobaseHeadquartersConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				go.AddComponent<GhostPipSpawner>();
			}
		}
	}
}
