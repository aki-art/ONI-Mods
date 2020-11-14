using TUNING;
using UnityEngine;

namespace TransparentAluminium
{
	public class TransparentAluminiumTileConfig : IBuildingConfig
	{
		public const string ID = "TAT_TransparentAluminiumTile";

		public override BuildingDef CreateBuildingDef()
		{
			var def = FUtility.Buildings.CreateTileDef(
				ID: ID, anim: "floor_glass", 
				constructionMass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
				material: "TransparentAluminum", 
				decor: BUILDINGS.DECOR.PENALTY.TIER2, 
				transparent: true);
	
			def.ShowInBuildMenu = true;

			string name = "transparent_aluminum";
			FUtility.Buildings.AddCustomTileAtlas(def, "tiles_metal", name, true);
			FUtility.Buildings.AddCustomTops(def, "tiles_glass_tops_decor_info", name,
				useExistingPlace: true,
				topsPlaceAtlas: "tiles_glass_tops_decor_place_info");

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
			var kprefabID = go.GetComponent<KPrefabID>();
			kprefabID.AddTag(GameTags.FloorTiles);
			kprefabID.AddTag(GameTags.Bunker);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
