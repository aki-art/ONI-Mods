using HarmonyLib;
using KMod;
using System;

namespace TrueTiles
{
    public class Mod : UserMod2
    {
        public static string Path { get; private set; }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Path = path;
        }
    }
}
