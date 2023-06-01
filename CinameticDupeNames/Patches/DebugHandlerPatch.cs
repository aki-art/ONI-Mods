using CinematicDupeNames.Content.Cmps;
using HarmonyLib;
namespace CinematicDupeNames.Patches
{
    public class DebugHandlerPatch
    {
        [HarmonyPatch(typeof(DebugHandler), "ToggleScreenshotMode")]
        public class DebugHandler_ToggleScreenshotMode_Patch
        {
            public static void Postfix() => CDN_CinematicModeHandler.Instance.UpdateNames();
        }
    }
}
