using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using SolidWaterPump.Settings;

namespace SolidWaterPump
{
    public class Mod : UserMod2
    {
        public static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();

            config = new SaveDataManager<Config>(path, true, true);
            Config.Sanitize(config.Settings);
        }
    }
}
