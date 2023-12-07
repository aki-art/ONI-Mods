using HarmonyLib;
using Twitchery.Content;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class GraveConfigPatch
	{
		[HarmonyPatch(typeof(GraveConfig), "ConfigureBuildingTemplate")]
		public class GraveConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(GameObject go)
			{
				go.AddOrGet<MinionStorage>();
				go.AddOrGet<AETE_GraveStoneMinionStorage>();
				go.AddTag(TTags.hideDeadDupesWithin);
			}
		}
	}
}
