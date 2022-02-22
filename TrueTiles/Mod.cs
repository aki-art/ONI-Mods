using HarmonyLib;
using KMod;

namespace TrueTiles
{
    public class Mod : UserMod2
    {
        public static string ModPath { get; private set; }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            ModPath = path;
        }
    }
}
