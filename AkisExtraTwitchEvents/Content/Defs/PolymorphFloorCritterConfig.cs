using FUtility;
using Klei.AI;
using TUNING;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class PolymorphFloorCritterConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_PolymorphCritter";
		public const string BASE_TRAIT_ID = "AkisExtraTwitchEvents_PolymorphCritterOriginal";

		public GameObject CreatePrefab()
		{
			var placedEntity = EntityTemplates.CreatePlacedEntity(
				ID,
				STRINGS.CREATURES.SPECIES.AKISEXTRATWITCHEVENTS_POLYMORPHCRITTER.NAME,
				"...",
				30f,
				Assets.GetAnim("squirrel_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				1,
				1,
				DECOR.BONUS.TIER1,
				NOISE_POLLUTION.NONE);

			var kbac = placedEntity.GetComponent<KBatchedAnimController>();
			kbac.isMovable = true;
			kbac.sceneLayer = Grid.SceneLayer.Move;

			placedEntity.AddOrGet<Pickupable>();
			placedEntity.AddOrGet<Clearable>().isClearable = false;

			placedEntity.AddOrGet<LoopingSounds>();

			Modifiers(placedEntity);
			Collider(placedEntity);
			Traits(placedEntity);

			placedEntity.AddOrGet<AttributeLevels>();
			placedEntity.AddOrGet<AttributeConverters>();

			var gridVisibility = placedEntity.AddOrGet<GridVisibility>();
			gridVisibility.radius = 30;
			gridVisibility.innerRadius = 20f;

			placedEntity.AddOrGet<Effects>();
			placedEntity.AddOrGet<AnimEventHandler>();
			placedEntity.AddOrGet<Health>();

			MoverLayer(placedEntity);

			placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();

			var navigator = placedEntity.AddOrGet<Navigator>();
			navigator.NavGridName = Consts.NAV_GRID.WALKER_1X2;
			navigator.CurrentNavType = NavType.Floor;

			placedEntity.AddOrGet<Chattable>();

			var choreTable = new ChoreTable.Builder()
				.Add(new DeathStates.Def())
				.Add(new FallStates.Def())
				.Add(new DebugGoToStates.Def())
				//.PushInterruptGroup()
				.Add(new IdleStates.Def()); //, forcePriority: Db.Get().ChoreTypes.Idle.priority);

			EntityTemplates.AddCreatureBrain(placedEntity, choreTable, GameTags.Creatures.Species.CrabSpecies, null);

			placedEntity.AddOrGet<CharacterOverlay>().shouldShowName = true;
			placedEntity.AddOrGet<FactionAlignment>().Alignment = FactionManager.FactionID.Duplicant;

			placedEntity.AddTag(GameTags.CreatureBrain);
			placedEntity.AddTag(GameTags.Creatures.Walker);

			placedEntity.AddOrGet<MinionStorage>();
			placedEntity.AddOrGet<AETE_PolymorphCritter>();

			return placedEntity;
		}

		private static void MoverLayer(GameObject placedEntity)
		{
			var moverLayerOccupier = placedEntity.AddOrGet<MoverLayerOccupier>();
			moverLayerOccupier.objectLayers = new[]
			{
				ObjectLayer.Minion,
				ObjectLayer.Mover
			};

			moverLayerOccupier.cellOffsets = new[]
			{
				CellOffset.none,
				new CellOffset(0, 1)
			};
		}

		private static void Modifiers(GameObject placedEntity)
		{
			var modifiers = placedEntity.AddOrGet<Modifiers>();
			modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
		}

		private static void Collider(GameObject placedEntity)
		{
			var collider = placedEntity.AddOrGet<KBoxCollider2D>();
			collider.size = new Vector2(1f, 2f);
			collider.offset = new Vector2f(0f, 1f);
		}

		private static void Traits(GameObject placedEntity)
		{
			placedEntity.AddOrGet<Traits>();
			var trait = Db.Get().CreateTrait(
				BASE_TRAIT_ID,
				"Polymorph",
				"",
				null,
				false,
				null,
				true,
				true);

			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 100f, "Polymorph"));
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
