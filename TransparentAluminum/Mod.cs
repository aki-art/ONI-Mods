using FUtility;
using HarmonyLib;
using KMod;

namespace TransparentAluminum
{
    public class Mod : UserMod2
    {
        public const string ID = "TransparentAluminum";

        public static string Prefix(string name) => $"{ID}_{name}";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            ModAssets.Load();
        }
    }
}
