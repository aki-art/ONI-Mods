using FUtility;
using Klei.AI;
using System.Collections.Generic;
using TemplateClasses;
using TUNING;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Critters
{
	internal class GiantCrabConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_GiantCrab";
		public const string BASE_TRAIT_ID = "AkisExtraTwitchEvents_GiantCrabOriginal";
		public const int WIDTH = 5;
		public const int HEIGHT = 7;

		private const float KG_ORE_EATEN_PER_CYCLE = 700f;
		private static readonly float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / KG_ORE_EATEN_PER_CYCLE;
		private const float MIN_POOP_SIZE_IN_KG = 250f;
		public static float STANDARD_CALORIES_PER_CYCLE = 500_000f;

		public GameObject CreatePrefab()
		{
			var placedEntity = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.AKISEXTRATWITCHEVENTS_GIANTCRAB.NAME,
				STRINGS.CREATURES.SPECIES.AKISEXTRATWITCHEVENTS_GIANTCRAB.DESCRIPTION,
				10000f,
				Assets.GetAnim("aete_giant_crab_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				WIDTH,
				HEIGHT,
				DECOR.BONUS.TIER5);

			EntityTemplates.ExtendEntityToBasicCreature(
				 placedEntity, FactionManager.FactionID.Friendly,
				  BASE_TRAIT_ID,
				  TNavGrids.GIANT_CRAB_NAV,
				  onDeathDropID: CrabShellConfig.ID,
				  onDeathDropCount: 40,
				  drownVulnerable: false,
				  entombVulnerable: false,
				  warningLowTemperature: 288.15f,
				  warningHighTemperature: 343.15f,
				  lethalHighTemperature: 373.15f);

			placedEntity.AddOrGet<Trappable>();
			placedEntity.AddOrGet<LoopingSounds>();
			placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();

			var def = placedEntity.AddOrGetDef<ThreatMonitor.Def>();
			def.fleethresholdState = Health.HealthState.Dead;
			def.friendlyCreatureTags = new[]
			{
				GameTags.Creatures.CrabFriend
			};

			EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);

			var kPrefabId = placedEntity.GetComponent<KPrefabID>();
			kPrefabId.AddTag(GameTags.Creatures.Walker);
			kPrefabId.AddTag(GameTags.Creatures.CrabFriend);
			kPrefabId.AddTag(GameTags.Amphibious);

			ConfigureBrain(placedEntity);
			ConfigureTraits();
			ConfigureDiet(placedEntity);

			if (placedEntity.TryGetComponent(out Butcherable butcherable))
			{
				var meats = 30;
				var shells = 40;

				var drops = new List<string>();

				for (int i = 0; i < meats; i++)
					drops.Add(ShellfishMeatConfig.ID);

				for (int i = 0; i < shells; i++)
					drops.Add(CrabShellConfig.ID);

				butcherable.SetDrops(drops.ToArray());
			}

			placedEntity.AddComponent<GiantCrab>();

			placedEntity.AddOrGet<UserNameable>();
			placedEntity.AddOrGet<CharacterOverlay>().shouldShowName = true;

			return placedEntity;
		}

		private void ConfigureDiet(GameObject placedEntity)
		{
			var dietInfo = BaseCrabConfig.BasicDiet(
				SimHashes.Sand.CreateTag(),
				CALORIES_PER_KG_OF_ORE,
				CREATURES.CONVERSION_EFFICIENCY.NORMAL,
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
			trait.Add(new AttributeModifier(amounts.HitPoints.maxAttribute.Id, 2500f, name));
			trait.Add(new AttributeModifier(amounts.Age.maxAttribute.Id, float.PositiveInfinity, name));
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

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			inst.TryGetComponent(out KBatchedAnimController kbac);
			kbac.animScale *= 4f;
		}
	}
}
