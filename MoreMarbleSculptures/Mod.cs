using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using MoreMarbleSculptures.Settings;
using Newtonsoft.Json;

namespace MoreMarbleSculptures
{
    public class Mod : UserMod2
    {
        public static SaveDataManager<Config> config;
        public static Config Settings => config.Settings;
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            //ModAssets.LoadAssets();
            //modPath = path;
            config = new SaveDataManager<Config>(path, converters: new Newtonsoft.Json.Converters.StringEnumConverter());
        }

    }
}
