using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using Slag.Settings;

namespace Slag
{
    public class Mod : UserMod2
    {
        private static SaveDataManager<Config> config;
        public static Config Settings => config.Settings;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();

            OnSetup();
        }

        private void OnSetup()
        {
            config = new SaveDataManager<Config>(Utils.ModPath);

            GameTags.MaterialBuildingElements.Add(ModAssets.Tags.slagWool);
        }
    }
}
