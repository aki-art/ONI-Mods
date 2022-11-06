using HarmonyLib;

namespace SketchPad.Patches
{
    public class OverlayScreenPatch
    {
        [HarmonyPatch(typeof(OverlayScreen), "RegisterModes")]
        public static class OverlayScreen_RegisterModes_Patch
        {
            public static void Postfix()
            {
                var overlayScreen = Traverse.Create(OverlayScreen.Instance);
                overlayScreen.Method("RegisterMode", new SketchOverlayMode()).GetValue();
            }
        }
    }
}
