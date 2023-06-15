using FUtility;
using Newtonsoft.Json;
using PrintingPodRecharge.Content.Cmps;
using PrintingPodRecharge.Content.Items;
using PrintingPodRecharge.Content.Items.BookI;
using PrintingPodRecharge.Content.Items.Dice;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PrintingPodRecharge.DataGen
{
	public class BundleGen
	{
		private static HashSet<string> modPaths;

		private static bool readModPaths;

		private static string ModPathsFilePath => Path.Combine(ModAssets.GetRootPath(), "data", "modpaths.json");

		private static Dictionary<Bundle, string> fileNames = new Dictionary<Bundle, string>()
		{
			{ Bundle.Egg, "eggy_bioink" },
			{ Bundle.Metal, "metallic_bioink" },
			{ Bundle.Food, "nutritious_bioink" },
			{ Bundle.SuperDuplicant, "vacillating_bioink" },
			{ Bundle.Shaker, "suspicious_bioink" },
			{ Bundle.Seed, "seedy_bioink" },
			{ Bundle.Medicinal, "medicinal_bioink" },
			{ Bundle.Twitch, "twitch_bioink" },
			{ Bundle.TwitchHelpful, "twitch_bioink_helpful" }
		};

		public static void Generate(string path, bool force)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			CreatePack(path, fileNames[Bundle.Egg], force, GenerateEggs);
			CreatePack(path, fileNames[Bundle.Metal], force, GenerateMetals);
			CreatePack(path, fileNames[Bundle.Food], force, GenerateFood);
			CreatePack(path, fileNames[Bundle.SuperDuplicant], force, GenerateSuperDuplicant);
			CreatePack(path, fileNames[Bundle.Shaker], force, GenerateRandoDuplicant);
			CreatePack(path, fileNames[Bundle.Seed], force, GenerateSeeds);
			CreatePack(path, fileNames[Bundle.Medicinal], force, GenerateMedicinal);
			CreatePack(path, fileNames[Bundle.Twitch], force, GenerateTwitch);
			CreatePack(path, fileNames[Bundle.TwitchHelpful], force, GenerateTwitchHelpful);
		}

		private static void CreatePack(string folder, string fileName, bool force, Func<BundleData> bundlegen)
		{
			var filePath = Path.Combine(folder, fileName + ".json");

			Log.Debuglog($"Creating pack {filePath}. {File.Exists(filePath)}");
			if (force || !File.Exists(filePath))
			{
				Write(filePath, bundlegen());
			}
		}

		// Made this for other mods, this can apply default settings just once
		// OnAllModsLoaded
		public static void Append(int bundle, string jsonFilePath)
		{
			if (modPaths == null)
			{
				var path = ModPathsFilePath;
				if (!readModPaths && File.Exists(path) && ModAssets.TryReadFile(path, out var json))
				{
					modPaths = JsonConvert.DeserializeObject<HashSet<string>>(json);
					readModPaths = true;
				}
				else
				{
					modPaths = new HashSet<string>();
				}
			}

			if (!File.Exists(jsonFilePath))
			{
				Log.Warning($"{jsonFilePath} does not exist.");
				return;
			}

			if (!modPaths.Add(jsonFilePath))
			{
				Log.Warning("could not add to modpaths");
				return;
			}

			if (bundle >= Enum.GetValues(typeof(Bundle)).Length)
			{
				Log.Warning($"Not a valid bundle id.");
				return;
			}

			if (ModAssets.TryReadFile(jsonFilePath, out var theirJson))
			{
				var newData = JsonConvert.DeserializeObject<List<PackageData>>(theirJson);

				if (newData == null)
				{
					Log.Warning("Null list.");
					return;
				}

				var filePath = Path.Combine(ModAssets.GetRootPath(), "data", "bundles", fileNames[(Bundle)bundle] + ".json");
				if (ModAssets.TryReadFile(filePath, out var myJson))
				{
					var oldData = JsonConvert.DeserializeObject<BundleData>(myJson);

					if (oldData == null)
					{
						Log.Warning("Null data.");
						return;
					}

					if (oldData.Packages == null)
					{
						oldData.Packages = new List<PackageData>();
					}

					oldData.Packages.AddRange(newData);
					Log.Debuglog($"Added {newData.Count} new packages to {(Bundle)bundle}");

					var json = JsonConvert.SerializeObject(oldData, Formatting.Indented, new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore,
						DefaultValueHandling = DefaultValueHandling.Ignore
					});

					File.WriteAllText(filePath, json);

					var json2 = JsonConvert.SerializeObject(modPaths, Formatting.Indented);
					File.WriteAllText(ModPathsFilePath, json2);

					Log.Info($"Appended new default package data from {jsonFilePath}");
				}
			}
		}

		private static void Write(string path, BundleData data)
		{
			try
			{
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

		private static BundleData GenerateMedicinal()
		{
			return new BundleData()
			{
				Bundle = Bundle.Medicinal,
				ColorHex = "45deb4",
				EnabledWithNoSpecialCarepackages = false,
				DuplicantCount = BundleData.MinMax.None,
				ItemCount = new BundleData.MinMax(5, 5),
				Packages = new List<PackageData>()
				{
                    // vanilla stuff
                    DiscoveredPackage(BasicRadPillConfig.ID, 10f),
					new PackageData(BasicCureConfig.ID, 10f),
					DiscoveredPackage(AdvancedCureConfig.ID, 10f),
					DiscoveredPackage(IntermediateCureConfig.ID, 10f),
					new PackageData(AntihistamineConfig.ID, 10f),
					new PackageData(BasicBoosterConfig.ID, 10f),
					new PackageData(OxygenMaskConfig.ID, 2f),
					new PackageData(SimHashes.BleachStone.ToString(), 200f),

                    // cures
                    DiscoveredPackage("AlienSicknessCure", 5f),
					DiscoveredPackage("AntihistamineBooster", 5f),
					DiscoveredPackage("GasCure", 5f),
					new PackageData("SunburnCure", 5f),
					DiscoveredPackage("MutatingAntiviral", 5f),

                    // vaccines
                    DiscoveredPackage("AllergyVaccine", 5f),
					DiscoveredPackage("GassyVaccine", 5f),
					DiscoveredPackage("SlimelungVaccine", 5f),
					DiscoveredPackage("HungermsVaccine", 5f),
					DiscoveredPackage("ZombieSporesVaccine", 5f),

                    // flasks
                    DiscoveredPackage("PollenFlask", 3f),
					DiscoveredPackage("GassyGermFlask", 3f),
					DiscoveredPackage("SlimelungFlask", 3f),
					DiscoveredPackage("HungermsFlask", 3f),
					DiscoveredPackage("ZombieSporesFlask", 3f),
					DiscoveredPackage("MutatingGermFlask", 3f),

                    // misc
                    new PackageData("HappyPill", 10f),
					DiscoveredPackage("MudMask", 10f),
					DiscoveredPackage("SuperSerum", 3f),
					DiscoveredPackage("RadShot", 3f),
					DiscoveredPackage("SapShot", 3f),

                    //ingredients
                    DiscoveredPackage(LightBugOrangeConfig.EGG_ID, 3f),
					DiscoveredPackage(LightBugBlackConfig.EGG_ID, 3f),
					DiscoveredPackage(LightBugConfig.EGG_ID, 5f),
					DiscoveredPackage(SwampLilyFlowerConfig.ID, 8f),
					DiscoveredPackage(PrickleFlowerConfig.SEED_ID, 3f),
					new PackageData(EvilFlowerConfig.SEED_ID, 2f)
					{
						HasToBeDicovered = true,
						ChanceModifier = 0.2f
					}
				}
			};
		}

		private static PackageData DiscoveredPackage(string ID, float amount)
		{
			return new PackageData(ID, amount)
			{
				HasToBeDicovered = true
			};
		}

		private static BundleData GenerateTwitch()
		{
			return new BundleData()
			{
				Bundle = Bundle.Twitch,
				ColorHex = "FFFFFF",
				Background = "rpp_twitch_select_kanim",
				EnabledWithNoSpecialCarepackages = false,
				DuplicantCount = BundleData.MinMax.None,
				ItemCount = new BundleData.MinMax(5, 5),
				Packages = new List<PackageData>()
				{
					new PackageData($"{GeyserGenericConfig.ID}_{GeyserGenericConfig.SmallVolcano}", 1f),
					new PackageData( $"PropFacilityCouch" , 1f),
					new PackageData( $"PropFacilityTable" , 1f),
					new PackageData( PuftAlphaConfig.ID, 1f),
					new PackageData( DustCometConfig.ID, 1f),
					new PackageData( SimHashes.Corium.ToString() , 300f),
					new PackageData( SimHashes.TempConductorSolid.ToString(), 0.001f),
					new PackageData( SimHashes.Cement.ToString(), 200f),
                    //new PackageData( SimHashes.Mercury.ToString(), 100f),
                    new PackageData( SimHashes.DirtyWater.ToString(), 500f),
					new PackageData( SimHashes.Salt.ToString(), 10f),
					new PackageData( GlomConfig.ID, 10f),
					new PackageData( MushBarConfig.ID, 1f),
					new PackageData( BasicForagePlantConfig.ID, 1f),
                    //new PackageData( HighEnergyParticleConfig.ID, 4f),
                    new PackageData( RawEggConfig.ID, 1),
					new PackageData( BioInkConfig.TWITCH, 2),
					new PackageData( CatDrawingConfig.ID, 1f),
					new PackageData( SleepClinicPajamas.ID, 3)
					{
						HasToBeDicovered = true
					},
					new PackageData( SelfImprovablesConfig.MANGA, 1),
				}
			};
		}

		private static BundleData GenerateTwitchHelpful()
		{
			return new BundleData()
			{
				Bundle = Bundle.TwitchHelpful,
				ColorHex = "FFFFFF",
				Background = "rpp_twitch_select_kanim",
				EnabledWithNoSpecialCarepackages = false,
				DuplicantCount = BundleData.MinMax.None,
				ItemCount = new BundleData.MinMax(5, 6),
				Packages = new List<PackageData>()
				{
					new PackageData( BioInkConfig.SEEDED, 4)
					{
						ChanceModifier = 0.5f
					},
					new PackageData( BioInkConfig.MEDICINAL, 4)
					{
						ChanceModifier = 0.5f
					},
					new PackageData( BioInkConfig.GERMINATED, 4)
					{
						ChanceModifier = 0.5f
					},
					new PackageData( BioInkConfig.VACILLATING, 4)
					{
						ChanceModifier = 0.3f
					},
					new PackageData( GeneShufflerRechargeConfig.ID, 1),
					new PackageData( SimHashes.Diamond.ToString(), 1200f),
					new PackageData( SimHashes.Steel.ToString(), 400f)
					{
						MinCycle = 70f
					},
					new PackageData( SimHashes.Gold.ToString(), 800f)
					{
						MinCycle = 40f,
						MaxCycle = 400f,
						ChanceModifier = 0.5f
					},
					new PackageData( SimHashes.Niobium.ToString(), 250f)
					{
						MinCycle = 150f
					},
					new PackageData( SimHashes.OxyRock.ToString(), 600f)
					{
						MaxCycle = 50f
					},
					new PackageData( SimHashes.Obsidian.ToString(), 10000f)
					{
						ChanceModifier = 0.5f,
						MaxCycle = 200
					},
					new PackageData( SimHashes.SuperInsulator.ToString(), 800f)
					{
						MinCycle = 70f
					},
					new PackageData( DreamJournalConfig.ID.ToString(), 8)
					{
						MinCycle = 50f
					},
					new PackageData( ResearchDatabankConfig.ID, 10)
					{
						HasToBeDicovered = true,
						MaxCycle = 800
					},
					new PackageData( SpiceBreadConfig.ID, 6)
					{
						MaxCycle = 150f
					},
					new PackageData( SelfImprovablesConfig.BOOK_VOL1, 1),
					new PackageData( SelfImprovablesConfig.BOOK_VOL2, 1),
					new PackageData( "TI.GlitterPuftConfig", 1),
					new PackageData( SimHashes.SuperCoolant.ToString(), 1000f)
					{
						MinCycle = 150f
					},
					new PackageData( SpicyTofuConfig.ID, 6)
					{
						MaxCycle = 150f
					},
					new PackageData( SimHashes.Wolframite.ToString(), 800f)
					{
						MaxCycle = 150f
					},
					new PackageData( SimHashes.Algae.ToString(), 4000f)
					{
						MaxCycle = 50f
					},
					new PackageData( AntihistamineConfig.ID, 30)
					{
						MaxCycle = 150f
					},
					new PackageData( FruitCakeConfig.ID, 5)
					{
						MaxCycle = 150f
					},
					new PackageData( SimHashes.Ice.ToString(), 4000)
					{
						MaxCycle = 150f
					},
					new PackageData( HeatCubeConfig.ID, 1)
					{
						MaxCycle = 150f
					},
					new PackageData(D6Config.ID, 6),
					new PackageData( SelfImprovablesConfig.D8, 2),
				}
			};
		}

		public static BundleData GenerateSeeds()
		{
			return new BundleData()
			{
				Bundle = Bundle.Seed,
				ColorHex = "409e38",
				EnabledWithNoSpecialCarepackages = false,
				DuplicantCount = BundleData.MinMax.None,
				ItemCount = new BundleData.MinMax(3, 5),
				Version = 1,
				Data = new Dictionary<string, float>()
				{
					{ "SeedCount" , 2 }
				},
				Packages = new List<PackageData>()
				{
					new PackageData(ForestTreeConfig.SEED_ID, 2f)
					{
						MinCycle = 30
					},
					new PackageData(SaltPlantConfig.SEED_ID, 3f)
					{
						MinCycle = 24
					},
					new PackageData(GasGrassConfig.SEED_ID, 3f)
					{
						HasToBeDicovered = true,
						MinCycle = 30
					},
					new PackageData(WormPlantConfig.SEED_ID, 3f)
					{
						MinCycle = 24
					},
					new PackageData(BeanPlantConfig.SEED_ID, 3f)
					{
						MinCycle = 32
					},
					new PackageData(OxyfernConfig.SEED_ID, 3f)
					{
						MinCycle = 12
					},
					new PackageData(ColdWheatConfig.SEED_ID, 3f)
					{
						MinCycle = 24
					},
					new PackageData(ColdBreatherConfig.SEED_ID, 2f)
					{
						MinCycle = 32,
						ChanceModifier = 0.3f
					},

                    // Hydrocactus
                    new PackageData(FilterPlantConfig.SEED_ID, 2f)
					{
						ModsRequired = new[]
						{
							"Sanchozz.ONIMods.Hydrocactus"
						}
					},

                    // Spooky Pumpkin
                    new PackageData("SP_PumpkinSeed", 3f)
					{
						HasToBeDicovered = true
					},

                    // Palmera Tree
                    new PackageData("PalmeraTreeSeed", 2f)
					{
						MinCycle = 24,
						HasToBeDicovered = true
					},

                    // Dupe's Cuisine
                    new PackageData("KakawaTreeSeed", 2f)
					{
						MinCycle = 12
					},
                    // Fervine
                    new PackageData("FervineBulb", 1f),
                    
                    // Beached 
                    new PackageData("Beached_DewPalmSeed", 2f)
					{
						MinCycle = 32
					},
					new PackageData("Beached_MangroveSeed", 2f)
					{
						MinCycle = 32
					},
					new PackageData("CritterTrapPlantSeed", 1f)
					{
						MinCycle = 300,
						ChanceModifier = 0.2f
					},
				}
			};
		}

		private static BundleData GenerateRandoDuplicant()
		{
			return new BundleData()
			{
				Bundle = Bundle.Shaker,
				ColorHex = "ffffff",
				EnabledWithNoSpecialCarepackages = true,
				DuplicantCount = new BundleData.MinMax(4, 5),
				ItemCount = BundleData.MinMax.None,
				Data = new Dictionary<string, float>()
				{
					{ "MinimumSkillBudgetModifier", -6 },
					{ "MaximumSkillBudgetModifier", 13 },
					{ "MaximumTotalBudget", 17 },
					{ "MaxBonusPositiveTraits", 3 },
					{ "MaxBonusNegativeTraits", 3 },
					{ "ChanceForVacillatorTrait", 0.1f },
					{ "ChanceForNoNegativeTraits", 0.2f },
				}
			};
		}

		private static BundleData GenerateSuperDuplicant()
		{
			return new BundleData()
			{
				Bundle = Bundle.SuperDuplicant,
				ColorHex = "6a4ccd",
				EnabledWithNoSpecialCarepackages = true,
				DuplicantCount = new BundleData.MinMax(4, 5),
				ItemCount = BundleData.MinMax.None,
				Data = new Dictionary<string, float>()
				{
					{ "ExtraSkillBudget", 4 }
				}
			};
		}

		private static BundleData GenerateFood()
		{
			var result = new BundleData()
			{
				Bundle = Bundle.Food,
				ColorHex = "7ab337",
				EnabledWithNoSpecialCarepackages = false,
				DuplicantCount = BundleData.MinMax.None,
				ItemCount = new BundleData.MinMax(5, 5),
				BlackList = new List<string>()
				{
					GammaMushConfig.ID
				},
				Data = new Dictionary<string, float>()
				{
					{ "KcalUnit", 2000 },
					{ "MidTierCycle", 40 },
					{ "HighTierCycle", 120 },
					{ "Tier1KcalMultiplier", 3 },
					{ "Tier2KcalMultiplier", 6 },
				}
			};

			return result;
		}

		public const float MINIMUM = 100;
		public const float MODEST = 300;
		public const float GENEROUS = 800;
		public const float LOTS = 1600;

		private static void AddCheapMetal(List<PackageData> data, SimHashes id, float multiplier)
		{
			AddCheapMetal(data, id.ToString(), multiplier);
		}

		private static void AddCheapMetal(List<PackageData> data, string id, float multiplier)
		{
			data.Add(new PackageData(id, Mathf.RoundToInt(MODEST * multiplier))
			{
				MaxCycle = 199
			});

			data.Add(new PackageData(id, Mathf.RoundToInt(GENEROUS * multiplier))
			{
				MinCycle = 200,
				MaxCycle = 499
			});

			data.Add(new PackageData(id, Mathf.RoundToInt(LOTS * multiplier))
			{
				MinCycle = 500
			});
		}


		private static void AddMediumMetal(List<PackageData> data, SimHashes id, float multiplier)
		{
			AddMediumMetal(data, id.ToString(), multiplier);
		}

		private static void AddMediumMetal(List<PackageData> data, string id, float multiplier)
		{
			data.Add(new PackageData(id, Mathf.RoundToInt(MINIMUM * multiplier))
			{
				MinCycle = 40,
				MaxCycle = 299,
				HasToBeDicovered = true
			});

			data.Add(new PackageData(id, Mathf.RoundToInt(MODEST * multiplier))
			{
				MinCycle = 300,
				HasToBeDicovered = true
			});
		}

		private static void AddPreciousMetal(List<PackageData> data, SimHashes id, float multiplier)
		{
			AddPreciousMetal(data, id.ToString(), multiplier);
		}

		private static void AddPreciousMetal(List<PackageData> data, string id, float multiplier)
		{
			data.Add(new PackageData(id, Mathf.RoundToInt(MINIMUM * multiplier))
			{
				MinCycle = 150,
				MaxCycle = 499,
				HasToBeDicovered = true
			});

			data.Add(new PackageData(id, Mathf.CeilToInt(MODEST * multiplier))
			{
				MinCycle = 500,
				HasToBeDicovered = true
			});
		}

		private static BundleData GenerateMetals()
		{
			var packages = new List<PackageData>();

			AddCheapMetal(packages, SimHashes.AluminumOre, 1.2f);
			AddCheapMetal(packages, SimHashes.IronOre, 1f);
			AddCheapMetal(packages, SimHashes.Cuprite, 1.2f);
			AddCheapMetal(packages, SimHashes.GoldAmalgam, 0.8f);
			AddCheapMetal(packages, SimHashes.Cobaltite, 0.8f);
			AddCheapMetal(packages, SimHashes.UraniumOre, 0.5f);
			AddCheapMetal(packages, "BismuthOre", 1f);
			AddCheapMetal(packages, "Zircon", 0.7f);
			AddCheapMetal(packages, "PaleOre", 0.7f);
			AddCheapMetal(packages, "ArgentiteOre", 1f);
			AddCheapMetal(packages, "AurichalciteOre", 1f);
			AddCheapMetal(packages, "Galena", 1f);

			AddMediumMetal(packages, SimHashes.Iron, 1f);
			AddMediumMetal(packages, SimHashes.Aluminum, 1.2f);
			AddMediumMetal(packages, SimHashes.Cobalt, 1f);
			AddMediumMetal(packages, SimHashes.Copper, 1f);
			AddMediumMetal(packages, SimHashes.DepletedUranium, 1.5f);
			AddMediumMetal(packages, SimHashes.Gold, 0.7f);
			AddMediumMetal(packages, SimHashes.Lead, 1f);
			AddMediumMetal(packages, SimHashes.Wolframite, 0.5f);
			AddMediumMetal(packages, SimHashes.Tungsten, 0.33f);
			AddMediumMetal(packages, "Bismuth", 1f);
			AddMediumMetal(packages, "Zirconium", 0.5f);
			AddMediumMetal(packages, "PureMetal", 0.7f);
			AddMediumMetal(packages, "Slag", 2f);
			AddMediumMetal(packages, "SolidBrass", 1f);
			AddMediumMetal(packages, "Plasteel", 0.25f);
			AddMediumMetal(packages, "SolidSilver", 0.8f);
			AddMediumMetal(packages, "SolidTungstenCarbide", 1f);
			AddMediumMetal(packages, "SolidZinc", 1f);

			AddPreciousMetal(packages, SimHashes.Niobium, 0.6f);
			AddPreciousMetal(packages, SimHashes.TempConductorSolid, 0.25f);
			AddPreciousMetal(packages, SimHashes.Steel, 1f);
			AddPreciousMetal(packages, "SolidTitanium", 1f);

			return new BundleData()
			{
				Bundle = Bundle.Metal,
				ColorHex = "aa5939",
				EnabledWithNoSpecialCarepackages = false,
				DuplicantCount = BundleData.MinMax.None,
				ItemCount = new BundleData.MinMax(4, 4),
				Data = new Dictionary<string, float>()
				{
					{ "MidTierCycle", 40 },
					{ "HighTierCycle", 120 },
				},
				Packages = packages
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
				Packages = new List<PackageData>()
				{
					new PackageData("EggRock", 1)
					{
						ModsRequired = new[] { "Sanchozz.ONIMods.ArtifactCarePackages" },
						ChanceModifier = 0.2f,
					},
					new PackageData("RainbowEggRock", 1)
					{
						ModsRequired = new[] { "Sanchozz.ONIMods.ArtifactCarePackages" },
						ChanceModifier = 0.2f
					},
				}
			};
		}
	}
}
