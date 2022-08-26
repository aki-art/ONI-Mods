using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using Slag.Settings;
using System.IO;

namespace Slag
{
    public class Mod : UserMod2
    {
        private static SaveDataManager<Config> config;
        public static SaveDataManager<SlagmiteRewards> slagmiteRewards;
        public static SaveDataManager<GleamiteRewards> gleamiteRewards;

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
            slagmiteRewards = new SaveDataManager<SlagmiteRewards>(Utils.ModPath, filename: "slagmite_rewards");
            gleamiteRewards = new SaveDataManager<GleamiteRewards>(Utils.ModPath, filename: "gleamite_rewards");

            GameTags.MaterialBuildingElements.Add(ModAssets.Tags.slagWool);
        }
    }
}
