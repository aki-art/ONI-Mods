using STRINGS;
using System.IO;
using TUNING;
using UnityEngine;

namespace Asphalt
{
    public class AsphaltConfig : IBuildingConfig
    {
        public const string ID = "AsphaltTile";

        public static readonly LocString NAME = UI.FormatAsLink("Asphalt Tile", ID);
        public static readonly LocString DESC = "Asphalt tiles feel great to run on.";
        public static readonly LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSubstantially increases Duplicant runspeed.";

        public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_bunker_tops");

        private readonly string[] BitumenMaterials = new string[1] { "Bitumen" };

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 1,
                height: 1,
                anim: "floor_asphalt_kanim",
                hitpoints: TUNING.BUILDINGS.HITPOINTS.TIER2,
                construction_time: TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                construction_materials: BitumenMaterials,
                melting_point: TUNING.BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                build_location_rule: BuildLocationRule.Tile,
                decor: new EffectorValues(-5, 1),
                noise: NOISE_POLLUTION.NONE
            );

            BuildingTemplates.CreateFoundationTileDef(buildingDef);

            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.Overheatable = false;
            buildingDef.UseStructureTemperature = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "small";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
            buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
            buildingDef.isKAnimTile = true;
            buildingDef.isSolidTile = true;

            buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

            // Custom texture
            TextureAtlas referenceAtlas = Assets.GetTextureAtlas("tiles_metal"); 
            buildingDef.BlockTileAtlas = ModAssets.GetCustomAtlas(Path.Combine("anim", "assets", "tiles_asphalt"), GetType(), referenceAtlas);
            buildingDef.BlockTilePlaceAtlas = ModAssets.GetCustomAtlas(Path.Combine("anim", "assets", "tiles_asphalt_place"), GetType(), referenceAtlas);

            // Custom top pieces
            BlockTileDecorInfo decorBlockTileInfo = Object.Instantiate(Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_info"));
            decorBlockTileInfo.atlas = ModAssets.GetCustomAtlas(Path.Combine("anim", "assets", "tiles_asphalt_tops"), GetType(), decorBlockTileInfo.atlas);
            buildingDef.DecorBlockTileInfo = decorBlockTileInfo;
            buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_place_info");

            return buildingDef;
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
            simCellOccupier.movementSpeedMultiplier = FSpeedSlider.MapValue(SettingsManager.Settings.SpeedMultiplier);
            simCellOccupier.strengthMultiplier = 2f;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
            go.AddComponent<SimTemperatureTransfer>();
            go.AddComponent<ZoneTile>();
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}
