using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CenterOverlay
{
    class OverlayPatches
    {
        [HarmonyPatch(typeof(SimDebugView), "OnPrefabInit")]
        public static class SimDebugView_OnPrefabInit_Patch
        {
            public static void Postfix(Dictionary<HashedString, Func<SimDebugView, int, Color>> ___getColourFuncs)
            {
                ___getColourFuncs.Add(
                    MirrorSide.ID,
                    new Func<SimDebugView, int, Color>(GetMirrorColor));
            }

            private static Color GetMirrorColor(SimDebugView instance, int cell)
            {
                var xLimit = Grid.WidthInCells / 2 + ModAssets.Settings.Offset;
                if (Grid.WidthInCells % 2 == 1)
                    xLimit -= 1;

                var result = Grid.CellToXY(cell).x > xLimit ?
                    Color.green :
                    Color.red;

                return result;
            }
        }

        [HarmonyPatch(typeof(OverlayMenu), "InitializeToggles")]
        public static class OverlayMenu_InitializeToggles_Patch
        {
            public static void Postfix(ref List<KIconToggleMenu.ToggleInfo> ___overlayToggleInfos)
            {
                Type OverlayToggleInfo = AccessTools.Inner(typeof(OverlayMenu), "OverlayToggleInfo");

                var OverlayToggleInfoConstructor = OverlayToggleInfo.GetConstructor(
                    new Type[] {
                        typeof(string),
                        typeof(string),
                        typeof(HashedString),
                        typeof(string),
                        typeof(Action),
                        typeof(string),
                        typeof(string) });

                object[] args = new object[] {
                        "Toggle Symmetry Check",
                        "overlay_conveyor",
                        MirrorSide.ID,
                        "",
                        Action.NumActions,
                        "Symmetry overlay",
                        "Symmetry check" };

                object newToggle = OverlayToggleInfoConstructor.Invoke(args);
                ___overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)newToggle);
            }
        }

        [HarmonyPatch(typeof(OverlayScreen), "RegisterModes")]
        public static class OverlayScreen_RegisterModes_Patch
        {
            public static void Prefix()
            {
                Traverse overlayScreen = Traverse.Create(OverlayScreen.Instance);
                overlayScreen.Method("RegisterMode", new MirrorSide()).GetValue();
            }
        }
    }
}
