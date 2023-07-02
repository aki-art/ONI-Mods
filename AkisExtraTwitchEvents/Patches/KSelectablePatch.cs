using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	internal class KSelectablePatch
	{
		[HarmonyPatch(typeof(KSelectable), "ApplyHighlight")]
		public class KSelectable_ApplyHighlight_Patch
		{
			public static void Postfix(KSelectable __instance, float highlight)
			{
				MidasFx.TryApplyHighlight(__instance.gameObject, highlight);
			}
		}

		[HarmonyPatch(typeof(KSelectable), "ClearHighlight")]
		public class KSelectable_ClearHighlight_Patch
		{
			public static void Postfix(KSelectable __instance)
			{
				MidasFx.TryApplyHighlight(__instance.gameObject, 0);
			}
		}
	}
}
