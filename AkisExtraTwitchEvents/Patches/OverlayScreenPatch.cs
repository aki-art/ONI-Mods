using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class OverlayScreenPatch
	{
		[HarmonyPatch(typeof(OverlayScreen), "ToggleOverlay")]
		public class OverlayScreen_ToggleOverlay_Patch
		{
			public static void Prefix(HashedString newMode)
			{
				if (newMode == OverlayModes.Temperature.ID)
					AkisTwitchEvents.Instance.CreateOrEnableFireOverlay();
				else
					AkisTwitchEvents.Instance.HideFireOverlay();
			}
		}
	}
}
