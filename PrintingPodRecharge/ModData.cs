using FUtility;
using System.Collections.Generic;

namespace PrintingPodRecharge
{
    public class ModData
    {
        public bool IsArtifactsInCarePackagesHere;
        public bool IsDGSMHere;
        public bool IsSomeRerollModHere;
        public bool IsMeepHere;
        public bool IsDecorPackIHere;
        public bool IsDiseasesExpandedHere;
        public bool IsTwitchIntegrationHere;

        public HashSet<string> modList = new HashSet<string>();

        public ModData(IReadOnlyList<KMod.Mod> mods)
        {
            // all of these mods replace the reject button with a reroll button
            var rerollMods = new HashSet<string>()
            {
                "immigrantsReroll",
                "luo001PrintingPodRefresh",
                "RefreshImmigratScreenJustForTest",
                "2363561445.Steam", // Refresh Immigrants / 刷新选人
                "2641977549.Steam", // [test]ReshufflingArchetype
            };

            foreach (var mod in mods)
            {
                if (mod.IsEnabledForActiveDlc())
                {
                    modList.Add(mod.staticID);

                    if (mod.staticID == "DGSM")
                    {
                        IsDGSMHere = true;
                    }
                    else if (rerollMods.Contains(mod.staticID))
                    {
                        if (!IsSomeRerollModHere)
                        {
                            Log.Info($"{mod.title} found in modlist: Added default Bio-Ink to regular Care Packages as a way to obtain ink rather than rejection.");
                        }

                        IsSomeRerollModHere = true;
                    }

                    if (mod.staticID == "asquared31415.Meep")
                    {
                        IsMeepHere = true;
                    }

                    else if (mod.staticID == "DecorPackA")
                    {
                        IsDecorPackIHere = true;
                    }
                    else if (mod.staticID == "pether-pg.DiseasesExpanded")
                    {
                        IsDiseasesExpandedHere = true;
                    }
#if TWITCH
                    else if (Mod.Settings.TwitchIntegrationContent)
                    {
                        if (mod.staticID == "asquared31415.TwitchIntegration")
                        {
                            IsTwitchIntegrationHere = true;
                        }
                    }
#endif
                }
            }
        }
    }
}
