using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;

namespace MayISit
{
    public class Mod : UserMod2
    {
        private static SaveDataManager<Config> config;

        public static Config Setting => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Log.PrintVersion();
            config = new SaveDataManager<Config>(Utils.ModPath);
        }
    }
}
