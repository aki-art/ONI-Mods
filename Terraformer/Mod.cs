using FUtility;
using HarmonyLib;
using KMod;
using Terraformer.Entities;

namespace Terraformer
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<WorldDestroyer> WorldDestroyers = new Components.Cmps<WorldDestroyer>();

        public const string PREFIX = "Terraformer_";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
