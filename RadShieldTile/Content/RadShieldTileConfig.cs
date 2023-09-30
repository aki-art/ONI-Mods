using FUtility;
using TUNING;
using UnityEngine;

namespace RadShieldTile.Content
{
    public class RadShieldTileConfig : IBuildingConfig
	{
		public const string ID = "RadShieldTile_RadShieldTile";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"rst_floor_radshield_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				new[]
				{
					800f
				},
				new[]
				{
					RSTTags.ShieldMaterial.ToString()
				},
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.Tile,
				new EffectorValues(-10, 1),
				NOISE_POLLUTION.NONE
			);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			def.isKAnimTile = true;
			def.BlockTileMaterial = new Material(Assets.GetMaterial("tiles_solid"));
			def.BlockTileMaterial.SetColor("_ShineColour", new Color(0, 2f, 0.55f));

			Tiles.AddCustomTileAtlas(def, "rad_shield", true);
			Tiles.AddCustomTileTops(def, "rad_shield", false, "tiles_bunker_tops_decor_info", "tiles_bunker_tops_decor_place_info");

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = BunkerTileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.strengthMultiplier = 2f;
			simCellOccupier.movementSpeedMultiplier = 0.75f;
			simCellOccupier.notifyOnMelt = true;

			go.AddOrGet<RadShield>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);

			KPrefabID prefabID = go.GetComponent<KPrefabID>();
			prefabID.AddTag(GameTags.FloorTiles);

			go.AddComponent<ZoneTile>();
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}