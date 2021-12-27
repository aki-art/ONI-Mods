using FUtility;
using TUNING;
using UnityEngine;
using static FUtility.Consts;

namespace GlassCase.Buildings
{
    public class GlassCaseValveConfig : IBuildingConfig, IModdedBuilding
    {
        public static readonly string ID = Mod.Prefix("GlassCaseValve");
        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.UTILITIES, null);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               1,
               1,
               "floor_mesh_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
               MATERIALS.TRANSPARENTS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.NotInTiles,
               DECOR.NONE,
               NOISE_POLLUTION.NONE
           );

            BuildingTemplates.CreateFoundationTileDef(def);

            def.IsFoundation = true;
            def.TileLayer = ObjectLayer.Backwall;
            def.ReplacementLayer = ObjectLayer.Backwall;

            def.Floodable = false;
            def.Overheatable = false;
            def.Entombable = false;

            def.UseStructureTemperature = false;
            def.BaseTimeUntilRepair = -1f;

            def.AudioCategory = "Glass";
            def.AudioSize = "small";

            def.ObjectLayer = ObjectLayer.FoundationTile;
            def.SceneLayer = Grid.SceneLayer.TileMain;

            def.isKAnimTile = true;

            def.BlockTileIsTransparent = true;

            def.BlockTileAtlas = Assets.GetTextureAtlas("tiles_mesh");
            def.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_mesh_place");
            def.BlockTileShineAtlas = Assets.GetTextureAtlas("tiles_mesh_spec");
            def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

            def.RequiredDlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 8f;
            def.SelfHeatKilowattsWhenActive = 0.5f;

            // leaving these null so they are not rendered
            // def.DecorBlockTileInfo = null;
            // def.DecorPlaceBlockTileInfo = null;

            return def;
        }
        public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
        {
            go.AddComponent<GlassCaseValve>();

            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

            go.AddOrGet<TileTemperature>();

            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = Hash.SDBMLower("tiles_" + tag + "_tops");
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGetDef<PoweredController.Def>();
            go.AddComponent<SimTemperatureTransfer>();
            go.AddComponent<ZoneTile>();
            GeneratedBuildings.RemoveLoopingSounds(go);
        }
    }
}
