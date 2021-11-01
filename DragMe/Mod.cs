using HarmonyLib;
using KMod;
using System;

namespace DragMe
{
    public class Mod : UserMod2
    {
        public static string PREFIX = "DP_";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            //Log.PrintVersion();
            Debug.Log("LOADED");
        }
    }
}
