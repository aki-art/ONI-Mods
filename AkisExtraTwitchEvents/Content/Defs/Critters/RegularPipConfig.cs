using FUtility;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using TemplateClasses;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Critters
{
	internal class RegularPipConfig : IEntityConfig
	{
		public const string ID = "AETE_RegularPip";
		public const string BASE_TRAIT_ID = "AETE_RegularPipOriginal";
		public const string SCHEDULE_NAME = "(Hidden) Regular Pip schedule";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				ID,
				"Regular Pip",
				STRINGS.DUPLICANTS.REGULAR_PIP_NAMES.DESCRIPTION,
				100f,
				true,
				Assets.GetAnim("aete_regular_pip_kanim"),
				"idle_loop",
				Grid.SceneLayer.Creatures,
				additionalTags: new()
				{
					GameTags.Creature,
					GameTags.DupeBrain,
					ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
				});

			var kbac = prefab.GetComponent<KBatchedAnimController>();
			kbac.isMovable = true;
			kbac.SetSymbolVisiblity("snapto_tag", false);

			// traits and modifiers
			var modifiers = prefab.AddOrGet<Modifiers>();
			modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Construction.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Digging.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.CarryAmount.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Machinery.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Botanist.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.FarmTinker.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Athletics.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Caring.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Learning.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.SpaceNavigation.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Cooking.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.LifeSupport.Id);
			modifiers.initialAttributes.Add(Db.Get().Attributes.Strength.Id);

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
			gridVisibility.radius = 30;
			gridVisibility.innerRadius = 20f;

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

			prefab.AddOrGet<ConsumableConsumer>();
			// still plant seeds lol
			//prefab.AddOrGetDef<SeedPlantingMonitor.Def>();

			prefab.AddOrGet<RegularPipBrain>();
			prefab.AddOrGet<RegularPip>();

			// experimental

			/// <see cref="Patches.ScheduleManagerPatch.ScheduleManager_OnSpawn_Patch"/>
			//prefab.AddComponent<Schedulable>();
			// prefab.AddComponent<WarmBlooded>();

			/*			var oxygenBreather = prefab.AddOrGet<OxygenBreather>();
						oxygenBreather.O2toCO2conversion = 0.02f;
						oxygenBreather.lowOxygenThreshold = 0.12f;
						oxygenBreather.noOxygenThreshold = 0.01f;
						oxygenBreather.mouthOffset = (Vector2)new Vector2f(0.25f, 0.47f);
						oxygenBreather.minCO2ToEmit = 0.005f;
						oxygenBreather.breathableCells = OxygenBreather.DEFAULT_BREATHABLE_OFFSETS;*/

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
				new ScoutRoverConfig.LaserEffect()
				{
					id = "DigEffect",
					animFile = "laser_kanim",
					anim = "idle",
					context = "dig"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "BuildEffect",
					animFile = "construct_beam_kanim",
					anim = "loop",
					context = "build"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "FetchLiquidEffect",
					animFile = "hose_fx_kanim",
					anim = "loop",
					context = "fetchliquid"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "PaintEffect",
					animFile = "paint_beam_kanim",
					anim = "loop",
					context = "paint"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "HarvestEffect",
					animFile = "plant_harvest_beam_kanim",
					anim = "loop",
					context = "harvest"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "CaptureEffect",
					animFile = "net_gun_fx_kanim",
					anim = "loop",
					context = "capture"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "AttackEffect",
					animFile = "attack_beam_fx_kanim",
					anim = "loop",
					context = "attack"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "PickupEffect",
					animFile = "vacuum_fx_kanim",
					anim = "loop",
					context = "pickup"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "StoreEffect",
					animFile = "vacuum_reverse_fx_kanim",
					anim = "loop",
					context = "store"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "DisinfectEffect",
					animFile = "plant_spray_beam_kanim",
					anim = "loop",
					context = "disinfect"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "TendEffect",
					animFile = "plant_tending_beam_fx_kanim",
					anim = "loop",
					context = "tend"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "PowerTinkerEffect",
					animFile = "electrician_beam_fx_kanim",
					anim = "idle",
					context = "powertinker"
				},
				new ScoutRoverConfig.LaserEffect()
				{
					id = "SpecialistDigEffect",
					animFile = "senior_miner_beam_fx_kanim",
					anim = "idle",
					context = "specialistdig"
				},
				new ScoutRoverConfig.LaserEffect()
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
			navigator.NavGridName = Consts.NAV_GRID.PIP;
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
				.Add(new EatStates.Def())
				.Add(new IdleStates.Def(), forcePriority: Db.Get().ChoreTypes.Idle.priority);

			EntityTemplates.AddCreatureBrain(prefab, choreTable, GameTags.Creatures.Species.SquirrelSpecies, null);
		}

		private void ConfigureTraits(Modifiers modifiers)
		{
			var name = global::STRINGS.CREATURES.SPECIES.SQUIRREL.NAME.ToString();

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

			modifiers.initialAttributes.Add(attributes.AirConsumptionRate.Id);
			modifiers.initialAttributes.Add(attributes.CarryAmount.Id);
			modifiers.initialAmounts.Add(Db.Get().Amounts.Calories.Id);

			trait.Add(new AttributeModifier(attributes.CarryAmount.Id, 200f, name));
			trait.Add(new AttributeModifier(attributes.Digging.Id, TUNING.ROBOTS.SCOUTBOT.DIGGING, name));
			trait.Add(new AttributeModifier(attributes.Construction.Id, TUNING.ROBOTS.SCOUTBOT.CONSTRUCTION, name));
			trait.Add(new AttributeModifier(attributes.Athletics.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, name));
			trait.Add(new AttributeModifier(attributes.Botanist.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, name));
			trait.Add(new AttributeModifier(attributes.Machinery.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, name));
			trait.Add(new AttributeModifier(attributes.MachinerySpeed.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, name));
			trait.Add(new AttributeModifier(attributes.Cooking.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, name));
			trait.Add(new AttributeModifier(attributes.LifeSupport.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, name));
			trait.Add(new AttributeModifier(attributes.Strength.Id, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, name));
			trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, TUNING.ROBOTS.SCOUTBOT.HIT_POINTS, name));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -1666.66663f, name));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name));

			// experimental
			//trait.Add(new AttributeModifier(attributes.AirConsumptionRate.Id, 0.025f, name));

			modifiers.initialTraits.Add(BASE_TRAIT_ID);
		}

		private ChoreGroup[] GetDisabledChoreGroups()
		{
			var choreGroups = Db.Get().ChoreGroups;
			var enabledChoreGroups = new List<ChoreGroup>()
			{
				choreGroups.Hauling,
				choreGroups.Storage,
				choreGroups.Dig,
				choreGroups.Build,
				choreGroups.Farming,
				choreGroups.Cook,
				choreGroups.LifeSupport,
				choreGroups.MachineOperating,
				choreGroups.Basekeeping,
				//choreGroups.MedicalAid,
				//choreGroups.Recreation,
				//choreGroups.Research,
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
			foreach(var food in EdiblesManager.GetAllFoodTypes())
			{
				consumableConsumer.SetPermitted(food.Id, food.Id == MushBarConfig.ID);
			}

			// sense stuff
			var sensors = inst.GetComponent<Sensors>();
			//sensors.Add(new PathProberSensor(sensors));
			sensors.Add(new PickupableSensor(sensors));
			sensors.Add(new ClosestEdibleSensor(sensors));
			//sensors.Add(new BreathableAreaSensor(sensors));
			//sensors.Add(new SafeCellSensor(sensors));
			//sensors.Add(new IdleCellSensor(sensors));

			// navigation
			var navigator = inst.GetComponent<Navigator>();
			//navigator.transitionDriver.overrideLayers.Add(new BipedTransitionLayer(navigator, 3.325f, 2.5f));
			navigator.transitionDriver.overrideLayers.Add(new NavTeleportTransitionLayer(navigator));
			navigator.transitionDriver.overrideLayers.Add(new SplashTransitionLayer(navigator));
			//navigator.transitionDriver.overrideLayers.Add(new DoorTransitionLayer(navigator));
			navigator.CurrentNavType = NavType.Floor;
			navigator.SetFlags(PathFinder.PotentialPath.Flags.None);

/*			if (inst.TryGetComponent(out OxygenBreather breather) && breather.GetGasProvider() == null)
				breather.SetGasProvider(new GasBreatherFromWorldProvider());*/

			//navigator.transitionDriver.overrideLayers.Add(new BipedTransitionLayer(navigator, 3.325f, 2.5f));
			//navigator.transitionDriver.overrideLayers.Add(new LadderDiseaseTransitionLayer(navigator));

			var pathProber = inst.GetComponent<PathProber>();
			if (pathProber != null)
				pathProber.SetGroupProber(MinionGroupProber.Get());

		}
	}
}
