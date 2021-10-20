using FUtility;
using TUNING;
using UnityEngine;

namespace Asphalt
{
    public class AsphaltTileConfig : IBuildingConfig, IModdedBuilding
    {
        public const string ID = "AsphaltTile";
        public MBInfo Info => new MBInfo(ID, "Base", "ImprovedCombustion", MetalTileConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 1,
                height: 1,
                anim: "floor_asphalt_kanim",
                hitpoints: BUILDINGS.HITPOINTS.TIER2,
                construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                construction_materials: new string[1] { "Bitumen" },
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                build_location_rule: BuildLocationRule.Tile,
                decor: new EffectorValues(-5, 1),
                noise: NOISE_POLLUTION.NONE
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
            def.isSolidTile = true;
            def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

            Buildings.AddCustomTileAtlas(def, "tiles_metal", "asphalt");
            Buildings.AddCustomTops(def, "tiles_bunker_tops_decor_info", "asphalt", useExistingPlace: true, topsPlaceAtlas: "tiles_bunker_tops_decor_place_info");

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
            simCellOccupier.movementSpeedMultiplier = Tuning.ConvertSpeed(ModSettings.Asphalt.SpeedMultiplier);
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
        private void AdjustSpeed(GameObject go)
        {
            if(ModSettings.speedChanged)
                go.GetComponent<SimCellOccupier>().movementSpeedMultiplier = Tuning.ConvertSpeed(ModSettings.Asphalt.SpeedMultiplier);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}
