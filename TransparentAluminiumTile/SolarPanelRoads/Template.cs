using TUNING;
using UnityEngine;

namespace TransparentAluminium.SolarPanelRoads
{
	public class Template
	{
		public static BuildingDef CreateBuildingDef(string ID, float[] masses, string[] materials, string anim, float wattage)
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: anim + "_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				construction_mass: masses,
				construction_materials: materials,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NONE
				);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.GeneratorWattageRating = wattage;
			def.GeneratorBaseCapacity = wattage;
			def.ExhaustKilowattsWhenActive = 0.0f;
			def.SelfHeatKilowattsWhenActive = 0.0f;

			def.Entombable = false;
			def.Overheatable = false;
			def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
			def.AudioCategory = "HollowMetal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			def.PermittedRotations = PermittedRotations.FlipH;
			def.isSolidTile = false;
			def.DragBuild = true;

			return def;
		}

		public static void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.strengthMultiplier = 10f;
			simCellOccupier.notifyOnMelt = true;

			go.AddOrGet<AnimTileable>();
			go.AddOrGet<TileTemperature>();
			Prioritizable.AddRef(go);
		}

		public static void DoPostConfigureComplete(GameObject go, int level, Tag targetUpgrade)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			var kprefabID = go.GetComponent<KPrefabID>();

			var solarUpgrades = go.AddOrGet<UpgradeableSolarPanel>();
			solarUpgrades.powerDistributionOrder = 9;
			solarUpgrades.maxWatts = Tuning.GetLeveledMaxWatt(level);
			solarUpgrades.wattPerLux = Tuning.GetLeveledWPL(level); ;
			solarUpgrades.targetUpgrade = targetUpgrade;

			kprefabID.AddTag(GameTags.FloorTiles);
		}

		public static void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddOrGet<KAnimGridTileVisualizer>();
			go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
