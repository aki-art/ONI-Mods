using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using PrintingPodRecharge.Settings;
using System.Collections.Generic;
using System.IO;

namespace PrintingPodRecharge
{
    public class Mod : UserMod2
    {
        public static bool IsArtifactsInCarePackagesHere;
        // public static bool IsTwitchIntegrationHere;

        private static SaveDataManager<General> generalConfig;
        private static SaveDataManager<Recipes> recipesConfig;

        public static HashSet<string> modList = new HashSet<string>();

        public static General Settings => generalConfig.Settings;

        public static Recipes Recipes => recipesConfig.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Log.PrintVersion();

            generalConfig = new SaveDataManager<General>(ModAssets.GetRootPath());
            recipesConfig = new SaveDataManager<Recipes>(Path.Combine(ModAssets.GetRootPath(), "data"), filename: "recipes");
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            DataGen.BundleGen.Generate(Path.Combine(ModAssets.GetRootPath(), "data", "bundles"), true);

            foreach (var mod in mods)
            {
                if (mod.IsEnabledForActiveDlc())
                {
                    modList.Add(mod.staticID);
                }

                /*
                if (mod.staticID == "asquared31415.TwitchIntegration" && mod.IsActive() && mod.IsEnabledForActiveDlc())
                {
                    IsTwitchIntegrationHere = true;
                    Integration.TwitchIntegration.GeyserPatch.Patch(harmony);
                    Log.Info("Set up compatibility Twitch Integration.\n" +
                        "Added event \"Leaky Printing Pod\"\n" +
                        "Added event \"Useless Print\"");
                }
                */
            }
        }


        [HarmonyPatch(typeof(Game), "Load")]
        public class Game_Load_Patch
        {
            public static void Postfix()
            {
                Integration.ArtifactsInCarePackages.SetData();
            }
        }
    }
}
