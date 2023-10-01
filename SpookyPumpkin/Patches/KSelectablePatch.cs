using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;

namespace SpookyPumpkinSO.Patches
{
	public class KSelectablePatch
	{
		[HarmonyPatch(typeof(KSelectable), "ApplyHighlight")]
		public class KSelectable_ApplyHighlight_Patch
		{
			public static void Postfix(KSelectable __instance, float highlight)
			{
				Ghastly.TryApplyHighlight(__instance.gameObject, highlight);
			}
		}

		[HarmonyPatch(typeof(KSelectable), "ClearHighlight")]
		public class KSelectable_ClearHighlight_Patch
		{
			public static void Postfix(KSelectable __instance)
			{
				Ghastly.TryApplyHighlight(__instance.gameObject, 0);
			}
		}
	}
}
