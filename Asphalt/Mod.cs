using Asphalt.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;

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
            Log.Debuglog("DEBUG");
            Log.PrintVersion();
            ModAssets.LoadAssets();
            modPath = path;
            config = new SaveDataManager<Config>(path);
        }
    }
}
