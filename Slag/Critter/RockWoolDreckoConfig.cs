using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Slag.Critter
{
    class RockWoolDreckoConfig : IEntityConfig
	{
		public const string ID = "RockWoolDrecko";
		public const string BASE_TRAIT_ID = "RockWoolDreckoBaseTrait";
		public const string EGG_ID = "RockWoolDreckoEgg";

		public static Tag POOP_ELEMENT = SimHashes.Aluminum.CreateTag();
		public static Tag EMIT_ELEMENT = SlagWoolConfig.ID;

		private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 2f;
		private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;
		private static float KG_POOP_PER_DAY_OF_PLANT = 5f;
		private static float MIN_POOP_SIZE_IN_KG = 1.5f;
		private static float MIN_POOP_SIZE_IN_CALORIES = CALORIES_PER_DAY_OF_PLANT_EATEN * MIN_POOP_SIZE_IN_KG / KG_POOP_PER_DAY_OF_PLANT;

		public static float SCALE_GROWTH_TIME_IN_CYCLES = 8f;
		public static float FIBER_PER_CYCLE = 0.25f;

		public static int EGG_SORT_ORDER = 800;

		public static GameObject CreateDrecko(string id, string name, string desc, string anim_file, bool is_baby)
		{

			GameObject rockwooldrecko = EntityTemplates.ExtendEntityToWildCreature(
					BaseDreckoConfig.BaseDrecko(
					id,
					name: name,
					desc: desc,
					anim_file: anim_file,
					trait_id: BASE_TRAIT_ID,
					is_baby: is_baby,
					symbol_override_prefix: null,
					warnLowTemp: 308.15f,
					warnHighTemp: 363.15f), 
				DreckoTuning.PEN_SIZE_PER_CREATURE, 150f);

			Trait trait = Db.Get().CreateTrait(BASE_TRAIT_ID, name, name, null, false, null, true, true);
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, name, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));

			Diet.Info[] diet_infos = new Diet.Info[]
			{
				new Diet.Info(new HashSet<Tag>
				{
					 SpiceVineConfig.ID.ToTag(),
					 SwampLilyConfig.ID.ToTag(),
					 BasicSingleHarvestPlantConfig.ID.ToTag()
				}, SimHashes.Aluminum.CreateTag() , CALORIES_PER_DAY_OF_PLANT_EATEN, KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, true)
			};
			

			Diet diet = new Diet(diet_infos);

			CreatureCalorieMonitor.Def creature_calorie_monitor = rockwooldrecko.AddOrGetDef<CreatureCalorieMonitor.Def>();
			creature_calorie_monitor.diet = diet;
			creature_calorie_monitor.minPoopSizeInCalories = MIN_POOP_SIZE_IN_CALORIES;

			rockwooldrecko.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			ScaleGrowthMonitor.Def scale_growth_monitor = rockwooldrecko.AddOrGetDef<ScaleGrowthMonitor.Def>();
			scale_growth_monitor.defaultGrowthRate = 1f / SCALE_GROWTH_TIME_IN_CYCLES / 600f;
			scale_growth_monitor.dropMass = FIBER_PER_CYCLE * SCALE_GROWTH_TIME_IN_CYCLES;
			scale_growth_monitor.itemDroppedOnShear = EMIT_ELEMENT;
			scale_growth_monitor.levelCount = 6;
			scale_growth_monitor.targetAtmosphere = SimHashes.Hydrogen;

			return rockwooldrecko;
		}

		public virtual GameObject CreatePrefab()
		{
			return EntityTemplates.ExtendEntityToFertileCreature(
				prefab: CreateDrecko(
					id: ID,
					name: "Rockwool Drecko",
					desc: "RockWool Drecko desc.",
					anim_file: "rockwool_drecko_kanim",
					is_baby: false),
				eggId: "RockWoolDreckoEgg",
				eggName: CREATURES.SPECIES.DRECKO.EGG_NAME,
				eggDesc: CREATURES.SPECIES.DRECKO.DESC,
				egg_anim: "egg_drecko_kanim",
				egg_mass: DreckoTuning.EGG_MASS,
				baby_id: "RockWoolDreckoBaby",
				fertility_cycles: 90f,
				incubation_cycles: 30f,
				egg_chances: DreckoTuning.EGG_CHANCES_BASE,
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
