using Klei.AI;
using UnityEngine;

namespace Twitchery.Content.Defs.Critters
{
	public class SmallBeeConfig : EntityConfigBase
	{
		public const string ID = "AkisExtraTwitchEvents_SmallBee";
		public const string BASE_TRAIT_ID = "AkisExtraTwitchEvents_SmallBeeOriginal";


		public override GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Bee",
				"",
				10f,
				Assets.GetAnim("aete_bee_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				1,
				1,
				TUNING.DECOR.NONE);

			EntityTemplates.ExtendEntityToBasicCreature(
				prefab,
				FactionManager.FactionID.Hostile,
				BASE_TRAIT_ID,
				CONSTS.NAV_GRID.FLYER_1X1,
				NavType.Hover,
				moveSpeed: 4f,
				onDeathDropID: null,
				onDeathDropCount: 0,
				drownVulnerable: false,
				entombVulnerable: false,
				warningLowTemperature: 288.15f,
				warningHighTemperature: 343.15f,
				lethalHighTemperature: 373.15f);

			prefab.AddOrGet<Trappable>();
			prefab.AddOrGet<LoopingSounds>();

			var def = prefab.AddOrGetDef<ThreatMonitor.Def>();
			def.fleethresholdState = Health.HealthState.Dead;
			def.friendlyCreatureTags = [GameTags.Creatures.CrabFriend];
			def.maxSearchDistance = 12;

			EntityTemplates.CreateAndRegisterBaggedCreature(prefab, true, true);
			EntityTemplates.ExtendEntityToWildCreature(prefab, 4);

			var kPrefabId = prefab.GetComponent<KPrefabID>();
			kPrefabId.AddTag(GameTags.Creatures.Flyer);

			ConfigureBrain(prefab);
			ConfigureTraits();

			//prefab.AddComponent<TinyBee>();
			prefab.AddComponent<Prioritizable>();
			prefab.AddWeapon(0.05f, 0.3f);

			return prefab;
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
				.Add(new StunnedStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new FleeStates.Def())
				//.Add(new DefendStates.Def())
				.Add(new AttackStates.Def())
				.PushInterruptGroup()
				.Add(new FixedCaptureStates.Def())
				.PopInterruptGroup()
				.Add(new IdleStates.Def());

			EntityTemplates.AddCreatureBrain(prefab, choreTable, GameTags.Creatures.Species.BeetaSpecies, "");
		}
	}
}
