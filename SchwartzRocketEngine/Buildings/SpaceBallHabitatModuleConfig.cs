using TUNING;
using UnityEngine;

namespace SchwartzRocketEngine.Buildings
{
	public class SpaceBallHabitatModuleConfig : IBuildingConfig
    {
        public static readonly string ID = Mod.Prefix("SpaceBallHabitatModule");

		private readonly ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(-1, 0));
		private readonly ConduitPortInfo gasOutputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 0));
		private readonly ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(-1, 1));
		private readonly ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

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

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			go.AddOrGet<LoopingSounds>();

			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			//go.AddTag(GameTags.NoseRocketModule);
			go.AddTag(GameTags.LaunchButtonRocketModule);

			go.AddOrGet<AssignmentGroupController>().generateGroupOnStart = true;
			go.AddOrGet<PassengerRocketModule>().interiorReverbSnapshot = AudioMixerSnapshots.Get().MediumRocketInteriorReverbSnapshot;
            
			ClustercraftExteriorDoor door = go.AddOrGet<ClustercraftExteriorDoor>();
            door.interiorTemplateName = "SchwartzEngine/spaceBallHabitat";//"expansion1::interiors/habitat_small";
			//door.anim = "spaceball_bg_kanim";

			go.AddOrGetDef<SimpleDoorController.Def>();
			go.AddOrGet<NavTeleporter>();
			go.AddOrGet<AccessControl>();
			go.AddOrGet<LaunchableRocketCluster>();
			go.AddOrGet<RocketCommandConditions>();
			go.AddOrGet<RocketProcessConditionDisplayTarget>();
			go.AddOrGet<CharacterOverlay>().shouldShowName = true;

			Storage liquidStorage = go.AddComponent<Storage>();
			liquidStorage.showInUI = false;
			liquidStorage.capacityKg = 10f;

			RocketConduitSender liquidSender = go.AddComponent<RocketConduitSender>();
			liquidSender.conduitStorage = liquidStorage;
			liquidSender.conduitPortInfo = liquidInputPort;

			go.AddComponent<RocketConduitReceiver>().conduitPortInfo = liquidOutputPort;

			Storage gasStorage = go.AddComponent<Storage>();
			gasStorage.showInUI = false;
			gasStorage.capacityKg = 1f;

			RocketConduitSender gasSender = go.AddComponent<RocketConduitSender>();
			gasSender.conduitStorage = gasStorage;
			gasSender.conduitPortInfo = gasInputPort;

			go.AddComponent<RocketConduitReceiver>().conduitPortInfo = gasOutputPort;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR_PLUS);

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

            go.GetComponent<ReorderableBuilding>().buildConditions.Add(new LimitOneCommandModule());
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			base.DoPostConfigurePreview(def, go);
			go.AddOrGet<BuildingCellVisualizer>();
			AttachPorts(go);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<BuildingCellVisualizer>();
			AttachPorts(go);
		}

		private void AttachPorts(GameObject go)
		{
			go.AddComponent<ConduitSecondaryInput>().portInfo = liquidInputPort;
			go.AddComponent<ConduitSecondaryOutput>().portInfo = liquidOutputPort;
			go.AddComponent<ConduitSecondaryInput>().portInfo = gasInputPort;
			go.AddComponent<ConduitSecondaryOutput>().portInfo = gasOutputPort;
		}
	}
}
