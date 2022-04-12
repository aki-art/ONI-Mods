using FUtility;
using Newtonsoft.Json;
using System;
using System.IO;
using static PrintingPodRecharge.Cmps.BundleData;

namespace PrintingPodRecharge.Cmps
{
    public class BundleDataGen
    {
        public static void Generate(string root)
        {
            Write(root, "eggs", ConfigureEggPackage());
            Write(root, "metals", ConfigureMetalPackage());
            Write(root, "ooze", ConfigureOozePackage());
            Write(root, "vacillator", ConfigureVacillatorPackage());
        }

        private static BundleData ConfigureVacillatorPackage()
        {
            var bundle = new BundleData
            {
                ID = ImmigrationModifier.Bundle.SuperDuplicant,
                PackageCount = 0,
                DupeCount = new MinMax(3, 5),
                BgColor = "FF00FF",
                FXColor = "FF00FF",
                DuplicantOptions = new DuplicantConfig()
                {
                    TraitRarity = Rarity.Epic,
                    AllowVacillatorTraits = true,
                    SkillBudget = new MinMax(2, 5)
                }
            };


            bundle.Packages = null;

            return bundle;
        }

        private static BundleData ConfigureOozePackage()
        {
            var bundle = new BundleData
            {
                ID = ImmigrationModifier.Bundle.Duplicant,
                PackageCount = 0,
                DupeCount = new MinMax(4, 5)
            };

            bundle.Packages = null;

            return bundle;
        }

        private static BundleData ConfigureMetalPackage()
        {
            var bundle = new BundleData
            {
                ID = ImmigrationModifier.Bundle.Metal,
                BgColor = "c9662b",
                FXColor = "d88f2f",
                PackageCount = 4,
                DupeCount = MinMax.ZERO
            };

            bundle.Packages = new Package[]
            {
                new Package(SimHashes.Aluminum.ToString(), 200f),
                new Package(SimHashes.AluminumOre.ToString(), 400f),
                new Package(SimHashes.Copper.ToString(), 200f),
                new Package(SimHashes.Cuprite.ToString(), 500f),
                new Package(SimHashes.DepletedUranium.ToString(), 200f),
                new Package(SimHashes.Gold.ToString(), 200f),
                new Package(SimHashes.GoldAmalgam.ToString(), 400f),
                new Package(SimHashes.Iron.ToString(), 200f),
                new Package(SimHashes.IronOre.ToString(), 500f),
                new Package(SimHashes.Lead.ToString(), 200f),
                new Package(SimHashes.Niobium.ToString(), 50f),
                new Package(SimHashes.Steel.ToString(), 100f),
                new Package(SimHashes.TempConductorSolid.ToString(), 50f),
                new Package(SimHashes.Tungsten.ToString(), 150f),
                new Package(SimHashes.Wolframite.ToString(), 250f),

                new Package(SimHashes.Cobalt.ToString(), 150f),
                new Package(SimHashes.Cobaltite.ToString(), 300f),
            };

            return bundle;
        }

