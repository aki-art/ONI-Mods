using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static Slag.STRINGS.CREATURES.SPECIES.MOLE.VARIANT_BRITTLE;

namespace Slag.Content.Critters.BrittleDrill
{
    internal class BrittleDrillConfig : IEntityConfig
    {
        public const string ID = "Beached_BrittleDrill";
        public const string BASE_TRAIT_ID = "Beached_BrittleDrillBaseTrait";
        public const string EGG_ID = "Beached_BrittleDrillEgg";

        public static int EGG_SORT_ORDER = 800;

        public static List<FertilityMonitor.BreedingChance> EGG_CHANCES = new List<FertilityMonitor.BreedingChance>()
        {
            new FertilityMonitor.BreedingChance
            {
                egg = MoleConfig.EGG_ID,
                weight = 0.02f
            },
            new FertilityMonitor.BreedingChance
            {
                egg = MoleDelicacyConfig.EGG_ID,
                weight = 0.02f
            },
            new FertilityMonitor.BreedingChance
            {
                egg = EGG_ID,
                weight = 0.96f
            }
        };

        public GameObject CreatePrefab()
        {
            var prefab = CreateMole(ID, NAME, DESC, "brittledrill_kanim", false);
            EntityTemplates.ExtendEntityToFertileCreature(prefab, EGG_ID, EGG_NAME, DESC, "egg_driller_kanim", MoleTuning.EGG_MASS, "MoleBaby", 60f, 20f, EGG_CHANCES, EGG_SORT_ORDER);

            return prefab;
        }

        public static Diet BrittleDrillDiet()
        {
            var sand = MoleDiet(SimHashes.Sand, SimHashes.Glass, CREATURES.CONVERSION_EFFICIENCY.BAD_1, Mod.Settings.BrittleDrill.KcalPerKgOfSand);
            var regolith = MoleDiet(SimHashes.Regolith, Elements.SlagGlass, CREATURES.CONVERSION_EFFICIENCY.BAD_1, Mod.Settings.BrittleDrill.KcalPerKgOfSand);

            return new Diet(sand, regolith);
        }

        private static Diet.Info MoleDiet(SimHashes eats, SimHashes outputs, float efficiency, float kCalPerEats)
        {
            var inputs = new HashSet<Tag>()
            {
                eats.CreateTag()
            };

            return new Diet.Info(inputs, outputs.CreateTag(), kCalPerEats * 1000f, efficiency, produce_solid_tile: true);
        }

        public static GameObject CreateMole(string id, string name, string desc, string anim_file, bool is_baby = false)
        {
            var prefab = BaseMoleConfig.BaseMole(id, name, desc, BASE_TRAIT_ID, anim_file, is_baby, null, 10);

            prefab.AddTag(GameTags.Creatures.Digger);

            var deltaKcal = Mod.Settings.BrittleDrill.DeltaKcalPerCycle * 1000f;

            EntityTemplates.ExtendEntityToWildCreature(prefab, MoleTuning.PEN_SIZE_PER_CREATURE);
            CritterUtil.CreateCritterBaseTrait(
                BASE_TRAIT_ID,
                name,
                deltaKcal * Mod.Settings.BrittleDrill.StarveInCycles,
                deltaKcal,
                Mod.Settings.BrittleDrill.Hp,
                Mod.Settings.BrittleDrill.LifeTime);

            var diet = BrittleDrillDiet();

            var calorieMonitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
            calorieMonitor.diet = diet;
            calorieMonitor.minPoopSizeInCalories = Mod.Settings.BrittleDrill.MinProductInKcal;

            prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
            prefab.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
            prefab.AddOrGet<LoopingSounds>();

            return prefab;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
    }
}
