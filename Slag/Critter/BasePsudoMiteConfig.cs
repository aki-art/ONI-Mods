using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace Slag.Critter
{
	public static class BasePsudoMiteConfig
	{
		public static GameObject BasePsudoMite(string id, string name, string desc, string anim_file, string traitId, bool is_baby, string symbolOverridePrefix = null)
		{
			float mass = 100f;
			EffectorValues tier = DECOR.BONUS.TIER0;
			string nav_grid = is_baby ? "DreckoBabyNavGrid" : "DreckoNavGrid";

			var prefab = EntityTemplates.ExtendEntityToBasicCreature(
				template: EntityTemplates.CreatePlacedEntity(
					id: id,
					name: name,
					desc: desc,
					mass: mass,
					anim: Assets.GetAnim(anim_file),
					initialAnim: "idle_loop",
					sceneLayer: Grid.SceneLayer.Creatures,
					width: 1,
					height: 1,
					decor: tier,
					noise: default,
					element: SimHashes.Creature,
					additionalTags: null,
					defaultTemperature: 293f),
				faction: FactionManager.FactionID.Pest,
				initialTraitID: traitId,
				NavGridName: nav_grid,
				navType: NavType.Floor,
				max_probing_radius: 32,
				moveSpeed: 2f,
				onDeathDropID: "Meat",
				onDeathDropCount: 2,
				drownVulnerable: true,
				entombVulnerable: false,
				warningLowTemperature: 283.15f,
				warningHighTemperature: 293.15f,
				lethalLowTemperature: 243.15f,
				lethalHighTemperature: 343.15f);

			if (symbolOverridePrefix != null)
				prefab.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);

			prefab.AddOrGet<Trappable>();
			prefab.AddOrGetDef<CreatureFallMonitor.Def>();
			prefab.AddOrGetDef<BurrowMonitor.Def>();
			prefab.AddOrGetDef<WorldSpawnableMonitor.Def>().adjustSpawnLocationCb = new Func<int, int>(AdjustSpawnLocationCB);
			prefab.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
			prefab.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);

			SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_idle", NOISE_POLLUTION.CREATURES.TIER2);
			SoundEventVolumeCache.instance.AddVolume("FloorSoundEvent", "Hatch_footstep", NOISE_POLLUTION.CREATURES.TIER1);
			SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_land", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_chew", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_hurt", NOISE_POLLUTION.CREATURES.TIER5);
			SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_die", NOISE_POLLUTION.CREATURES.TIER5);
			SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_emerge", NOISE_POLLUTION.CREATURES.TIER6);
			SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_hide", NOISE_POLLUTION.CREATURES.TIER6);

			EntityTemplates.CreateAndRegisterBaggedCreature(prefab, true, true, false);

			KPrefabID prefab_id = prefab.GetComponent<KPrefabID>();
			prefab_id.AddTag(GameTags.Creatures.Walker, false);
			prefab_id.prefabInitFn += delegate (GameObject inst)
			{
				inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
			};

			bool can_burrow = !is_baby;
			ChoreTable.Builder chore_table = new ChoreTable.Builder()
				.Add(new DeathStates.Def(), true)
				.Add(new AnimInterruptStates.Def(), true)
				.Add(new ExitBurrowStates.Def(), can_burrow)
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Burrowed, true, "idle_mound", STRINGS.CREATURES.STATUSITEMS.BURROWED.NAME, STRINGS.CREATURES.STATUSITEMS.BURROWED.TOOLTIP), can_burrow)
				.Add(new GrowUpStates.Def(), true)
				.Add(new TrappedStates.Def(), true)
				.Add(new IncubatingStates.Def(), true)
				.Add(new BaggedStates.Def(), true)
				.Add(new FallStates.Def(), true)
				.Add(new StunnedStates.Def(), true)
				.Add(new DrowningStates.Def(), true)
				.Add(new DebugGoToStates.Def(), true)
				.Add(new FleeStates.Def(), true)
				.Add(new AttackStates.Def(), can_burrow)
				.PushInterruptGroup()
				.Add(new CreatureSleepStates.Def(), true)
				.Add(new FixedCaptureStates.Def(), true)
				.Add(new RanchedStates.Def(), true)
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.WantsToEnterBurrow, false, "hide", STRINGS.CREATURES.STATUSITEMS.BURROWING.NAME, STRINGS.CREATURES.STATUSITEMS.BURROWING.TOOLTIP), can_burrow)
				.Add(new LayEggStates.Def(), true)
				.Add(new EatStates.Def(), true)
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true)
				.Add(new CallAdultStates.Def(), true)
				.PopInterruptGroup()
				.Add(new IdleStates.Def(), true);

			EntityTemplates.AddCreatureBrain(prefab, chore_table, GameTags.Creatures.Species.HatchSpecies, symbolOverridePrefix);

			return prefab;
		}

		public static List<Diet.Info> BasicDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
		{
			return new List<Diet.Info>
			{
				new Diet.Info(
					consumed_tags: new HashSet<Tag> { ElementLoader.FindElementByName("Slag").tag },
					produced_element: poopTag,
					calories_per_kg: caloriesPerKg,
					produced_conversion_rate: producedConversionRate,
					disease_id: diseaseId,
					disease_per_kg_produced: diseasePerKgProduced,
					produce_solid_tile: false,
					eats_plants_directly: false)
			};
		}
		public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos, float referenceCaloriesPerKg, float minPoopSizeInKg)
		{
			Diet diet = new Diet(diet_infos.ToArray());

			var calorie_monitor = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
			calorie_monitor.diet = diet;
			calorie_monitor.minPoopSizeInCalories = referenceCaloriesPerKg * minPoopSizeInKg;

			prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

			return prefab;
		}

		private static int AdjustSpawnLocationCB(int cell)
		{
			while (!Grid.Solid[cell])
			{
				int cell_below = Grid.CellBelow(cell);
				if (!Grid.IsValidCell(cell))
				{
					break;
				}
				cell = cell_below;
			}
			return cell;
		}
	}
}

