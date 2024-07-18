using FUtility;
using TUNING;
using UnityEngine;

namespace Asphalt.Buildings
{
	public class AsphaltTileConfig : IBuildingConfig
	{
		public const string ID = "AsphaltTile";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"floor_asphalt_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				ModAssets.ROAD_SURFACE_MATERIALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.Tile,
				new EffectorValues(-5, 1),
				NOISE_POLLUTION.NONE
			);

			BuildingTemplates.CreateFoundationTileDef(def);

			def.Floodable = false;
			def.Entombable = false;
			def.Overheatable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = AUDIO.CATEGORY.METAL;
			def.AudioSize = AUDIO.SIZE.SMALL;
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			def.isKAnimTile = true;
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

			Tiles.AddCustomTileAtlas(def, "asphalt");
			Tiles.AddCustomTileTops(def, "asphalt", false, "tiles_bunker_tops_decor_info", "tiles_bunker_tops_decor_place_info");

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;

			SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.strengthMultiplier = 2f;
			simCellOccupier.movementSpeedMultiplier = Mod.Settings.Speed;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);

			KPrefabID prefabID = go.GetComponent<KPrefabID>();
			prefabID.AddTag(GameTags.FloorTiles);
			prefabID.prefabSpawnFn += AdjustSpeed;

			go.AddComponent<SimTemperatureTransfer>();
			go.AddComponent<ZoneTile>();
		}

		// Neccessary for already existing tiles to be updated on world reload after editing settings
		// This way the player isn't forced to restart their game to apply the changes
		private void AdjustSpeed(GameObject go)
		{
			if (Mod.Settings.SpeedChanged)
				go.GetComponent<SimCellOccupier>().movementSpeedMultiplier = Mod.Settings.Speed;
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}