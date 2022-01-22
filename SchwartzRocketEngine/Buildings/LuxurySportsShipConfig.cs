using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace SchwartzRocketEngine.Buildings
{
	public class LuxurySportsShipConfig : IBuildingConfig
	{
		public static readonly string ID = Mod.Prefix("LuxurySportsShip");

		public override string[] GetDlcIds() => DlcManager.AVAILABLE_EXPANSION1_ONLY;

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				ID,
				3,
				3,
				"rocket_nosecone_kanim",
				BUILDINGS.HITPOINTS.TIER4,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER0,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER4,
				BuildLocationRule.Anywhere,
				BUILDINGS.DECOR.NONE,
				NOISE_POLLUTION.NOISY.TIER2);

			BuildingTemplates.CreateRocketBuildingDef(def);

			def.AttachmentSlotTag = GameTags.Rocket;
			def.SceneLayer = Grid.SceneLayer.Building;
			def.OverheatTemperature = BUILDINGS.OVERHEAT_TEMPERATURES.HIGH_4;
			def.Floodable = false;
			def.ObjectLayer = ObjectLayer.Building;
			def.ForegroundLayer = Grid.SceneLayer.Front;
			def.RequiresPowerInput = false;
			def.attachablePosition = new CellOffset(0, 0);
			def.CanMove = true;
			def.Cancellable = false;
			def.ShowInBuildMenu = false;
			def.UtilityInputOffset = new CellOffset(2, 3);
			def.InputConduitType = ConduitType.Liquid;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			go.AddOrGet<LoopingSounds>();

			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			go.AddTag(GameTags.NoseRocketModule);
			go.AddTag(GameTags.LaunchButtonRocketModule);

			go.AddOrGet<AssignmentGroupController>().generateGroupOnStart = true;
			go.AddOrGet<PassengerRocketModule>().interiorReverbSnapshot = AudioMixerSnapshots.Get().MediumRocketInteriorReverbSnapshot;
			go.AddOrGet<ClustercraftExteriorDoor>().interiorTemplateName = "SchwartzEngine/spaceBallHabitat";//"expansion1::interiors/habitat_small";
			go.AddOrGetDef<SimpleDoorController.Def>();
			go.AddOrGet<NavTeleporter>();
			go.AddOrGet<AccessControl>();
			go.AddOrGet<LaunchableRocketCluster>();
			go.AddOrGet<RocketCommandConditions>();
			go.AddOrGet<RocketProcessConditionDisplayTarget>();
			go.AddOrGet<CharacterOverlay>().shouldShowName = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
        {
            BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR_PLUS);

            AddEngine(go);
            AddFuelTank(go);

            Ownable ownable = go.AddOrGet<Ownable>();
            ownable.slotID = Db.Get().AssignableSlots.HabitatModule.Id;
            ownable.canBePublic = false;

            FakeFloorAdder fakeFloorAdder = go.AddOrGet<FakeFloorAdder>();
            fakeFloorAdder.floorOffsets = new CellOffset[]
            {
                new CellOffset(-1, -1),
                new CellOffset(0, -1),
                new CellOffset(1, -1)
            };
            fakeFloorAdder.initiallyActive = false;

            go.AddOrGet<BuildingCellVisualizer>();

            ConfigurePlaceConditions(go);
        }

        private static void ConfigurePlaceConditions(GameObject go)
        {
            ReorderableBuilding reorderableBuilding = go.GetComponent<ReorderableBuilding>();
            reorderableBuilding.buildConditions.Add(new LimitOneCommandModule());
            reorderableBuilding.buildConditions.Add(new TopOnly());
        }

        private static void AddEngine(GameObject go)
        {
            RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
            rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Petroleum).tag;
            rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.WEAK;
            rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
            rocketEngine.exhaustElement = SimHashes.CarbonDioxide;
            rocketEngine.exhaustTemperature = 500f;
        }

        private void AddFuelTank(GameObject go)
        {
			Storage storage = go.AddOrGet<Storage>();
			storage.capacityKg = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
			storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
			{
				Storage.StoredItemModifier.Hide,
				Storage.StoredItemModifier.Seal,
				Storage.StoredItemModifier.Insulate
			});

			FuelTank fuelTank = go.AddOrGet<FuelTank>();
			fuelTank.consumeFuelOnLand = !DlcManager.FeatureClusterSpaceEnabled();
			fuelTank.storage = storage;
			fuelTank.physicalFuelCapacity = storage.capacityKg;

			go.AddOrGet<CopyBuildingSettings>();

			go.AddOrGet<DropToUserCapacity>();

			ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKG.SetStorage(storage);
			manualDeliveryKG.refillMass = storage.capacityKg;
			manualDeliveryKG.capacity = storage.capacityKg;
			manualDeliveryKG.operationalRequirement = FetchOrder2.OperationalRequirement.None;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;

			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Liquid;
			conduitConsumer.consumptionRate = 10f;
			conduitConsumer.capacityTag = GameTags.Liquid;
			conduitConsumer.capacityKG = storage.capacityKg;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			base.DoPostConfigurePreview(def, go);
			go.AddOrGet<BuildingCellVisualizer>();
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<BuildingCellVisualizer>();
		}
	}
}
