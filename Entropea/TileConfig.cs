using FUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace Entropea
{
    class TileConfig : IBuildingConfig, IModdedBuilding
    {
        public const string ID = "DumbTestTile";


        public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_bunker_tops");


        public MBInfo Info => new MBInfo(ID, "Base");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 1,
                height: 1,
                anim: "floor_mesh_kanim",
                hitpoints: TUNING.BUILDINGS.HITPOINTS.TIER2,
                construction_time: TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                construction_materials: MATERIALS.ANY_BUILDABLE,
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
            buildingDef.BlockTileAtlas = GetCustomAtlas(Path.Combine("assets", "tiles"), GetType(), referenceAtlas);


            buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_insulated_place");
            buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
            buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_info");
            buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_solid_tops_place_info");
            // Custom top pieces

            return buildingDef;
        }
        public static TextureAtlas GetCustomAtlas(string name, Type type, TextureAtlas tileAtlas)
        {
            var dir = Path.GetDirectoryName(type.Assembly.Location);
            var texFile = Path.Combine(dir, name + ".png");

            TextureAtlas atlas = null;

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(data);

                atlas = ScriptableObject.CreateInstance<TextureAtlas>();
                atlas.texture = tex;
                atlas.vertexScale = tileAtlas.vertexScale;
                atlas.items = tileAtlas.items;
            }
            else
                Debug.LogError($"ASPHALT: Could not load atlas image at path {texFile}.");

            return atlas;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            go.AddOrGet<TileTemperature>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
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
