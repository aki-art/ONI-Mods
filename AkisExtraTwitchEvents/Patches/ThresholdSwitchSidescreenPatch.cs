using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class ThresholdSwitchSidescreenPatch
	{
		// TODO: this is unfriendly to other mods trying to do something similar
		[HarmonyPatch(typeof(ThresholdSwitchSideScreen), "SetTarget")]
		public class ThresholdSwitchSideScreen_SetTarget_Patch
		{
			public static void Postfix(ThresholdSwitchSideScreen __instance)
			{
				if (__instance.target == null)
					return;

				var enableControls = !__instance.target.HasTag(TTags.disableUserScreen);

				__instance.numberInput.inputField.interactable = enableControls;
				__instance.aboveToggle.interactable = enableControls;
				__instance.belowToggle.interactable = enableControls;
				__instance.thresholdSlider.interactable = enableControls;
			}
		}

		[HarmonyPatch(typeof(ThresholdSwitchSideScreen), "UpdateThresholdValue")]
		public class ThresholdSwitchSideScreen_UpdateThresholdValue_Patch
		{
			public static bool Prefix(ThresholdSwitchSideScreen __instance)
			{
				if (__instance.target == null)
					return true;

				return !__instance.target.HasTag(TTags.disableUserScreen);
			}
		}
	}
}
