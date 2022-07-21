using FUtility;
using HarmonyLib;
using KMod;

namespace Backwalls
{
    public class Mod : UserMod2
    {
        public static BackwallRenderer renderer;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
