using FUtility;
using Newtonsoft.Json;
using PrintingPodRecharge.Content;
using PrintingPodRecharge.Settings;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
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
                                GenerateSeedPackages(bundle, infos);
                                break;
                            case Bundle.SuperDuplicant:
                                bundleSettings.vacillating = new BundlaData.Vacillating()
                                {
                                    ExtraSkillBudget = (int)GetOrDefault(bundle.Data, "ExtraSkillBudget", 8)
                                };
                                break;
                            case Bundle.Shaker:
                                bundleSettings.rando = new BundlaData.Rando()
                                {
                                    MinimumSkillBudgetModifier = (int)GetOrDefault(bundle.Data, "MinimumSkillBudgetModifier", -6),
                                    MaximumSkillBudgetModifier = (int)GetOrDefault(bundle.Data, "MaximumSkillBudgetModifier", 13),
                                    MaximumTotalBudget = (int)GetOrDefault(bundle.Data, "MaximumTotalBudget", 17),
                                    MaxBonusPositiveTraits = (int)GetOrDefault(bundle.Data, "MaxBonusPositiveTraits", 3),
                                    MaxBonusNegativeTraits = (int)GetOrDefault(bundle.Data, "MaxBonusNegativeTraits", 3),
                                    ChanceForVacillatorTrait = GetOrDefault(bundle.Data, "ChanceForVacillatorTrait", 0.1f),
                                    ChanceForNoNegativeTraits = GetOrDefault(bundle.Data, "ChanceForNoNegativeTraits", 0.2f),
                                };
                                break;
                        }

                        foreach (var package in bundle.Packages)
                        {
                            Log.Debuglog($"ADDING PACKAGE INFO " + package.PrefabID);
                            var id = package.PrefabID;

                            if (bundle.BlackList.Contains(id))
                            {
                                continue;
                            }

                            var prefab = Assets.TryGetPrefab(id);
                            if (prefab == null || prefab.HasTag(PTags.dontPrint))
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

                                if (package.ModsRequired != null)
                                {
                                    foreach (var requiredMod in package.ModsRequired)
                                    {
                                        if (!Mod.modList.Contains(requiredMod))
                                        {
                                            Log.Debuglog($"missing mod requirement {requiredMod}");
                                            return false;
                                        }
                                    }
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
                                bundle.Background));

                        if(bundle.Bundle == Bundle.Egg)
                        {
                            foreach(var info in infos)
                            {
                                Log.Debuglog($"INFO {info.id}");
                            }
                        }
                    }
                }
            }
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

            var definedPackages = new HashSet<string>();

            foreach (var package in bundle.Packages)
            {
                //bundle.BlackList.Add(package.PrefabID);
                definedPackages.Add(package.PrefabID);
            }

            var eggCount = GetOrDefault(bundle.Data, "EggCount", 2);
            var babyCount = GetOrDefault(bundle.Data, "BabyCount", 1);

            var eggs = Assets.GetPrefabsWithTag(GameTags.IncubatableEgg);
            foreach (var egg in eggs)
            {
                var id = egg.PrefabID().ToString();

                if (definedPackages.Contains(id) || bundle.BlackList.Contains(id) || egg.HasTag(PTags.dontPrint))
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

                    if (bundle.BlackList.Contains(babyId) || babyPrefab.HasTag(PTags.dontPrint))
                    {
                        continue;
                    }

                    infos.Add(new CarePackageInfo(babyId, babyCount, null));
                }
            }
        }

        private static void GenerateSeedPackages(BundleData bundle, List<CarePackageInfo> infos)
        {
            if (bundle.OverrideInternalLogic)
            {
                return;
            }

            var count = GetOrDefault(bundle.Data, "SeedCount", 2);
            var prefabs = Assets.GetPrefabsWithTag(GameTags.Seed);
            foreach (var prefab in prefabs)
            {
                var id = prefab.PrefabID();

                if (bundle.BlackList.Contains(id.ToString()) || prefab.HasTag(PTags.dontPrint))
                {
                    continue;
                }

                infos.Add(new CarePackageInfo(id.ToString(), count, null));
            }

            foreach (var package in bundle.Packages)
            {
                bundle.BlackList.Add(package.PrefabID);
            }
        }

        private static void GenerateFoodPackages(BundleData bundle, List<CarePackageInfo> infos)
        {
            if (bundle.OverrideInternalLogic)
            {
                return;
            }

            var tier0Kcal = GetOrDefault(bundle.Data, "KcalUnit", 2000);
            var tier1Kcal = GetOrDefault(bundle.Data, "Tier1KcalMultiplier", 3f);
            var tier2Kcal = GetOrDefault(bundle.Data, "Tier2KcalMultiplier", 6f);
            var midCycle = GetOrDefault(bundle.Data, "MidTierCycle", 40);
            var endCycle = GetOrDefault(bundle.Data, "HighTierCycle", 120);

            var foods = Assets.GetPrefabsWithComponent<Edible>();
            foreach (var food in foods)
            {
                var id = food.PrefabID();

                if (bundle.BlackList.Contains(id.ToString()) || food.HasTag(PTags.dontPrint))
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

            foreach (var package in bundle.Packages)
            {
                bundle.BlackList.Add(package.PrefabID);
            }
        }

        private static float GetOrDefault(Dictionary<string, float> dictionary, string key, float value)
        {
            if (dictionary.TryGetValue(key, out var result))
            {
                return result;
            }

            return value;
        }

    }
}
