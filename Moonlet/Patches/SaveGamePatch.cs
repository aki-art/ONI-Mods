﻿using HarmonyLib;
using Moonlet.Scripts;

namespace Moonlet.Patches
{
	internal class SaveGamePatch
	{

		[HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
		public class SaveGame_OnPrefabInit_Patch
		{
			public static void Postfix(SaveGame __instance)
			{
				__instance.gameObject.AddOrGet<Moonlet_Mod>();
			}
		}
	}
}