using FUtility;
using KMod;

namespace TwelveCyclesOfChristmas
{
    public class Mod : UserMod2
    {
        public override void OnLoad(HarmonyLib.Harmony harmony)
        {
            Log.PrintVersion();
        }
    }
}
