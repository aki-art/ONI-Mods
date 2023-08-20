using FUtility;
using HarmonyLib;
using Klei.AI;
using System;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class ModifierSetPatch
	{
		[HarmonyPatch(typeof(ModifierSet), "Initialize")]
		public class ModifierSet_Initialize_Patch
		{
			public static void Postfix(ModifierSet __instance)
			{
				TEffects.Register(__instance);
			}
		}
	}
}
