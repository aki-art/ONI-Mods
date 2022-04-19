using HarmonyLib;
using KMod;
using PrintingPodRecharge.Cmps;
using System.Collections.Generic;
using System.IO;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "PrintingPodRecharge_";
        public const string KANIM_PREFIX = "ppr_";

        public static HashSet<string> mods;

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            Mod.mods = new HashSet<string>();
            foreach(var mod in mods)
            {
                if(mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    Mod.mods.Add(mod.staticID);
                }
            }
        }
    }
}
