using FUtility;
using HarmonyLib;
using KMod;
using Rendering;
using System;
using UnityEngine;

namespace DecorPackA
{
    public class Mod : UserMod2
    {
        public static string PREFIX = "DP_";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
