using FUtility;
using HarmonyLib;
using KMod;

namespace TwelveCyclesOfChristmas
{
    public class Mod : UserMod2
    {
        public const string ID = "TwelveCyclesOfChristmas";
        public static string Prefix(string name) => ID + name;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.Info("LOADED");
        }

    }
}
