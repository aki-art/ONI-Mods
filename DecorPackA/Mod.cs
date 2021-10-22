using FUtility;
using HarmonyLib;
using KMod;

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
