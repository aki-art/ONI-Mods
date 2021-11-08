using FUtility;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA
{
    public class Mod : UserMod2
    {
        public static string PREFIX = "DP_";
        public static Dictionary<int, Color> colorOverlays = new Dictionary<int, Color>();

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
