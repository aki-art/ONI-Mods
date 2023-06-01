using CinematicDupeNames.Content;
using CinematicDupeNames.Content.Cmps;
using HarmonyLib;

namespace CinematicDupeNames.Patches
{
    public class PlayerControllerPatch
    {

        [HarmonyPatch(typeof(PlayerController), "OnKeyDown")]
        public class PlayerController_OnKeyDown_Patch
        {
            public static void Prefix(KButtonEvent e)
            {
                if (e.TryConsume(CDNActions.ToggleNamesAction.GetKAction()))
                    CDN_CinematicModeHandler.Instance.ToggleNames();
            }
        }
    }
}
