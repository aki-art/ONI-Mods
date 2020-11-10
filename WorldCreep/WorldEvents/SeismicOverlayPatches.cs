using FUtility.FUI;
using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    class SeismicOverlayPatches
    {
        // Regsiter Seismic overlay screen
        [HarmonyPatch(typeof(OverlayScreen), "RegisterModes")]
        public static class OverlayScreen_RegisterModes_Patch
        {
            public static void Postfix()
            {
                Traverse overlayScreen = Traverse.Create(OverlayScreen.Instance);
                overlayScreen.Method("RegisterMode", new SeismicOverlayMode()).GetValue();
            }
        }

        // Colors cells based on seismic activity
        [HarmonyPatch(typeof(SimDebugView), "OnPrefabInit")]
        public static class SimDebugView_OnPrefabInit_Patch
        {
            public static void Postfix(Dictionary<HashedString, Func<SimDebugView, int, Color>> ___getColourFuncs)
            {
                ___getColourFuncs.Add(SeismicOverlayMode.SeismicMode.ID, GetColor);
                ___getColourFuncs.Add(SeismicOverlayMode.EarthQuakeOverlayMode.ID, GetEpicenterColors);
#if DEBUG
                ___getColourFuncs.Add(SeismicOverlayMode.FaultChunksOverlayMode.ID, GetChunk);
#endif
            }

            private static Color GetColor(SimDebugView instance, int cell)
            {
                Color black = Color.black;
                Color blue = Color.blue;
                Color pink = Tuning.Colors.seismicOverlayActive;
                Color yellow = new Color(1, 1, 0);
                Color red = Color.red;

                float treshold = 0.5f;
                float value = SeismicGrid.activity[cell];
                float currentActivity = SeismicGrid.currentSeismicActivity[cell];
                float upcomingActivity = SeismicGrid.upcomingSeismicActivity[cell];

                Color current = currentActivity == 0 ? Color.black : Color.Lerp(black, red, currentActivity);
                Color upcoming = upcomingActivity == 0 ? Color.black : Color.Lerp(black, yellow, upcomingActivity);
                Color seismicLevel = value < treshold
                    ? Color.Lerp(black, blue, value / treshold)
                    : Color.Lerp(blue, pink, (value - treshold) / treshold);

                return seismicLevel + current + upcoming;
            }
            private static Color GetEpicenterColors(SimDebugView instance, int cell)
            {
                if (WorldEventScheduler.Instance.currentEvent.affectedCells.TryGetValue(cell, out float val))
                    return Color.Lerp(Color.black, Color.red, val);
                else return Color.black;
            }

#if DEBUG
            private static Color GetChunk(SimDebugView instance, int cell)
            {
                float maxVal = SeismicGrid.GetChunkMaxActivity(cell);
                return Color.Lerp(Color.black, Color.green, maxVal);
            }
#endif
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
                        "Toggle Seismic overlay",
                        "priority_1",
                        SeismicOverlayMode.SeismicMode.ID,
                        "",
                        Action.NumActions,
                        "Symmetry overlay",
                        "Symmetry check" };

                object newToggle = OverlayToggleInfoConstructor.Invoke(args);
                ___overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)newToggle);

#if DEBUG
                object[] args2 = new object[] {
                        "Toggle Chunk overlay",
                        "priority_2",
                        SeismicOverlayMode.FaultChunksOverlayMode.ID,
                        "",
                        Action.NumActions,
                        "Symmetry overlay",
                        "Symmetry check" };

                object chunkToggle = OverlayToggleInfoConstructor.Invoke(args2);
                ___overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)chunkToggle);

                object[] args3 = new object[] {
                        "Toggle Eartquake overlay",
                        "priority_3",
                        SeismicOverlayMode.EarthQuakeOverlayMode.ID,
                        "",
                        Action.NumActions,
                        "Symmetry overlay",
                        "Symmetry check" };

                object eqToggle = OverlayToggleInfoConstructor.Invoke(args3);
                ___overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)eqToggle);
#endif
            }
        }
    }
}
