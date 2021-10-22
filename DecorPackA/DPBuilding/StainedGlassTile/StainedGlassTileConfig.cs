using FUtility;
using FUtility.BuildingHelper;
using TUNING;
using UnityEngine;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class StainedGlassTileConfig : IBuildingConfig, IModdedBuilding
	{
		public static string ID = Mod.PREFIX + "StainedGlassTile";
		public MBInfo Info => new MBInfo(ID, Consts.BUILD_MENU.BASE, "GlassFurnishings", GlassTileConfig.ID);

		public override BuildingDef CreateBuildingDef()
		{
			var def = FUtility.Buildings.CreateTileDef(
				ID, 
				"floor_glass",
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
				MATERIALS.GLASS,
				BUILDINGS.DECOR.PENALTY.TIER2,
				true);

			def.ShowInBuildMenu = true;

			Tiles.AddCustomTileAtlas(def, "steel_glass", true);
			Tiles.AddCustomTileTops(def, "steel_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.strengthMultiplier = 10f;
			simCellOccupier.notifyOnMelt = true;
			simCellOccupier.setTransparent = true;

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.AddTag(GameTags.FloorTiles);
			go.AddTag(GameTags.Bunker);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
