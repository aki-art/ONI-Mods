using FUtility;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace DecorPackA
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "DecorPackA_";
        internal static Harmony harmony;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Mod.harmony = harmony;

            Log.PrintVersion();
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            Integration.BluePrintsMod.TryPatch(harmony);
        }
    }
}
