using Klei.AI;
using System;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Critters
{
	[EntityConfigOrder(0)]
	public class MagicalFloxConfig : IEntityConfig, IHasDlcRestrictions
	{
		public const string ID = "AkisExtraTwitchEvents_MagicalFlox";
		public const string BASE_TRAIT_ID = "AkisExtraTwitchEvents_MagicalFloxOriginal";
		public GameObject CreatePrefab()
		{
			var name = "Magical Flox";
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				name,
				"desc1",
				100f,
				Assets.GetAnim("ice_floof_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				1,
				2,
				TUNING.DECOR.BONUS.TIER5);

			EntityTemplates.ExtendEntityToBasicCreature(
				prefab,
				FactionManager.FactionID.Pest,
				BASE_TRAIT_ID,
				CONSTS.NAV_GRID.WALKER_1X2,
				entombVulnerable: false,
				warningLowTemperature: 243.15f,
				warningHighTemperature: 283.15f,
				lethalLowTemperature: 213.15f,
				lethalHighTemperature: 373.15f);

			prefab.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["WoodDeer"];
			prefab.AddOrGet<Trappable>();
			prefab.AddOrGet<LoopingSounds>();
			prefab.AddOrGetDef<CreatureFallMonitor.Def>();
			prefab.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
			prefab.AddWeapon(1f, 1f);

			EntityTemplates.CreateAndRegisterBaggedCreature(prefab, true, true);

			if (prefab.TryGetComponent(out KPrefabID kPrefabId))
			{
				kPrefabId.AddTag(GameTags.Creatures.Walker);
				kPrefabId.prefabInitFn += inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
			}

			ChoreTable.Builder chore_table = new ChoreTable.Builder()
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				.Add(new TrappedStates.Def())
				.Add(new BaggedStates.Def())
				.Add(new FallStates.Def())
				.Add(new StunnedStates.Def())
				.Add(new DrowningStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				.Add(new AttackStates.Def())
				.PushInterruptGroup()
				.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new EatStates.Def())
				.Add(new DrinkMilkStates.Def())
				.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", (string)global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string)global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.Add(new CritterCondoStates.Def())
				.PopInterruptGroup()
				.Add(new IdleStates.Def());

			EntityTemplates.AddCreatureBrain(prefab, chore_table, GameTags.Creatures.Species.DeerSpecies, null);

			EntityTemplates.ExtendEntityToWildCreature(prefab, DeerTuning.PEN_SIZE_PER_CREATURE);

			var trait = Db.Get().CreateTrait(BASE_TRAIT_ID, name, name, null, false, null, true, true);

			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.666672f, (string)global::STRINGS.UI.TOOLTIPS.BASE_VALUE));
			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, float.PositiveInfinity, name));

			Diet.Info[] diets =
			[
				BaseDeerConfig.CreateDietInfo(
					HardSkinBerryPlantConfig.ID,
					SimHashes.Dirt.CreateTag(),
					WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG,
					WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER,
					null,
					0.0f),
				new Diet.Info(
					[HardSkinBerryConfig.ID],
					SimHashes.Dirt.CreateTag(),
					(float) ( WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * (double) WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG / 1.0),
					WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 3f),
				BaseDeerConfig.CreateDietInfo(
						PrickleFlowerConfig.ID,
						SimHashes.Dirt.CreateTag(),
						WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 2f,
						WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER,
						null,
						0.0f),
				new Diet.Info(
					[PrickleFruitConfig.ID],
					SimHashes.Dirt.CreateTag(),
					(float) ( WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * (double) WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 1.0),
					WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 6f)
			];

			BaseDeerConfig.SetupDiet(prefab, diets, WoodDeerConfig.MIN_KG_CONSUMED_BEFORE_POOPING);
			prefab.AddTag(GameTags.OriginalCreature);

			var def = prefab.AddOrGetDef<WellFedShearable.Def>();
			def.effectId = "WoodDeerWellFed";
			def.caloriesPerCycle = 100000f;
			def.growthDurationCycles = WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;
			def.dropMass = WoodDeerConfig.WOOD_MASS_PER_ANTLER;
			def.itemDroppedOnShear = WoodLogConfig.TAG;
			def.levelCount = 6;

			var magic = prefab.AddComponent<MagicalFlox>();
			magic.radius = 2;
			magic.cellsPerUpdate = 4;
			magic.temperatureShift = -10f;

			var light = prefab.AddComponent<Light2D>();
			light.Color = new Color(0.8f, 1.0f, 1.3f);
			light.Range = 2;
			light.Lux = 600;
			light.shape = LightShape.Circle;
			light.drawOverlay = false;
			light._offset = new Vector2(0, 0.5f);

			return prefab;
		}

		public string[] GetRequiredDlcIds() => DlcManager.DLC2;

		public string[] GetForbiddenDlcIds() => null;

		[Obsolete]
		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
