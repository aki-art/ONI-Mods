using FUtility;
using HarmonyLib;
using System;
using System.Reflection;

namespace PrintingPodRecharge.Integration
{
    public class ArtifactsInCarePackages
    {
        // reflecting for configuration settings
        public static void SetData()
        {
            var artifactsSettingsType = Type.GetType("ArtifactCarePackages.ArtifactCarePackageOptions, ArtifactCarePackages", false);
            if (artifactsSettingsType != null)
            {
                Mod.IsArtifactsInCarePackagesHere = true;

                var p_CyclesUntilTier0 = artifactsSettingsType.GetProperty("CyclesUntilTier0", BindingFlags.Public | BindingFlags.Instance);
                var p_CyclesUntilTierNext = artifactsSettingsType.GetProperty("CyclesUntilTierNext", BindingFlags.Public | BindingFlags.Instance);

                if (p_CyclesUntilTier0 != null && p_CyclesUntilTierNext != null)
                {
                    var settingsInstance = Traverse.Create(artifactsSettingsType.BaseType).Property("Instance").GetValue();

                    if (settingsInstance != null)
                    {
                        var tier0 = (int)p_CyclesUntilTier0.GetValue(settingsInstance);
                        var interval = (int)p_CyclesUntilTierNext.GetValue(settingsInstance);

                        Mod.Settings.EggCycle = tier0 + interval * 3;
                        Mod.Settings.RainbowEggCycle = tier0 + interval * 4;
                    }
                }
            }

            Log.Info("Set up compatibility with Artifacts In Care Packages.)");
            Log.Debuglog($"Eggs: {Mod.Settings.EggCycle}, Rainbow eggs: {Mod.Settings.RainbowEggCycle}");
        }
    }
}
