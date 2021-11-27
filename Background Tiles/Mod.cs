using BackgroundTiles.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;

namespace BackgroundTiles
{
    public class Mod : UserMod2
    {
        public const string ID = "BackgroundTiles";
        public static string BackwallCategory = "BackWalls";
        public static HashedString BackwallCategoryHashed = new HashedString("BackWalls");
        public static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            ModAssets.LoadAssets();
            config = new SaveDataManager<Config>(path);
        }
    }
}
