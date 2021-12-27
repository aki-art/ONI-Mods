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
        //public static IDictionary<string, object> registry;

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            // registry = FURegistry.Initialize();
            config = new SaveDataManager<Config>(path);

            MigrateSettings();

            Log.PrintVersion();

            ConditionalPatching(harmony);
            base.OnLoad(harmony);
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
            config.WriteIfDoesntExist(false);
            Integration.BluePrintsMod.TryPatch(harmony);
        }

        private void MigrateSettings()
        {
            if (Settings.GlassTile.DyeRatio == 0)
            {
                Settings.GlassTile.DyeRatio = .5f;
                config.Write();
            }
        }
    }
}
