using HarmonyLib;
using KMod;
using PrintingPodRecharge.Cmps;
using System.IO;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "PrintingPodRecharge_";
        public const string KANIM_PREFIX = "ppr_";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            BundleDataGen.Generate(Path.Combine(path, "data", "packages"));
        }
    }
}
