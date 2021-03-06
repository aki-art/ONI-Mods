﻿using FUtility;
using TUNING;
using UnityEngine;

namespace TransparentAluminium
{
	class SolarPanelRoadConfig : IBuildingConfig, IModdedBuilding
	{
		public const string ID = "TAT_SolarPanelRoad";
		public const float WATTS_PER_LUX = 0.00053f;
		public const float MAX_WATTS = 380f;

		public MBInfo Info => new MBInfo(ID, "Base");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "farmtilerotating_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				construction_mass: new float[] { 100f, 50f, 50f },
				construction_materials: new string[] { "Transparent", "RefinedMetal", "Ceramic" },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NONE
				);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.GeneratorWattageRating = 999999f;
			def.GeneratorBaseCapacity = def.GeneratorWattageRating;
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

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
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

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			var kprefabID = go.GetComponent<KPrefabID>();

			var solarUpgrades = go.AddOrGet<UpgradeableSolarPanel>();
			solarUpgrades.powerDistributionOrder = 9;
			solarUpgrades.maxWatts = 100;
			solarUpgrades.wattPerLux = 0.00053f / 8f;

			kprefabID.AddTag(GameTags.FloorTiles);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
			go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
