using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static PrintingPodRecharge.Cmps.ImmigrationModifier;

namespace PrintingPodRecharge.DataGen
{
    public class BundleGen
    {
        public static void Generate(string path)
        {
            Write(path, "eggy_bioink", GenerateEggs());
            Write(path, "metallic_bioink", GenerateMetals());
            Write(path, "nutritious_bioink", GenerateFood());
            Write(path, "oozing_bioink", GenerateSuperDuplicant());
            Write(path, "seedy_bioink", GenerateSeeds());
            Write(path, "twitch_bioink", GenerateTwitch());
        }

        private static void Write(string folder, string fileName, BundleData data)
        {
            try
            {
                var path = Path.Combine(folder, fileName + ".json");

                var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText(path, json);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Datagen: Could not write bundle file: " + e.Message);
            }
        }

        private static BundleData GenerateTwitch()
        {
            return new BundleData()
            {
                Bundle = Bundle.Twitch,
                ColorHex = "aa5939",
                EnabledWithNoSpecialCarepackages = false,
                DuplicantCount = BundleData.MinMax.None,
                ItemCount = new BundleData.MinMax(5, 5),
                PackageMode = BundleData.Mode.Merge,
                Packages = new List<PackageData>()
                {
                    new PackageData($"{GeyserGenericConfig.ID}_{GeyserGenericConfig.SmallVolcano}", 1f),
                    new PackageData( $"PropFacilityCouch" , 1f),
                    new PackageData( $"PropFacilityTable" , 1f),
                    new PackageData( PuftAlphaConfig.ID, 1f),
                    new PackageData( DustCometConfig.ID, 1f),
                    new PackageData( SimHashes.Corium.ToString() , 300f),
                    new PackageData( SimHashes.TempConductorSolid.ToString() , 0.001f),
                    new PackageData( SimHashes.Cement.ToString() , 200f),
                    new PackageData( SimHashes.Mercury.ToString() , 100f),
                }
            };
        }

        private static BundleData GenerateSeeds()
        {
            return new BundleData()
            {
                Bundle = Bundle.Seed,
                ColorHex = "aa5939",
                EnabledWithNoSpecialCarepackages = false,
                DuplicantCount = BundleData.MinMax.None,
                ItemCount = new BundleData.MinMax(3, 5),
                PackageMode = BundleData.Mode.Merge
            };
        }

        private static BundleData GenerateSuperDuplicant()
        {
            return new BundleData()
            {
                Bundle = Bundle.SuperDuplicant,
                ColorHex = "aa5939",
                EnabledWithNoSpecialCarepackages = false,
                DuplicantCount = new BundleData.MinMax(3, 5),
                ItemCount = BundleData.MinMax.None,
                PackageMode = BundleData.Mode.Merge
            };
        }

        private static BundleData GenerateFood()
        {
            return new BundleData()
            {
                Bundle = Bundle.Food,
                ColorHex = "aa5939",
                EnabledWithNoSpecialCarepackages = false,
                DuplicantCount = BundleData.MinMax.None,
                ItemCount = new BundleData.MinMax(5, 5),
                PackageMode = BundleData.Mode.Merge,
                Packages = new List<PackageData>()
                {
                    new PackageData(FieldRationConfig.ID, 5f)
                    {
                        MaxCycle = 20
                    },
                    new PackageData(FieldRationConfig.ID, 10f)
                    {
                        MinCycle = 20,
                        MaxCycle = 100
                    },
                    new PackageData(FieldRationConfig.ID, 15f)
                    {
                        MinCycle = 100
                    },
                    new PackageData(ColdWheatBreadConfig.ID, 12f)
                    {
                        MinCycle = 100
                    },
                    new PackageData(BurgerConfig.ID, 2f)
                    {
                        MinCycle = 30
                    },
                    new PackageData(ForestForagePlantConfig.ID, 2f),
                    new PackageData(MushroomWrapConfig.ID, 3f)
                    {
                        MinCycle = 50
                    },
                    new PackageData(SpiceBreadConfig.ID, 4f)
                    {
                        MinCycle = 30
                    },
                    new PackageData(GammaMushConfig.ID, 5f)
                    {
                        MinCycle = 30,
                        DLCRequired = true
                    },
                }
            };
        }

        private static BundleData GenerateMetals()
        {
            return new BundleData()
            {
                Bundle = Bundle.Metal,
                ColorHex = "aa5939",
                EnabledWithNoSpecialCarepackages = false,
                DuplicantCount = BundleData.MinMax.None,
                ItemCount = new BundleData.MinMax(4, 4),
                PackageMode = BundleData.Mode.Merge,
                Packages = new List<PackageData>()
                {
                    new PackageData(SimHashes.Aluminum.ToString(), 100f)
                    {
                        MinCycle = 12,
                        HasToBeDicovered = false
                    },
                    new PackageData(SimHashes.Lead.ToString(), 300f)
                    {
                        MinCycle = 30,
                        HasToBeDicovered = false
                    },
                    new PackageData(SimHashes.IronOre.ToString(), 1200f)
                    {
                        MinCycle = 24,
                        HasToBeDicovered = false
                    },
                    new PackageData(SimHashes.Niobium.ToString(), 100f)
                    {
                        MinCycle = 200,
                        HasToBeDicovered = true
                    },
                    new PackageData(SimHashes.Gold.ToString(), 500f)
                    {
                        MinCycle = 0,
                        HasToBeDicovered = false
                    },
                    // Early packages
                    new PackageData(SimHashes.Cuprite.ToString(), 500f)
                    {
                        MinCycle = 0,
                        MaxCycle = 12,
                        HasToBeDicovered = false
                    },
                    new PackageData(SimHashes.GoldAmalgam.ToString(), 1200f)
                    {
                        MinCycle = 0,
                        MaxCycle = 12,
                        HasToBeDicovered = false
                    },
                    new PackageData(SimHashes.Copper.ToString(), 100f)
                    {
                        MinCycle = 0,
                        MaxCycle = 24,
                        HasToBeDicovered = false
                    },
                    new PackageData(SimHashes.Iron.ToString(), 100f)
                    {
                        MinCycle = 0,
                        MaxCycle = 24,
                        HasToBeDicovered = false
                    },
                    new PackageData(SimHashes.AluminumOre.ToString(), 50f)
                    {
                        MinCycle = 0,
                        MaxCycle = 24,
                        HasToBeDicovered = false
                    },
                    // DLC only
                    new PackageData(SimHashes.DepletedUranium.ToString(), 50f)
                    {
                        MinCycle = 12,
                        MaxCycle = 32,
                        HasToBeDicovered = false,
                        DLCRequired = true
                    },
                    new PackageData(SimHashes.DepletedUranium.ToString(), 50f)
                    {
                        MinCycle = 32,
                        HasToBeDicovered = false,
                        DLCRequired = true
                    },
                }
            };
        }

        private static BundleData GenerateEggs()
        {
            return new BundleData()
            {
                Bundle = Bundle.Egg,
                ColorHex = "ba932f",
                Background = "rpp_greyscale_dupeselect_kanim",
                EnabledWithNoSpecialCarepackages = false,
                DuplicantCount = BundleData.MinMax.None,
                ItemCount = new BundleData.MinMax(4, 5),
                PackageMode = BundleData.Mode.Merge,
                Packages = new List<PackageData>()
                {
                    new PackageData("EggRock", 1)
                    {
                        MinCycle = Mod.ArtifactsInCarePackagesEggCycle,
                        ModsRequired = new[] { "Sanchozz.ONIMods.ArtifactCarePackages" },
                        ChanceModifier = 0.2f
                    },
                }
            };
        }
    }
}
