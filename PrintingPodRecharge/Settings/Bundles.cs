using FUtility.SaveData;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PrintingPodRecharge.Cmps;
using System;
using System.Collections.Generic;

namespace PrintingPodRecharge.Settings
{
    public class BundlesConfig : IUserSetting
    {
        public Dictionary<ImmigrationModifier.Bundle, PackageData[]> ItemBundles { get; set; }

        [Serializable]
        public class PackageData
        {
            public PackageData(string prefabID, float amount = 1f)
            {
                PrefabID = prefabID;
                Amount = amount;
            }

            public PackageData AddCondition(BundleFunction fn, string arg = null)
            {
                if(Conditions == null)
                {
                    Conditions = new List<BundleFunction>();
                }

                if(ConditionArguments == null)
                {
                    ConditionArguments = new List<string>();
                }

                if(fn == BundleFunction.Discovered && arg == null)
                {
                    arg = PrefabID;
                }

                Conditions.Add(fn);
                ConditionArguments.Add(arg);

                return this;
            }

            public string PrefabID { get; set; }

            public float Amount { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<BundleFunction> Conditions { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<string> ConditionArguments { get; set; }
        }

        public BundlesConfig()
        {
            ItemBundles = new Dictionary<ImmigrationModifier.Bundle, PackageData[]>();

            var eggsAndbabies = new PackageData[]
            {
                // EGGS
                new PackageData(HatchConfig.EGG_ID, 2f).AddCondition(BundleFunction.Discovered),
                new PackageData(HatchHardConfig.EGG_ID, 2f),
                new PackageData(HatchVeggieConfig.EGG_ID, 2f),

                new PackageData(SquirrelConfig.EGG_ID, 2f),

                new PackageData(PacuConfig.EGG_ID, 4f),
                new PackageData(PacuTropicalConfig.EGG_ID, 3f),
                new PackageData(PacuCleanerConfig.EGG_ID, 3f),

                new PackageData(DreckoConfig.EGG_ID, 1f),
                new PackageData(DreckoPlasticConfig.EGG_ID, 1f),

                new PackageData(LightBugBlackConfig.EGG_ID, 1f),
                new PackageData(LightBugBlueConfig.EGG_ID, 1f),
                new PackageData(LightBugCrystalConfig.EGG_ID, 1f),
                new PackageData(LightBugOrangeConfig.EGG_ID, 1f),
                new PackageData(LightBugPinkConfig.EGG_ID, 1f),
                new PackageData(LightBugPurpleConfig.EGG_ID, 1f),
                new PackageData(LightBugConfig.EGG_ID, 2f),

                new PackageData(CrabConfig.EGG_ID, 2f),

                new PackageData(OilFloaterConfig.EGG_ID, 2f),
                new PackageData(OilFloaterHighTempConfig.EGG_ID, 2f),
                new PackageData(OilFloaterDecorConfig.EGG_ID, 2f),

                new PackageData(PuftConfig.EGG_ID, 2f),
                new PackageData(PuftAlphaConfig.EGG_ID, 2f),
                new PackageData(PuftBleachstoneConfig.EGG_ID, 2f),
                new PackageData(PuftOxyliteConfig.EGG_ID, 2f),

                new PackageData(MoleConfig.EGG_ID, 2f),

                // DLC
                new PackageData(StaterpillarConfig.EGG_ID, 2f),
                new PackageData(DivergentBeetleConfig.EGG_ID, 2f),
                new PackageData(DivergentWormConfig.EGG_ID, 1f),
                new PackageData(DivergentWormConfig.EGG_ID, 2f),

                //BABIES
                new PackageData(BabyCrabConfig.ID, 2f),

                new PackageData(BabyDreckoConfig.ID, 2f),
                new PackageData(BabyDreckoPlasticConfig.ID, 2f),

                new PackageData(BabyHatchConfig.ID, 2f),
                new PackageData(BabyHatchHardConfig.ID, 2f),
                new PackageData(BabyHatchMetalConfig.ID, 2f),
                new PackageData(BabyHatchVeggieConfig.ID, 2f),

                new PackageData(BabyMoleConfig.ID, 2f),

                new PackageData(BabyPacuCleanerConfig.ID, 2f),
                new PackageData(BabyPacuConfig.ID, 2f),
                new PackageData(BabyPacuTropicalConfig.ID, 2f),

                new PackageData(BabyPuftAlphaConfig.ID, 2f),
                new PackageData(BabyPuftBleachstoneConfig.ID, 2f),
                new PackageData(BabyPuftOxyliteConfig.ID, 2f),
                new PackageData(BabyPuftConfig.ID, 2f),

                new PackageData(BabySquirrelConfig.ID, 2f),

                new PackageData(LightBugBabyConfig.ID, 2f),
                new PackageData(LightBugBlackBabyConfig.ID, 2f),
                new PackageData(LightBugBlueBabyConfig.ID, 2f),
                new PackageData(LightBugCrystalBabyConfig.ID, 2f),
                new PackageData(LightBugOrangeBabyConfig.ID, 2f),
                new PackageData(LightBugPinkBabyConfig.ID, 2f),
                new PackageData(LightBugPurpleBabyConfig.ID, 2f),

                new PackageData(OilFloaterBabyConfig.ID, 2f),
                new PackageData(OilFloaterDecorBabyConfig.ID, 2f),
                new PackageData(OilFloaterHighTempBabyConfig.ID, 2f),

                // DLC
                new PackageData(BabyBeeConfig.ID, 2f),
                new PackageData(BabyDivergentBeetleConfig.ID, 2f),
                new PackageData(BabyStaterpillarConfig.ID, 2f),
                new PackageData(BabyWormConfig.ID, 2f),
            };

            ItemBundles.Add(ImmigrationModifier.Bundle.Egg, eggsAndbabies);
        }
 
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BundleFunction
        {
            None,
            CycleCount,
            Discovered
        }
    }
}
