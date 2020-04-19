
using System;
using TUNING;
using UnityEngine;

namespace Slag.Buildings
{
	public class DenseInsulationTileConfig : IBuildingConfig
	{
		public const string ID = "DenseInsulationTile";
		public override BuildingDef CreateBuildingDef()
		{
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "floor_insulated_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0], 2f },
				construction_materials: new string[] { "BuildableRaw", ModAssets.slagWoolTag.ToString() },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.PENALTY.TIER2,
				noise: none
				);

			BuildingTemplates.CreateFoundationTileDef(def);
			def.ThermalConductivity = 0.0025f;
			def.Floodable = false;
			def.Overheatable = false;
			def.Entombable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.isKAnimTile = true;

			def.BlockTileAtlas = Assets.GetTextureAtlas("tiles_insulated");
			def.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_insulated_place");
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			def.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_info");
			def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_place_info");

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			SimCellOccupier sim_cell_occupier = go.AddOrGet<SimCellOccupier>();
			sim_cell_occupier.doReplaceElement = true;
			sim_cell_occupier.notifyOnMelt = true;

			go.AddOrGet<Insulator>();
			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
