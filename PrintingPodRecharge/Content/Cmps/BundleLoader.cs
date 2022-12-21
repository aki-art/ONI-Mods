using FUtility;
using Newtonsoft.Json;
using PrintingPodRecharge.DataGen;
using PrintingPodRecharge.Settings;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PrintingPodRecharge.Content.Cmps
{
    public class BundleLoader
    {
        public static BundlaData bundleSettings = new BundlaData();

        public static void LoadBundles(ref Dictionary<Bundle, ImmigrationModifier.CarePackageBundle> bundles)
        {
            bundles = new Dictionary<Bundle, ImmigrationModifier.CarePackageBundle>();

            var path = Path.Combine(ModAssets.GetRootPath(), "data", "bundles");

            foreach (var file in Directory.GetFiles(path, "*.json"))
            {
                if (ModAssets.TryReadFile(file, out var json))
                {
                    var bundle = JsonConvert.DeserializeObject<BundleData>(json);
                    if (bundle != null)
                    {
                        var infos = new List<CarePackageInfo>();

                        switch (bundle.Bundle)
                        {
                            case Bundle.Food:
                                GenerateFoodPackages(bundle, infos);
                                break;
                            case Bundle.Egg:
                                GenerateEggPackages(bundle, infos);
                                break;
                            case Bundle.Seed:
                                if (bundle.Version < 1)
                                {
                                    bundle = BundleGen.GenerateSeeds();
                                }
                                GenerateSeedPackages(bundle, infos);
                                break;
                            case Bundle.SuperDuplicant:
                                bundleSettings.vacillating = new BundlaData.Vacillating()
                                {
                                    ExtraSkillBudget = bundle.GetOrDefault("ExtraSkillBudget", 8)
                                };
                                break;
                            case Bundle.Shaker:
                                bundleSettings.defaultRandoPreset = new BundlaData.Rando()
                                {
                                    MinimumSkillBudgetModifier = bundle.GetOrDefault("MinimumSkillBudgetModifier", -6),
                                    MaximumSkillBudgetModifier = bundle.GetOrDefault("MaximumSkillBudgetModifier", 13),
                                    MaximumTotalBudget = bundle.GetOrDefault("MaximumTotalBudget", 17),
                                    MaxBonusPositiveTraits = bundle.GetOrDefault("MaxBonusPositiveTraits", 3),
                                    MaxBonusNegativeTraits = bundle.GetOrDefault("MaxBonusNegativeTraits", 3),
                                    ChanceForVacillatorTrait = bundle.GetOrDefault("ChanceForVacillatorTrait", 0.1f),
                                    ChanceForNoNegativeTraits = bundle.GetOrDefault("ChanceForNoNegativeTraits", 0.2f),
                                };

                                break;
                        }

                        foreach (var package in bundle.Packages)
                        {
                            var id = package.PrefabID;

                            if (bundle.BlackList.Contains(id))
                            {
                                continue;
                            }

                            if (!AreDependentModsHere(package))
                            {
                                continue;
                            }

                            var prefab = Assets.TryGetPrefab(id);
                            if (prefab == null)
                            {
                                continue;
                            }

                            var info = new CarePackageInfo(package.PrefabID, package.Amount, () =>
                            {
                                var currentCycle = GameClock.Instance.GetCycle();
                                if (package.MinCycle > 0 && currentCycle < package.MinCycle)
                                {
                                    return false;
                                }

                                if (package.MaxCycle > 0 && currentCycle > package.MaxCycle)
                                {
                                    return false;
                                }

                                if (package.HasToBeDicovered && !DiscoveredResources.Instance.IsDiscovered(package.PrefabID))
                                {
                                    return false;
                                }

                                if (package.DLCRequired && !DlcManager.IsExpansion1Active())
                                {
                                    return false;
                                }

                                return true;
                            });

                            infos.Add(info);
                        }


                        var color = bundle.ColorHex.IsNullOrWhiteSpace() ? Color.white : Util.ColorFromHex(bundle.ColorHex);

                        bundles.Add(bundle.Bundle,
                            new ImmigrationModifier.CarePackageBundle(
                                infos,
                                bundle.DuplicantCount.Min,
                                bundle.DuplicantCount.Max,
                                bundle.ItemCount.Min,
                                bundle.ItemCount.Max,
                                color,
                                color,
                                bundle.EnabledWithNoSpecialCarepackages,
                                bundle.Background));
                    }
                }
            }
        }

        private static bool AreDependentModsHere(PackageData package)
        {
            if (package.ModsRequired != null)
            {
                foreach (var requiredMod in package.ModsRequired)
                {
                    if (!Mod.otherMods.modList.Contains(requiredMod))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static float KCalToCount(float kcalTarget, float kcalPerItem)
        {
            return Mathf.Max(1, Mathf.RoundToInt(kcalTarget / kcalPerItem));
        }

        private static void GenerateEggPackages(BundleData bundle, List<CarePackageInfo> infos)
        {
            if (bundle.OverrideInternalLogic)
            {
                return;
            }

            var definedPackages = ListPool<string, BundleLoader>.Allocate();

            foreach (var package in bundle.Packages)
            {
                definedPackages.Add(package.PrefabID);
            }

            var eggCount = bundle.GetOrDefault("EggCount", 2);
            var babyCount = bundle.GetOrDefault("BabyCount", 1);

            var eggs = Assets.GetPrefabsWithTag(GameTags.IncubatableEgg);
            foreach (var egg in eggs)
            {
                var id = egg.PrefabID().ToString();

                if (definedPackages.Contains(id) || bundle.BlackList.Contains(id))
                {
                    continue;
                }

                infos.Add(new CarePackageInfo(id, eggCount, null));

                var incubationMonitor = egg.GetDef<IncubationMonitor.Def>();

                if (incubationMonitor != null)
                {
                    var babyId = incubationMonitor.spawnedCreature.ToString();
                    var babyPrefab = Assets.GetPrefab(babyId);

                    if (babyPrefab == null)
                    {
                        continue;
                    }

                    if (bundle.BlackList.Contains(babyId))
                    {
                        continue;
                    }

                    infos.Add(new CarePackageInfo(babyId, babyCount, null));
                }
            }

            definedPackages.Recycle();
        }

        private static void GenerateSeedPackages(BundleData bundle, List<CarePackageInfo> infos)
        {
            if (bundle.OverrideInternalLogic)
            {
                return;
            }

            var definedPackages = ListPool<string, BundleLoader>.Allocate();

            foreach (var package in bundle.Packages)
            {
                definedPackages.Add(package.PrefabID);
            }

            var count = bundle.GetOrDefault("SeedCount", 2);
            var prefabs = Assets.GetPrefabsWithTag(GameTags.Seed);
            foreach (var prefab in prefabs)
            {
                var id = prefab.PrefabID().ToString();

                if (definedPackages.Contains(id) || bundle.BlackList.Contains(id))
                {
                    continue;
                }

                infos.Add(new CarePackageInfo(id.ToString(), count, null));
            }

            definedPackages.Recycle();
        }

        private static void GenerateFoodPackages(BundleData bundle, List<CarePackageInfo> infos)
        {
            if (bundle.OverrideInternalLogic)
            {
                return;
            }

            var definedPackages = ListPool<string, BundleLoader>.Allocate();

            foreach (var package in bundle.Packages)
            {
                definedPackages.Add(package.PrefabID);
            }

            var tier0Kcal = bundle.GetOrDefault("KcalUnit", 2000);
            var tier1Kcal = bundle.GetOrDefault("Tier1KcalMultiplier", 3f);
            var tier2Kcal = bundle.GetOrDefault("Tier2KcalMultiplier", 6f);
            var midCycle = bundle.GetOrDefault("MidTierCycle", 40);
            var endCycle = bundle.GetOrDefault("HighTierCycle", 120);

            var foods = Assets.GetPrefabsWithComponent<Edible>();
            foreach (var food in foods)
            {
                var id = food.PrefabID().ToString();

                if (definedPackages.Contains(id) || bundle.BlackList.Contains(id))
                {
                    continue;
                }

                if (food.TryGetComponent(out Edible edible))
                {
                    var foodInfo = edible.FoodInfo;
                    var kcalPerUnit = foodInfo.CaloriesPerUnit / 1000f;

                    if (foodInfo.Quality < TUNING.FOOD.FOOD_QUALITY_AMAZING)
                    {
                        var lowTier = new CarePackageInfo(id.ToString(), KCalToCount(tier0Kcal, kcalPerUnit), () =>
                        {
                            return GameClock.Instance.GetCycle() < midCycle;
                        });
                        infos.Add(lowTier);
                    }

                    if (foodInfo.Quality < TUNING.FOOD.FOOD_QUALITY_MORE_WONDERFUL)
                    {
                        var midTier = new CarePackageInfo(id.ToString(), KCalToCount(tier1Kcal, kcalPerUnit), () =>
                        {
                            var cycle = GameClock.Instance.GetCycle();
                            return cycle >= midCycle && cycle < endCycle;
                        });
                        infos.Add(midTier);
                    }

                    var highTier = new CarePackageInfo(id.ToString(), KCalToCount(tier2Kcal, kcalPerUnit), () =>
                    {
                        return GameClock.Instance.GetCycle() >= endCycle;
                    });

                    infos.Add(highTier);
                }
            }

            definedPackages.Recycle();
        }
    }
}
