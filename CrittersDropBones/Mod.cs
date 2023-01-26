global using FUtility;
using CrittersDropBones.Settings;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace CrittersDropBones
{
    public class Mod : UserMod2
    {
        public static DropsConfig dropsConfig = new();

        public static bool isSpookyPumpkinHere;
        public static bool isPalmeraTreeHere;
        public static bool isCannedFoodHere;

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
                if(mod.IsEnabledForActiveDlc())
                {
                    switch (mod.staticID)
                    {
                        case "SpookyPumpkin":
                            isSpookyPumpkinHere = true;
                            break;
                        case "Cairath.PalmeraTree":
                            isPalmeraTreeHere = true;
                            break;
                        case "CannedFoods":
                            isCannedFoodHere = true;
                            break;
                    }
                }
            }
        }
    }
}
