using FUtility;
using UnityEngine;
using FUtility.BuildingHelper;
using TUNING;

namespace BackgroundTiles.Buildings
{
    public class BackgroundTileConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.ID + "BackgroundTile"; // yes it's BackgroundTileBackgroundTile

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.BASE, "GlassFurnishings", GlassTileConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "floor_carpet_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
                MATERIALS.ANY_BUILDABLE,
                BUILDINGS.MELTING_POINT_KELVIN.TIER3,
                BuildLocationRule.Tile,
                DECOR.NONE,
                NOISE_POLLUTION.NONE
            );

            BuildingTemplates.CreateFoundationTileDef(def);
            def.Floodable = false;
            def.Overheatable = false;
            def.Entombable = false;
            def.UseStructureTemperature = false;
            def.AudioCategory = "Glass";
            def.AudioSize = "small";
            def.BaseTimeUntilRepair = -1f;
            def.SceneLayer = Grid.SceneLayer.Background;
            def.isKAnimTile = true;
            def.BlockTileIsTransparent = false;
            def.ShowInBuildMenu = true; // this makes the Blueprints mod happy

            def.BlockTileMaterial = global::Assets.GetMaterial("tiles_solid");

            // do not add tops/"decor"

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}
