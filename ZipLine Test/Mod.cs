using FUtility;
using HarmonyLib;
using KMod;

namespace ZiplineTest
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<ZiplineAnchor> ZipLines = new Components.Cmps<ZiplineAnchor>();

        public static NavType ZipLine = (NavType)134;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
