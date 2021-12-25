using Asphalt.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace Asphalt
{
    public class Mod : UserMod2
    {
        public const string ID = "Asphalt";
        public static string modPath;
        public static SaveDataManager<Config> config;
        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            ModAssets.LoadAssets();
            modPath = path;
            config = new SaveDataManager<Config>(path);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
        }
    }
}
