using DecorPackB.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace DecorPackB
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "DecorPackB_";

        public static SaveDataManager<Config> config;
        public static SaveDataManager<ElementColors> colorOverrides;

        public static Config Settings => config.Settings;
        public static ElementColors Colors => colorOverrides.Settings;

        public override void OnLoad(Harmony harmony)
        {
            config = new SaveDataManager<Config>(path);

            colorOverrides = new SaveDataManager<ElementColors>(path, filename: "liquid_colors");
            //colorOverrides.WatchForChanges();
            //colorOverrides.OnRead += config => colorOverrides.Settings.ProcessColors(config);

            Log.PrintVersion();
            base.OnLoad(harmony);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            config.WriteIfDoesntExist(false);
            colorOverrides.WriteIfDoesntExist(false);
        }
    }
}
