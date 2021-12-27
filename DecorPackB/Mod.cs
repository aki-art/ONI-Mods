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

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            config = new SaveDataManager<Config>(path);

            Log.PrintVersion();
            base.OnLoad(harmony);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            config.WriteIfDoesntExist(false);
        }
    }
}
