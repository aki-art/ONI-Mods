using FUtility;
using Klei;
using Klei.CustomSettings;
using ProcGen;
using Randomizer.Content.Scripts.Generators.YamlWorld;

namespace Randomizer.Content.Scripts
{
    public class WorldRandomizerGenerator : KMonoBehaviour
    {
        private InfoScreenLineItem infoScreenLineItem;
        private ColonyDestinationSelectScreen destinationSelectScreen;

        private float progress = 0f;
        public int seed = 0;
        public SeededRandom rng;

        private ClusterGenerator clusterGenerator;
        public static string currentCluster;

        public void OnCancel()
        {
        }

        public void Configure(InfoScreenLineItem infoScreenLineItem, ColonyDestinationSelectScreen destinationSelectScreen)
        {
            this.infoScreenLineItem = infoScreenLineItem;
            this.destinationSelectScreen = destinationSelectScreen;
            rng = new SeededRandom(seed);
            UpdateProgress();
            Generate();
            destinationSelectScreen.newGameSettings.SetSetting(CustomGameSettingConfigs.ClusterLayout, currentCluster);
            CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, currentCluster);
        }

        public void Generate()
        {
            Log.Info($"Generating new randomized world with seed {0}.");
            clusterGenerator = new ClusterGenerator(rng, 1);

            var errors = ListPool<YamlIO.Error, ClusterGenerator>.Allocate();

            SettingsCache.LoadFiles(Mod.worldGenFolder, "", errors);

            foreach (YamlIO.Error error in errors)
            {
                YamlIO.LogError(error, true);
            }

            errors.Recycle();
        }

        public void UpdateProgress()
        {
            infoScreenLineItem.SetText($"Progress: {GameUtil.GetFormattedPercent(progress * 100f)}");
        }
    }
}
