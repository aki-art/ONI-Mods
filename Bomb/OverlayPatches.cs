using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb
{
    class OverlayPatches
    {
        [HarmonyPatch(typeof(OverlayScreen), "RegisterModes")]
        public static class OverlayScreen_RegisterModes_Patch
        {
            public static void Postfix()
            {
                Traverse overlayScreen = Traverse.Create(OverlayScreen.Instance);
                overlayScreen.Method("RegisterMode", new DebugOcclusionOverlayMode()).GetValue();
            }
        }

        // Colors cells based on seismic activity
        [HarmonyPatch(typeof(SimDebugView), "OnPrefabInit")]
        public static class SimDebugView_OnPrefabInit_Patch
        {
            public static void Postfix(Dictionary<HashedString, Func<SimDebugView, int, Color>> ___getColourFuncs)
            {
                ___getColourFuncs.Add(DebugOcclusionOverlayMode.BombMode.ID, GetColor);
            }

            private static Color GetColor(SimDebugView instance, int cell)
            {
                if(ExplosionRayCaster.occlusionMap != null && ExplosionRayCaster.occlusionMap.TryGetValue(cell, out ExplosionRayCaster.Cell c))
                {
                    return Color.Lerp(Color.black, Color.green, c.occlusion);
                }

                return Color.black;
            }
        }

        // Adds overlay toggle buttons
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
                        typeof(string)
                    });

                object[] args = new object[] {
                        "Toggle Occlusion overlay",
                        "overlay_sound",
                        DebugOcclusionOverlayMode.BombMode.ID,
                        "",
                        Action.NumActions,
                        "Occlusion overlay",
                        "occlusion" };

                object newToggle = OverlayToggleInfoConstructor.Invoke(args);
                ___overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)newToggle);
            }
        }
    }
}
