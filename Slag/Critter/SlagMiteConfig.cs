using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Slag.Critter
{
    class SlagMiteConfig : IEntityConfig
	{
		public const string ID = "SlagMite";

		public const string BASE_TRAIT_ID = "SlagMiteBaseTrait";
		public const string EGG_ID = "MiteEgg";
		public const int EGG_SORT_ORDER = 0;

		private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

		private const float KG_ORE_EATEN_PER_CYCLE = 140f;
		private static readonly float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;
		private const float MIN_POOP_SIZE_IN_KG = 25f;
		private const float MOLT_PER_CYCLE = .25f;


		public static GameObject CreateMite(string id, string name, string desc, string anim_file, bool is_baby)
		{
			GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(
				BasePsudoMiteConfig.BasePsudoMite(
					id: id,
					name: name,
					desc: desc,
					anim_file: anim_file,
					traitId: BASE_TRAIT_ID,
					is_baby: is_baby,
					symbolOverridePrefix: null), 
				HatchTuning.PEN_SIZE_PER_CREATURE, 
				100f);

			Trait trait = Db.Get().CreateTrait(
				id: BASE_TRAIT_ID,
				name: name,
				description: name,
				group_name: null,
				should_save: false,
				disabled_chore_groups: null,
				positive_trait: true,
				is_valid_starter_trait: true);

			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));

			ScaleGrowthMonitor.Def scale_growth_monitor = prefab.AddOrGetDef<ScaleGrowthMonitor.Def>();
			scale_growth_monitor.defaultGrowthRate = 1f / .2f / 600f;
			scale_growth_monitor.dropMass = DreckoConfig.FIBER_PER_CYCLE * DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES;
			scale_growth_monitor.itemDroppedOnShear = Items.SlagMiteMoltConfig.ID;
			scale_growth_monitor.levelCount = 1;

			return BasePsudoMiteConfig.SetupDiet(
				prefab: prefab,
				diet_infos: BasePsudoMiteConfig.BasicDiet(
					poopTag: EMIT_ELEMENT.CreateTag(),
					caloriesPerKg: CALORIES_PER_KG_OF_ORE,
					producedConversionRate: CREATURES.CONVERSION_EFFICIENCY.NORMAL,
					diseaseId: null,
					diseasePerKgProduced: 0f),
				referenceCaloriesPerKg: CALORIES_PER_KG_OF_ORE,
				minPoopSizeInKg: MIN_POOP_SIZE_IN_KG);
		}

		public GameObject CreatePrefab()
		{
			return EntityTemplates.ExtendEntityToFertileCreature(
				prefab: CreateMite(
					id: ID,
					name: "Slag Mite",
					desc: "Slag Mite desc",
					anim_file: "hatch_kanim",
					is_baby: false),
				eggId: EGG_ID,
				eggName: "Slag Mite Egg",
				eggDesc: "Slag Mite Egg Desc",
				egg_anim: "egg_hatch_kanim",
				egg_mass: HatchTuning.EGG_MASS,
				baby_id: BabySlagMiteConfig.ID,
				fertility_cycles: 60.000004f,
				incubation_cycles: 20f,
				egg_chances: HatchTuning.EGG_CHANCES_BASE,
				eggSortOrder: EGG_SORT_ORDER,
				is_ranchable: true,
				add_fish_overcrowding_monitor: false,
				add_fixed_capturable_monitor: true,
				egg_anim_scale: 1f);
		}

		public void OnPrefabInit(GameObject prefab)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}

	}

}
