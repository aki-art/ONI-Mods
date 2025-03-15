using GravitasBigStorage.Content;
using HarmonyLib;
using UnityEngine;

namespace GravitasBigStorage.Patches
{
	public class LonelyMinionHouseConfigPatch
	{
		[HarmonyPatch(typeof(LonelyMinionHouseConfig), "ConfigureBuildingTemplate")]
		public class LonelyMinionHouseConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				var def = go.AddComponent<GBS_UnlockableTechs>();
				def.unlockTechId = GravitasBigStorageConfig.ID;
			}
		}
	}
}
