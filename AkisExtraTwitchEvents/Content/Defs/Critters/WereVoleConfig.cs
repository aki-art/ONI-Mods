using FUtility;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.WereVoleScripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Critters
{
	internal class WereVoleConfig : IEntityConfig
	{
		public const string ID = "AETE_WereVole";
		public const string BASE_TRAIT_ID = "AETE_WereVoleOriginal";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				ID,
				"Were Vole",
				"placeholder",
				100f,
				true,
				Assets.GetAnim("aete_werevole_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				additionalTags: new()
				{
					GameTags.Creature,
					GameTags.DupeBrain,
					ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
				});

			prefab.AddOrGet<RegularPip>();

			var kbac = prefab.GetComponent<KBatchedAnimController>();
			kbac.isMovable = true;
			kbac.SetSymbolVisiblity("snapto_tag", false);

			// traits and modifiers
			var modifiers = prefab.AddOrGet<Modifiers>();
			modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Digging.Id);
			//modifiers.initialAttributes.Add(Db.Get().Attributes.CarryAmount.Id);

			prefab.AddOrGet<Traits>();
			prefab.AddOrGet<Effects>();

			ConfigureTraits(modifiers);

			prefab.AddOrGet<AttributeConverters>();

			prefab.AddOrGet<Worker>();

			var moverLayerOccupier = prefab.AddOrGet<MoverLayerOccupier>();
			moverLayerOccupier.objectLayers = new[]
			{
				ObjectLayer.Mover
			};
			moverLayerOccupier.cellOffsets = new[]
			{
				  CellOffset.none,
			};

			// inventory
			var storage = prefab.AddOrGet<Storage>();
			storage.fxPrefix = Storage.FXPrefix.PickedUp;
			storage.dropOnLoad = true;
			storage.SetDefaultStoredItemModifiers(new()
			{
			  Storage.StoredItemModifier.Preserve,
			  Storage.StoredItemModifier.Seal
			});

			// Ctrl + Q go here command
			prefab.AddOrGetDef<CreatureDebugGoToMonitor.Def>();

			// grid drevealer
			var gridVisibility = prefab.AddOrGet<GridVisibility>();
			gridVisibility.radius = 10;
			gridVisibility.innerRadius = 5f;

			prefab.AddOrGet<AnimEventHandler>();
			prefab.AddOrGet<Health>();

			prefab.AddOrGetDef<RegularPipAi.Def>();

			CreateBrain(prefab);
			ConfigureNavigation(prefab);

			// collider
			prefab.AddOrGet<KBoxCollider2D>().size = new(1, 1);

			// nameable
			prefab.AddOrGet<UserNameable>();
			prefab.AddOrGet<CharacterOverlay>().shouldShowName = true;

			prefab.AddOrGetDef<CreatureFallMonitor.Def>();

			prefab.AddOrGet<Sensors>();
			prefab.AddOrGet<Pickupable>().SetWorkTime(5f);

			prefab.AddOrGet<SnapOn>();

			ConfigureLaserEffects(prefab);

			SymbolOverrideControllerUtil.AddToPrefab(prefab);

			/// <see cref="Patches.ScheduleManagerPatch.ScheduleManager_OnSpawn_Patch"/>
			prefab.AddComponent<Schedulable>();

			prefab.AddComponent<MinionStorage>();
			prefab.AddComponent<WereVoleContainer>();

			return prefab;
		}

		private void ConfigureLaserEffects(GameObject prefab)
		{
			var laserGo = new GameObject("LaserEffect");
			laserGo.transform.parent = prefab.transform;

			var animEventToggler = laserGo.AddComponent<KBatchedAnimEventToggler>();
			animEventToggler.eventSource = prefab;
			animEventToggler.enableEvent = GameHashes.LaserOn.ToString();
			animEventToggler.disableEvent = GameHashes.LaserOff.ToString();
			animEventToggler.entries = new();

			var laserEffectArray = new[]
			{
				new BaseRoverConfig.LaserEffect()
				{
					id = "DigEffect",
					animFile = "laser_kanim",
					anim = "idle",
					context = "dig"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "BuildEffect",
					animFile = "construct_beam_kanim",
					anim = "loop",
					context = "build"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "FetchLiquidEffect",
					animFile = "hose_fx_kanim",
					anim = "loop",
					context = "fetchliquid"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "PaintEffect",
					animFile = "paint_beam_kanim",
					anim = "loop",
					context = "paint"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "HarvestEffect",
					animFile = "plant_harvest_beam_kanim",
					anim = "loop",
					context = "harvest"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "CaptureEffect",
					animFile = "net_gun_fx_kanim",
					anim = "loop",
					context = "capture"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "AttackEffect",
					animFile = "attack_beam_fx_kanim",
					anim = "loop",
					context = "attack"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "PickupEffect",
					animFile = "vacuum_fx_kanim",
					anim = "loop",
					context = "pickup"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "StoreEffect",
					animFile = "vacuum_reverse_fx_kanim",
					anim = "loop",
					context = "store"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "DisinfectEffect",
					animFile = "plant_spray_beam_kanim",
					anim = "loop",
					context = "disinfect"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "TendEffect",
					animFile = "plant_tending_beam_fx_kanim",
					anim = "loop",
					context = "tend"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "PowerTinkerEffect",
					animFile = "electrician_beam_fx_kanim",
					anim = "idle",
					context = "powertinker"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "SpecialistDigEffect",
					animFile = "senior_miner_beam_fx_kanim",
					anim = "idle",
					context = "specialistdig"
				},
				new BaseRoverConfig.LaserEffect()
				{
					id = "DemolishEffect",
					animFile = "poi_demolish_fx_kanim",
					anim = "idle",
					context = "demolish"
				}
			};

			var kbac = prefab.GetComponent<KBatchedAnimController>();

			foreach (var laserEffect in laserEffectArray)
			{
				var go = new GameObject(laserEffect.id);
				go.transform.parent = laserGo.transform;
				go.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);

				var kbatchedAnimTracker = go.AddOrGet<KBatchedAnimTracker>();
				kbatchedAnimTracker.controller = kbac;
				kbatchedAnimTracker.symbol = new HashedString("snapto_tag");
				kbatchedAnimTracker.offset = new Vector3(40f, 0.0f, 0.0f);
				kbatchedAnimTracker.useTargetPoint = true;

				var kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
				kbatchedAnimController.AnimFiles = new[]
				{
					Assets.GetAnim(laserEffect.animFile)
				};

				var entry = new KBatchedAnimEventToggler.Entry()
				{
					anim = laserEffect.anim,
					context = laserEffect.context,
					controller = kbatchedAnimController
				};

				animEventToggler.entries.Add(entry);
				go.AddOrGet<LoopingSounds>();
			}
		}

		private void ConfigureNavigation(GameObject prefab)
		{
			var navigator = prefab.AddOrGet<Navigator>();
			navigator.NavGridName = CONSTS.NAV_GRID.DIGGER;
			navigator.CurrentNavType = NavType.Floor;
			navigator.defaultSpeed = 2f;
			navigator.updateProber = true;
			navigator.sceneLayer = Grid.SceneLayer.Creatures;
		}

		private void CreateBrain(GameObject prefab)
		{
			var choreTable = new ChoreTable.Builder()
				.Add(new RobotDeathStates.Def())
				.Add(new FallStates.Def())
				.Add(new DrowningStates.Def())
				.Add(new DebugGoToStates.Def())
				.Add(new GoHomeStates.Def())
				.Add(new EatStates.Def())
				.Add(new IdleStates.Def(), forcePriority: Db.Get().ChoreTypes.Idle.priority);

			EntityTemplates.AddCreatureBrain(prefab, choreTable, GameTags.Creatures.Species.MoleSpecies, null);
		}

		private void ConfigureTraits(Modifiers modifiers)
		{
			var name = global::STRINGS.CREATURES.SPECIES.MOLE.NAME.ToString();

			var trait = Db.Get().CreateTrait(
				BASE_TRAIT_ID,
				name,
				name,
				null,
				false,
				GetDisabledChoreGroups(),
				true,
				true);

			var attributes = Db.Get().Attributes;
			trait.Add(new AttributeModifier(attributes.Digging.Id, 20, name));
			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 100f, name));
			//trait.Add(new AttributeModifier(attributes.CarryAmount.Id, 200f, name));

			modifiers.initialTraits.Add(BASE_TRAIT_ID);
		}

		private ChoreGroup[] GetDisabledChoreGroups()
		{
			var choreGroups = Db.Get().ChoreGroups;
			var enabledChoreGroups = new List<ChoreGroup>()
			{
				//choreGroups.Hauling,
				//choreGroups.Storage,
				choreGroups.Dig
			};

			return choreGroups.resources.Where(c => !enabledChoreGroups.Contains(c)).ToArray();
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
			var consumer = inst.GetComponent<ChoreConsumer>();
			if (consumer != null)
				consumer.AddProvider(GlobalChoreProvider.Instance);
		}

		public void OnSpawn(GameObject inst)
		{
			var consumableConsumer = inst.AddOrGet<ConsumableConsumer>();
			foreach (var food in EdiblesManager.GetAllFoodTypes())
			{
				consumableConsumer.SetPermitted(food.Id, food.Id == MushBarConfig.ID);
			}

			// sense stuff
			var sensors = inst.GetComponent<Sensors>();
			//sensors.Add(new PathProberSensor(sensors));
			sensors.Add(new PickupableSensor(sensors));

			// navigation
			var navigator = inst.GetComponent<Navigator>();
			navigator.transitionDriver.overrideLayers.Add(new NavTeleportTransitionLayer(navigator));
			navigator.transitionDriver.overrideLayers.Add(new SplashTransitionLayer(navigator));
			navigator.CurrentNavType = NavType.Floor;
			navigator.SetFlags(PathFinder.PotentialPath.Flags.None);

			var pathProber = inst.GetComponent<PathProber>();
			if (pathProber != null)
				pathProber.SetGroupProber(MinionGroupProber.Get());

		}
	}
}
