/*
using TUNING;
using UnityEngine;

namespace Slag.Buildings
{
	using System;

	public class ConcreteTileConfig : IBuildingConfig
	{
		public const string ID = "ConcreteTile";
		public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_solid_tops");
		public static LocString NAME = "Concrete tile";
		public static LocString DESC = "Concrete tile des";
		public static LocString EFFECT = "Concretet tile effect";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "floor_basic_kanim",
				hitpoints: 100,
				construction_time: 3f,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				construction_materials: MATERIALS.RAW_MINERALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.BONUS.TIER0,
				noise: NOISE_POLLUTION.NONE );

			BuildingTemplates.CreateFoundationTileDef(def);

			def.Floodable = false;
			def.Overheatable = false;
			def.Entombable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.isKAnimTile = true;
			def.isSolidTile = true;

			def.BlockTileAtlas = Assets.GetTextureAtlas("tiles_solid");
			def.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_solid_place");
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			def.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_info");
			def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_place_info");

			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			def.DragBuild = true;
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			SimCellOccupier sim_cell_occupier = go.AddOrGet<SimCellOccupier>();
			sim_cell_occupier.doReplaceElement = true;
			sim_cell_occupier.strengthMultiplier = 1.5f;
			sim_cell_occupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.BONUS_2;
			sim_cell_occupier.notifyOnMelt = true;

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
			go.AddComponent<Colorable>();
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}

	}

}
*/