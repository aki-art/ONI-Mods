using DecorPackA.Patches;
using DecorPackA.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace DecorPackA
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "DecorPackA_";
        public static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();

            config = new SaveDataManager<Config>(path);
            MigrateSettings();

            ConditionalPatching(harmony);
        }

        public void ConditionalPatching(Harmony harmony)
        {
            if (Settings.GlassTile.UseDyeTC)
            {
                AdditionalDetailsPanelPatch.Patch(harmony);
            }
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            Integration.BluePrintsMod.TryPatch(harmony);
        }

        private void MigrateSettings()
        {
            if(Settings.GlassTile.DyeRatio == 0)
            {
                Settings.GlassTile.DyeRatio = .25f;
                config.Write();
                Log.Info("Added/Reset Dye Ratio option to settings.json with default value of 0.25");
            }
        }
    }
}
