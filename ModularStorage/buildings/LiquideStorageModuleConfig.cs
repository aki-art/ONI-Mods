using FUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace ModularStorage.buildings
{
    class LiquideStorageModuleConfig : IBuildingConfig, IModdedBuilding
	{
		public static string ID = "MS_LiquideStorageModule";
		public MBInfo Info => new MBInfo(ID, null);

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"ladder_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.Tile,
				BUILDINGS.DECOR.PENALTY.TIER1,
				NOISE_POLLUTION.NONE);

			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.Overheatable = false;
			buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
			buildingDef.isSolidTile = true;
			buildingDef.IsFoundation = true;
			buildingDef.DragBuild = true;
			buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			go.AddComponent<StorageModule>();
			go.AddComponent<Storage>();
			go.AddComponent<MSController>();

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<AnimTileable>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddTag(Tuning.Tags.StorageModule);
			go.AddTag(Tuning.Tags.LiquidStorageModule);
		}
	}
}
