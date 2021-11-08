using FUtility;
using HarmonyLib;
using KMod;

namespace Asphalt
{
    public class Mod : UserMod2
    {
        public const string ID = "Asphalt";
        public static string modPath;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            modPath = path;
        }
    }
}
