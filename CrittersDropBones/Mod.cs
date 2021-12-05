using CrittersDropBones.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;

namespace CrittersDropBones
{
    public class Mod : UserMod2
    {
        public static SaveDataManager<Config> config;
        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            config = new SaveDataManager<Config>(path);
            config.WriteIfDoesntExist(false);

            base.OnLoad(harmony);
            Log.PrintVersion();
        }
    }
}
