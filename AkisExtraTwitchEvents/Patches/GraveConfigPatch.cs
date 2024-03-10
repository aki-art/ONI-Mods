﻿using HarmonyLib;
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

				var serializer = go.AddOrGet<AETE_GameObjectSerializer>();
				serializer.showInUIs = false;
				serializer.checkForSafeLocation = true;
				serializer.saveToFile = true;

				go.AddTag(TTags.hideDeadDupesWithin);
			}
		}
	}
}
