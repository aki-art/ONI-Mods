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

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            modPath = path;
            config = new SaveDataManager<Config>(path);
            Log.Debuglog("Read configs:", config.Settings.Test);
            config.Settings.Test += "Another value";
            config.Write();
        }
    }
}
