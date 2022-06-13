using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using Kigurumis.Content;
using KMod;
using System.Collections.Generic;

namespace Kigurumis
{
    public class Mod : UserMod2
    {
        public static readonly SaveDataManager<Config> config = new SaveDataManager<Config>();

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

        }
    }
}
