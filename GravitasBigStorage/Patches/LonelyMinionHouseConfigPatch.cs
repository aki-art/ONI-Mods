using GravitasBigStorage.Content;
using HarmonyLib;
using UnityEngine;

namespace GravitasBigStorage.Patches
{
	internal class LonelyMinionHouseConfigPatch
	{

		[HarmonyPatch(typeof(LonelyMinionHouseConfig), "ConfigureBuildingTemplate")]
		public class LonelyMinionHouseConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				go.AddOrGet<Analyzable>();

				go.AddOrGet<POITechItemUnlockWorkable>().workTime = 5f;
				var def = go.AddOrGetDef<POITechItemUnlocks.Def>();
				def.POITechUnlockIDs = [GravitasBigStorageConfig.ID];
				def.PopUpName = "Test";
				def.animName = "gravitasbigstorage_unlock_screen_kanim";
			}
		}
	}
}
