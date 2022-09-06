using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using ZipLine.Content.Buildings.ZiplinePost;

namespace ZipLine
{
    public class Mod : UserMod2
    {
        public static Components.Cmps<ZiplineAnchor> anchors = new Components.Cmps<ZiplineAnchor>();

        private static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();
            config = new SaveDataManager<Config>(Utils.ModPath);
        }
    }
}
