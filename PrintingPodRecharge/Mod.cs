using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using Klei.AI;
using KMod;
using PrintingPodRecharge.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TUNING;
using UnityEngine.EventSystems;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public static bool IsArtifactsInCarePackagesHere;
        public static bool IsDGSMHere;
        public static bool IsSomeRerollModHere;
        public static HashSet<string> modList = new HashSet<string>();
        public static Harmony harmonyInstance;
        public static BundlaData.Rando errorOverrides;
        public static float randoOverrideChance;

        public static bool IsTwitchIntegrationHere;

        private static SaveDataManager<General> generalConfig;
        private static SaveDataManager<Recipes> recipesConfig;

        public static General Settings => generalConfig.Settings;

        public static Recipes Recipes => recipesConfig.Settings;

        // hooks for Error challenge
        public static void AddRandoOverride(float randoChance, int minimumSkillBudgetModifier, int maximumSkillBudgetModifier, int maximumTotalBudget, int maxBonusPositiveTraits, int maxBonusNegativeTraits, float chanceForVacillatorTrait, float chanceForNoNegativeTraits)
        {
            randoOverrideChance = randoChance;
            errorOverrides = new BundlaData.Rando
            {
                MinimumSkillBudgetModifier = minimumSkillBudgetModifier,
                MaximumSkillBudgetModifier = maximumSkillBudgetModifier,
                MaximumTotalBudget = maximumTotalBudget,
                MaxBonusPositiveTraits = maxBonusPositiveTraits,
                MaxBonusNegativeTraits = maxBonusNegativeTraits,
                ChanceForVacillatorTrait = chanceForVacillatorTrait,
                ChanceForNoNegativeTraits = chanceForNoNegativeTraits
            };
        }

        public static void RemoveRandoOverride()
        {
            errorOverrides = null;
        }

        public override void OnLoad(Harmony harmony)
        {
            CreateConfigDirectory();
            base.OnLoad(harmony);

            Log.PrintVersion(this);

            generalConfig = new SaveDataManager<General>(ModAssets.GetRootPath());
            recipesConfig = new SaveDataManager<Recipes>(Path.Combine(ModAssets.GetRootPath(), "data"), filename: "recipes");
            harmonyInstance = harmony;
        }

        public static void SaveSettings()
        {
            generalConfig.Write();
        }

        private void CreateConfigDirectory()
        {
            var configPath = ModAssets.GetRootPath();

            if(!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
        }


        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            DataGen.BundleGen.Generate(Path.Combine(ModAssets.GetRootPath(), "data", "bundles"), true);

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
                    else if(rerollMods.Contains(mod.staticID))
                    {
                        if(!IsSomeRerollModHere)
                        {
                            Log.Info($"{mod.title} found in modlist: Added default Bio-Ink to regular Care Packages as a way to obtain ink rather than rejection.");
                        }

                        IsSomeRerollModHere = true;
                    }
                }

                if(Settings.TwitchIntegration)
                {
                if (mod.staticID == "asquared31415.TwitchIntegration" && mod.IsActive() && mod.IsEnabledForActiveDlc())
                    {
                        IsTwitchIntegrationHere = true;
                        Integration.TwitchIntegration.GeyserPatch.Patch(harmony);
                        Log.Info("Set up compatibility Twitch Integration.\n" +
                            "Added events: \n" +
                            "- \"Leaky Printing Pod\"\n" +
                            "- \"Useless Print\"\n" +
                            "- \"Spawn Wacky Dupe\"");
                    }
                }
            }
        }
    }
}
