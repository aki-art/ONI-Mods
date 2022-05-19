using FUtility;
using TUNING;
using UnityEngine;

namespace Slag.Content.Buildings
{
    public class InsulatedWindowTileConfig : IBuildingConfig
    {
        public const string ID = "Slag_InsulatedWindowTile";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "floor_insulated_window_kanim",
                BUILDINGS.HITPOINTS.TIER2,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                new[] { 300f, 4f },
                new[] { MATERIALS.TRANSPARENT, ModAssets.Tags.slagWool.ToString() },
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.Tile,
                DECOR.NONE,
                NOISE_POLLUTION.NONE
            );

            BuildingTemplates.CreateFoundationTileDef(def);

            def.Floodable = false;
            def.Entombable = false;
            def.Overheatable = false;

            def.UseStructureTemperature = false;
            def.ThermalConductivity = Mod.Settings.InsulatedWindowTCMultiplier;

            def.AudioCategory = AUDIO.CATEGORY.GLASS;
            def.AudioSize = AUDIO.SIZE.SMALL;

            def.BaseTimeUntilRepair = -1f;
            def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;

            def.SceneLayer = Grid.SceneLayer.TileMain;
            def.isKAnimTile = true;
            def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
            def.BlockTileIsTransparent = true;

            Tiles.AddCustomTileAtlas(def, "insulatedwindow", true);
            Tiles.AddCustomTileTops(def, "insulatedwindow", false, "tiles_glass_tops_decor_info", "tiles_glass_tops_decor_place_info");

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            go.AddOrGet<TileTemperature>();
            go.AddOrGet<Insulator>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;

            var simCellOccupier = go.AddOrGet<SimCellOccupier>();
            simCellOccupier.doReplaceElement = true;
            simCellOccupier.strengthMultiplier = 2f;
            simCellOccupier.setTransparent = true;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);

            var prefabID = go.GetComponent<KPrefabID>();
            prefabID.AddTag(GameTags.FloorTiles);
            prefabID.AddTag(GameTags.Window);

            go.AddComponent<SimTemperatureTransfer>();
            go.AddComponent<ZoneTile>();
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}