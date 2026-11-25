using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;
using System;
using UnityEngine;

namespace SpookyPumpkinSO.Patches
{
	public class KSelectablePatch
	{
		[HarmonyPatch(typeof(KSelectable), "ApplyHighlight", [typeof(Color)])]
		public class KSelectable_ApplyHighlight_Patch
		{
			public static void Postfix(KSelectable __instance, Color color)
			{
				Ghastly.TryApplyHighlight(__instance.gameObject, color);
			}
		}

		[HarmonyPatch(typeof(KSelectable), "ClearHighlight")]
		public class KSelectable_ClearHighlight_Patch
		{
			public static void Postfix(KSelectable __instance)
			{
				Ghastly.TryApplyHighlight(__instance.gameObject, Color.black);
			}
		}
	}
}
