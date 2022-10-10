using FUtility;
using FUtilityArt.Components;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace MoreSmallSculptures
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<ArtOverrideRestorer> artRestorers = new Components.Cmps<ArtOverrideRestorer>();
        public static List<string> myOverrides = new List<string>();

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
