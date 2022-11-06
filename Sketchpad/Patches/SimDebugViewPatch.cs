using HarmonyLib;
using SketchPad;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sketchpad.Patches
{
    public class SimDebugViewPatch
    {
        [HarmonyPatch(typeof(SimDebugView), "OnPrefabInit")]
        public static class SimDebugView_OnPrefabInit_Patch
        {
            public static void Postfix(Dictionary<HashedString, Func<SimDebugView, int, Color>> ___getColourFuncs)
            {
                ___getColourFuncs.Add(SketchOverlayMode.ID, GetColor);
            }

            private static Color GetColor(SimDebugView instance, int cell)
            {
                return Color.black;
            }
        }

    }
}
