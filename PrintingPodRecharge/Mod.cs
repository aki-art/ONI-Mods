using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "PrintingPodRecharge_";
        public const string KANIM_PREFIX = "ppr_";

        public static HashSet<string> mods;

        public static bool IsArtifactsInCarePackagesHere;

        public static int ArtifactsInCarePackagesEggCycle = 225;

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            Mod.mods = new HashSet<string>();
            foreach(var mod in mods)
            {
                if(mod.staticID == "Sanchozz.ONIMods.ArtifactCarePackages" && mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    IsArtifactsInCarePackagesHere = true;
                }

                if(mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    Mod.mods.Add(mod.staticID);
                }
            }
        }
    }
}
