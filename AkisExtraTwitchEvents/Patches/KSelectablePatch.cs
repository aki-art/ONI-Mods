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
				HighlightFx.TryApplyHighlight(__instance.gameObject, highlight);
				__instance.Trigger(ModEvents.OnHighlightApplied);
			}
		}

		[HarmonyPatch(typeof(KSelectable), "ClearHighlight")]
		public class KSelectable_ClearHighlight_Patch
		{
			public static void Postfix(KSelectable __instance)
			{
				HighlightFx.TryApplyHighlight(__instance.gameObject, 0);
				__instance.Trigger(ModEvents.OnHighlightCleared);
			}
		}
	}
}
