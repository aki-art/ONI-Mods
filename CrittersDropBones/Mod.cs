global using FUtility;
using CrittersDropBones.Settings;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace CrittersDropBones
{
    public class Mod : UserMod2
    {
        public static DropsConfig dropsConfig = new DropsConfig();

        public const string PREFIX = "CrittersDropBones_";

        public static bool IsSpookyPumpkinHere;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach(var mod in mods)
            {
                if(mod.staticID == "SpookyPumpkin" && mod.IsEnabledForActiveDlc())
                {
                    IsSpookyPumpkinHere = true;
                }
            }
        }
    }
}
