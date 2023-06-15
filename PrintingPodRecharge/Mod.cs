using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using PrintingPodRecharge.Content;
using PrintingPodRecharge.Settings;
using System.Collections.Generic;
using System.IO;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public static ModData otherMods;
        public static Harmony harmonyInstance;
        public static BundlaData.Rando errorOverrides;
        public static float randoOverrideChance;

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
            if(Recipes.Process())
            {
                recipesConfig.Write();
            }

            harmonyInstance = harmony;

            RegisterDevTools();
        }

        public static void SaveSettings()
        {
            generalConfig.Write();
        }

        private void CreateConfigDirectory()
        {
            var configPath = ModAssets.GetRootPath();

            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            DataGen.BundleGen.Generate(Path.Combine(ModAssets.GetRootPath(), "data", "bundles"), true);

            otherMods = new ModData(mods);
            if(otherMods.IsTwitchIntegrationHere)
            {
                Integration.TwitchIntegration.GeyserPatch.Patch(harmony);

                Log.Info("Set up compatibility Twitch Integration.\n" +
                    "Added events: \n" +
                    "- \"Leaky Printing Pod\"\n" +
                    "- \"Useless Print\"\n" +
                    "- \"Helpful Print\"\n" +
                    "- \"Spawn Wacky Dupe\"");
            }
        }

        private static void RegisterDevTools()
        {
            var m_RegisterDevTool = AccessTools.DeclaredMethod(typeof(DevToolManager), "RegisterDevTool", new[]
            {
                typeof(string)
            },
            new[]
            {
                typeof(InkDebugTool)
            });

            if (m_RegisterDevTool != null)
            {
                m_RegisterDevTool.Invoke(DevToolManager.Instance, new object[] { "Mods/Bio-Inks" });
            }
        }
    }
}
