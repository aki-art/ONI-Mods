using FUtility;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "PrintingPodRecharge_";
        public const string KANIM_PREFIX = "ppr_";

        public static bool IsArtifactsInCarePackagesHere;
        public static bool IsTwitchIntegrationHere = true;

        public static int ArtifactsInCarePackagesEggCycle = 225;

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach(var mod in mods)
            {
                if(mod.staticID == "Sanchozz.ONIMods.ArtifactCarePackages" && mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    IsArtifactsInCarePackagesHere = true;
                    Log.Info("Set up compatibility with Artifacts In Care Packages. (Egg artifacts now may appear in Eggy prints.)");
                }
                else if(mod.staticID == "asquared31415.TwitchIntegration" && mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    IsTwitchIntegrationHere = true;
                    Integration.TwitchIntegration.GeyserPatch.Patch(harmony);
                    Log.Info("Set up compatibility Twitch Integration. (2 new events to vote for)");
                }
            }
        }
    }
}
