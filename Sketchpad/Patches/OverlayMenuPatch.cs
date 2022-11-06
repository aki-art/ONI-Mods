using HarmonyLib;
using SketchPad;
using System.Collections.Generic;

namespace Sketchpad.Patches
{
    public class OverlayMenuPatch
    {
        // Adds overlay toggle buttons
        [HarmonyPatch(typeof(OverlayMenu), "InitializeToggles")]
        public static class OverlayMenu_InitializeToggles_Patch
        {
            public static void Postfix(ref List<KIconToggleMenu.ToggleInfo> ___overlayToggleInfos)
            {
                var OverlayToggleInfo = AccessTools.Inner(typeof(OverlayMenu), "OverlayToggleInfo");

                System.Reflection.ConstructorInfo OverlayToggleInfoConstructor = OverlayToggleInfo.GetConstructor(
                    new [] {
                        typeof(string),
                        typeof(string),
                        typeof(HashedString),
                        typeof(string),
                        typeof(Action),
                        typeof(string),
                        typeof(string)
                    });

                object[] args = new object[] {
                        "Toggle Humidity Overlay",
                        "priority_1",
                        SketchOverlayMode.ID,
                        "",
                        Action.NumActions,
                        "Symmetry overlay",
                        "Symmetry check" };

                object newToggle = OverlayToggleInfoConstructor.Invoke(args);
                ___overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)newToggle);
            }
        }
    }
}
