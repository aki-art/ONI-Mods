using FUtility;
using HarmonyLib;
using KMod;

namespace BackgroundTiles
{
    public class Mod : UserMod2
    {
        public const string ID = "BackgroundTiles";
        public static string BackwallCategory = "BackWalls";
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
