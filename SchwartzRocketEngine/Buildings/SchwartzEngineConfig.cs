using TUNING;
using UnityEngine;

namespace SchwartzRocketEngine.Buildings
{
    internal class SchwartzEngineConfig : IBuildingConfig
	{
		public static readonly string ID = Mod.Prefix("SchwartzEngine");
		public override string[] GetDlcIds() => DlcManager.AVAILABLE_VANILLA_ONLY;

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
					SimHashes.Steel.ToString(),
					SimHashes.Rust.ToString()
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
			def.RequiresPowerInput = false;
			def.RequiresPowerOutput = false;
			def.CanMove = true;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.AddOrGet<LoopingSounds>();
			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
			{
				new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
			};
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
			rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
			rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
			rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
			rocketEngine.exhaustElement = SimHashes.Steam;
			rocketEngine.exhaustTemperature = 2000f;
			BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_hydrogen_engine_bg_kanim", false);
		}
	}
}