        private static BundleData ConfigureEggPackage()
        {
            var bundle = new BundleData
            {
                ID = ImmigrationModifier.Bundle.Egg,
                BgColor = "FFC642FF",
                FXColor = "FFDD8FFF",
                PackageCount = 5,
                DupeCount = MinMax.ZERO
            };

            bundle.Packages = new Package[]  
            {
                // EGGS
                new Package(HatchConfig.EGG_ID, 2f).AddCondition(BundleFunction.Discovered),
                new Package(HatchHardConfig.EGG_ID, 2f),
                new Package(HatchVeggieConfig.EGG_ID, 2f),

                new Package(SquirrelConfig.EGG_ID, 2f),

                new Package(PacuConfig.EGG_ID, 4f),
                new Package(PacuTropicalConfig.EGG_ID, 3f),
                new Package(PacuCleanerConfig.EGG_ID, 3f),

                new Package(DreckoConfig.EGG_ID, 1f),
                new Package(DreckoPlasticConfig.EGG_ID, 1f),

                new Package(LightBugBlackConfig.EGG_ID, 1f),
                new Package(LightBugBlueConfig.EGG_ID, 1f),
                new Package(LightBugCrystalConfig.EGG_ID, 1f),
                new Package(LightBugOrangeConfig.EGG_ID, 1f),
                new Package(LightBugPinkConfig.EGG_ID, 1f),
                new Package(LightBugPurpleConfig.EGG_ID, 1f),
                new Package(LightBugConfig.EGG_ID, 2f),

                new Package(CrabConfig.EGG_ID, 2f),

                new Package(OilFloaterConfig.EGG_ID, 2f),
                new Package(OilFloaterHighTempConfig.EGG_ID, 2f),
                new Package(OilFloaterDecorConfig.EGG_ID, 2f),

                new Package(PuftConfig.EGG_ID, 2f),
                new Package(PuftAlphaConfig.EGG_ID, 2f),
                new Package(PuftBleachstoneConfig.EGG_ID, 2f),
                new Package(PuftOxyliteConfig.EGG_ID, 2f),

                new Package(MoleConfig.EGG_ID, 2f),

                // EGGS - DLC
                new Package(StaterpillarConfig.EGG_ID, 2f),
                new Package(DivergentBeetleConfig.EGG_ID, 2f),
                new Package(DivergentWormConfig.EGG_ID, 1f),
                new Package(DivergentWormConfig.EGG_ID, 2f),

                //BABIES
                new Package(BabyCrabConfig.ID, 2f),

                new Package(BabyDreckoConfig.ID, 2f),
                new Package(BabyDreckoPlasticConfig.ID, 2f),

                new Package(BabyHatchConfig.ID, 2f),
                new Package(BabyHatchHardConfig.ID, 2f),
                new Package(BabyHatchMetalConfig.ID, 2f),
                new Package(BabyHatchVeggieConfig.ID, 2f),

                new Package(BabyMoleConfig.ID, 2f),

                new Package(BabyPacuCleanerConfig.ID, 2f),
                new Package(BabyPacuConfig.ID, 2f),
                new Package(BabyPacuTropicalConfig.ID, 2f),

                new Package(BabyPuftAlphaConfig.ID, 2f),
                new Package(BabyPuftBleachstoneConfig.ID, 2f),
                new Package(BabyPuftOxyliteConfig.ID, 2f),
                new Package(BabyPuftConfig.ID, 2f),

                new Package(BabySquirrelConfig.ID, 2f),

                new Package(LightBugBabyConfig.ID, 2f),
                new Package(LightBugBlackBabyConfig.ID, 2f),
                new Package(LightBugBlueBabyConfig.ID, 2f),
                new Package(LightBugCrystalBabyConfig.ID, 2f),
                new Package(LightBugOrangeBabyConfig.ID, 2f),
                new Package(LightBugPinkBabyConfig.ID, 2f),
                new Package(LightBugPurpleBabyConfig.ID, 2f),

                new Package(OilFloaterBabyConfig.ID, 2f),
                new Package(OilFloaterDecorBabyConfig.ID, 2f),
                new Package(OilFloaterHighTempBabyConfig.ID, 2f),

                // BABIES - DLC
                new Package(BabyBeeConfig.ID, 2f),
                new Package(BabyDivergentBeetleConfig.ID, 2f),
                new Package(BabyStaterpillarConfig.ID, 2f),
                new Package(BabyWormConfig.ID, 2f),
            };

            return bundle;
        }

        public static void Write(string path, string filename, BundleData bundle)
        {
            try
            {
                path = Path.Combine(path, filename + ".json");
                string json = JsonConvert.SerializeObject(bundle, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Could not write bundle file: " + e.Message);
            }
        }
    }
}
