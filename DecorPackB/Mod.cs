using DecorPackB.Settings;
using FUtility;
using FUtility.Components;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.Linq;

namespace DecorPackB
{
    public class Mod : UserMod2
    {
        public const string PREFIX = "DecorPackB_";
        private static SaveDataManager<Config> config;
        public static bool isFullMinerYieldHere;
        public static Components.Cmps<Restorer> restorers = new Components.Cmps<Restorer>();

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            config = new SaveDataManager<Config>(Utils.ModPath);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
            isFullMinerYieldHere = mods.Any(m => m.staticID == "BertO.FullMinerYield");
        }
    }
}
