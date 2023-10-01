using HarmonyLib;
using SpookyPumpkinSO.Content.GhostPip.Spawning;
using UnityEngine;

namespace SpookyPumpkinSO.Patches
{
	internal class TelepadPatch
	{
		[HarmonyPatch(typeof(HeadquartersConfig), "ConfigureBuildingTemplate")]
		public static class HeadquartersConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				go.AddComponent<GhostPipSpawner>();
			}
		}
	}
}
