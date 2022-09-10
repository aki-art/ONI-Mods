using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using PrintingPodRecharge.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "PrintingPodRecharge_";
        public const string KANIM_PREFIX = "ppr_";

        public static bool IsArtifactsInCarePackagesHere;
        public static bool IsTwitchIntegrationHere = true;

        public static int ArtifactsInCarePackagesEggCycle = 225;

        private static SaveDataManager<General> generalConfig;

        public static General Settings => generalConfig.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            generalConfig = new SaveDataManager<General>(Utils.ModPath);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

#if DATAGEN
            DataGen.BundleGen.Generate(Path.Combine(Utils.ModPath, "data", "bundles"));
#endif
            foreach (var mod in mods)
            {
                if (mod.staticID == "Sanchozz.ONIMods.ArtifactCarePackages" && mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    //ArtifactsInCarePackages();
                }
                else if (mod.staticID == "asquared31415.TwitchIntegration" && mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    IsTwitchIntegrationHere = true;
                    Integration.TwitchIntegration.GeyserPatch.Patch(harmony);
                    Log.Info("Set up compatibility Twitch Integration.\n" +
                        "Added event \"Leaky Printing Pod\"\n" +
                        "Added event \"Useless Print\"");
                }
            }
        }


        [HarmonyPatch(typeof(Game), "Load")]
        public class Game_Load_Patch
        {
            public static void Postfix()
            {
                ArtifactsInCarePackages();
            }
        }

        // reflecting for configuration settings
        public static void ArtifactsInCarePackages()
        {
            IsArtifactsInCarePackagesHere = true;

            var artifactsSettingsType = Type.GetType("ArtifactCarePackages.ArtifactCarePackageOptions, ArtifactCarePackages", false);
            if (artifactsSettingsType != null)
            {
                var p_CyclesUntilTier0 = artifactsSettingsType.GetProperty("CyclesUntilTier0", BindingFlags.Public | BindingFlags.Instance);
                var p_CyclesUntilTierNext = artifactsSettingsType.GetProperty("CyclesUntilTierNext", BindingFlags.Public | BindingFlags.Instance);

                if (p_CyclesUntilTier0 != null && p_CyclesUntilTierNext != null)
                {
                    var settingsInstance = Traverse.Create(artifactsSettingsType.BaseType).Property("Instance").GetValue();

                    if (settingsInstance != null)
                    {
                        var tier0 = (int)p_CyclesUntilTier0.GetValue(settingsInstance);
                        var interval = (int)p_CyclesUntilTierNext.GetValue(settingsInstance);

                        Settings.EggCycle = tier0 + interval * 3;
                        Settings.RainbowEggCycle = tier0 + interval * 4;
                    }
                }
            }

            Log.Info("Set up compatibility with Artifacts In Care Packages.)");
            Log.Debuglog($"Eggs: {Settings.EggCycle}, Rainbow eggs: {Settings.RainbowEggCycle}");
        }
    }
}
