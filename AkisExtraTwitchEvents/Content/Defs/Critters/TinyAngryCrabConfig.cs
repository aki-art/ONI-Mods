using Klei.AI;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Critters
{
	public class TinyAngryCrabConfig : EntityConfigBase
	{
		public const string ID = "AkisExtraTwitchEvents_TinyAngryCrab";
		public const string BASE_TRAIT_ID = "AkisExtraTwitchEvents_TinyAngryCrabOriginal";

		private const float KG_ORE_EATEN_PER_CYCLE = 50f;
		private static readonly float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;
		private const float MIN_POOP_SIZE_IN_KG = 1f;
		public static float STANDARD_CALORIES_PER_CYCLE = 1_000f;

		public override GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Tiny Angry Pokeshell",
				"",
				10f,
				Assets.GetAnim("pincher_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				1,
				1,
				TUNING.DECOR.NONE);

			EntityTemplates.ExtendEntityToBasicCreature(
				prefab,
				FactionManager.FactionID.Hostile,
				BASE_TRAIT_ID,
				CONSTS.NAV_GRID.WALKER_1X2,
				NavType.Floor,
				moveSpeed: 4f,
				onDeathDropID: CrabShellConfig.ID,
				onDeathDropCount: 1,
				drownVulnerable: false,
				entombVulnerable: false,
				warningLowTemperature: 288.15f,
				warningHighTemperature: 343.15f,
				lethalHighTemperature: 373.15f);

			prefab.AddOrGet<Trappable>();
			prefab.AddOrGet<LoopingSounds>();
			prefab.AddOrGetDef<CreatureFallMonitor.Def>();

			var def = prefab.AddOrGetDef<ThreatMonitor.Def>();
			def.fleethresholdState = Health.HealthState.Dead;
			def.friendlyCreatureTags = [GameTags.Creatures.CrabFriend];
			def.maxSearchDistance = 12;

			EntityTemplates.CreateAndRegisterBaggedCreature(prefab, true, true);
			EntityTemplates.ExtendEntityToWildCreature(prefab, 4);

			var kPrefabId = prefab.GetComponent<KPrefabID>();
			kPrefabId.AddTag(GameTags.Creatures.Walker);
			kPrefabId.AddTag(GameTags.Creatures.CrabFriend);
			kPrefabId.AddTag(GameTags.Amphibious);

			if (prefab.TryGetComponent(out Butcherable butcherable))
				butcherable.SetDrops([ShellfishMeatConfig.ID, BabyCrabShellConfig.ID]);

			ConfigureBrain(prefab);
			ConfigureTraits();
			ConfigureDiet(prefab);

			prefab.AddComponent<TinyCrab>();
			prefab.AddComponent<Prioritizable>();
			prefab.AddWeapon(0.05f, 0.3f);

			return prefab;
		}

		private void ConfigureDiet(GameObject placedEntity)
		{
			var dietInfo = BaseCrabConfig.BasicDiet(
				SimHashes.Sand.CreateTag(),
				CALORIES_PER_KG_OF_ORE,
				TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL,
				null,
				0.0f);

			BaseCrabConfig.SetupDiet(
				placedEntity,
				dietInfo,
				CALORIES_PER_KG_OF_ORE,
				MIN_POOP_SIZE_IN_KG);
		}

		private void ConfigureTraits()
		{
			string name = STRINGS.CREATURES.SPECIES.AKISEXTRATWITCHEVENTS_GIANTCRAB.NAME;
			var trait = Db.Get().CreateTrait(
				BASE_TRAIT_ID,
				name,
				name,
				null,
				false,
				null,
				true,
				true);

			var amounts = Db.Get().Amounts;
			trait.Add(new AttributeModifier(amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE * 10f, name));
			trait.Add(new AttributeModifier(amounts.Calories.deltaAttribute.Id, (float)(-STANDARD_CALORIES_PER_CYCLE / CONSTS.CYCLE_LENGTH), name));
			trait.Add(new AttributeModifier(amounts.HitPoints.maxAttribute.Id, 10f, name));
			trait.Add(new AttributeModifier(amounts.Age.maxAttribute.Id, 1f, name));
			trait.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, 100f, name));
			trait.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, name));
		}

		private void ConfigureBrain(GameObject prefab)
		{
			var choreTable = new ChoreTable.Builder()
				.Add(new DeathStates.Def())
				.Add(new AnimInterruptStates.Def())
				.Add(new TrappedStates.Def())
				.Add(new BaggedStates.Def())
				.Add(new FallStates.Def())
				.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				//.Add(new DefendStates.Def())
				.Add(new AttackStates.Def())
				.PushInterruptGroup()
				.Add(new CreatureSleepStates.Def())
				.Add(new FixedCaptureStates.Def())
				.Add(new EatStates.Def())
				.Add(new PlayAnimsStates.Def(
					GameTags.Creatures.Poop,
					false,
					"poop",
					global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME,
					global::STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
				.PopInterruptGroup()
				.Add(new IdleStates.Def());

			EntityTemplates.AddCreatureBrain(prefab, choreTable, GameTags.Creatures.Species.CrabSpecies, "");
		}
	}
}
