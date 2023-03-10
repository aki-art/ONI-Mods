global using FUtility;
using HarmonyLib;
using KMod;
using Randomizer.Elements;
using System.Collections.Generic;
using System.IO;

namespace Randomizer
{
    public class Mod : UserMod2
    {
        public static ElementCollector elementCollector = new();
        public static string worldGenFolder;
        internal static bool isBeachedHere;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            //worldGenFolder = Path.Combine(Util.RootFolder(), "mods", "config", "randomizer", "worldgen");
            worldGenFolder = Path.Combine(Utils.ModPath, "worldgen");
            ModDb.Initialize();
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            foreach(var mod in mods)
            {
                if(mod.IsEnabledForActiveDlc())
                {
                    switch(mod.staticID)
                    {
                        case "Beached": isBeachedHere = true; break;
                    }
                }
            }
        }
    }
}
