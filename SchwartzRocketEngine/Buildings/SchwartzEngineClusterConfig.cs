using TUNING;
using UnityEngine;

namespace SchwartzRocketEngine.Buildings
{
    public class SchwartzEngineClusterConfig : IBuildingConfig
    {
		public static readonly string ID = Mod.Prefix("SchwartzEngineCluster");

        public override string[] GetDlcIds() => DlcManager.AVAILABLE_EXPANSION1_ONLY;

        public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                12,
                5,
				"rocket_cluster_hydrogen_engine_kanim",
                BUILDINGS.HITPOINTS.TIER4,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_LARGE,
				new string[]
				{
					SimHashes.Steel.ToString()
				},
                BUILDINGS.MELTING_POINT_KELVIN.TIER4,
				BuildLocationRule.Anywhere,
                BUILDINGS.DECOR.NONE,
				NOISE_POLLUTION.NOISY.TIER2);
			
			BuildingTemplates.CreateRocketBuildingDef(def);

			def.SceneLayer = Grid.SceneLayer.Building;
			def.OverheatTemperature = BUILDINGS.OVERHEAT_TEMPERATURES.HIGH_4;
			def.Floodable = false;
			def.AttachmentSlotTag = GameTags.Rocket;
			def.ObjectLayer = ObjectLayer.Building;
			def.attachablePosition = new CellOffset(0, 0);
			def.GeneratorWattageRating = 600f;
			def.GeneratorBaseCapacity = 40000f;
			def.RequiresPowerInput = false;
			def.RequiresPowerOutput = false;
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
			go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
			{
				new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
			};
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
			rocketEngineCluster.maxModules = 7;
			rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.VERY_TALL;
			rocketEngineCluster.fuelTag = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
			rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
			rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
			rocketEngineCluster.exhaustElement = SimHashes.Steam;
			rocketEngineCluster.exhaustTemperature = 2000f;

			go.AddOrGet<ModuleGenerator>();

			BuildingTemplates.ExtendBuildingToRocketModuleCluster(go,
                null,
                ROCKETRY.BURDEN.MAJOR_PLUS,
                ROCKETRY.ENGINE_POWER.LATE_VERY_STRONG,
                ROCKETRY.FUEL_COST_PER_DISTANCE.HIGH);
		}
	}
}
