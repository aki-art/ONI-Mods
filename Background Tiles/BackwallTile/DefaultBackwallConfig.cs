/*using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace BackgroundTiles.BackwallTile
{
    // This one exists as a fall back / base for others. should not be buildable in game
    public class DefaultBackwallConfig : IBuildingConfig
    {
        public string name = "Default";
        public const string DEFAULT_ID = Mod.ID + "_DefaultWall";

        public string ID => $"{Mod.ID}_{name}Wall";

        public float[] constructionMass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
        public EffectorValues decor = DECOR.NONE;

        public BuildingDef original;

        public override BuildingDef CreateBuildingDef()
        {
            if (original is null)
            {
                original = Assets.GetBuildingDef(TileConfig.ID);
            }

            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "bw_backwall_kanim",
                original.HitPoints,
                original.ConstructionTime,
                original.Mass,
                original.MaterialCategory,
                original.BaseMeltingPoint,
                BuildLocationRule.NotInTiles,
                decor,
                NOISE_POLLUTION.NONE
            );

            def.ReplacementTags = new List<Tag>
            {
                ModAssets.Tags.backWall
            };


            BuildingTemplates.CreateFoundationTileDef(def);

            def.Floodable = false;
            def.Overheatable = false;
            def.Entombable = false;

            def.DragBuild = true;

            def.UseStructureTemperature = false;
            def.BaseTimeUntilRepair = -1f;

            def.AudioCategory = original.AudioCategory;
            def.AudioSize = AUDIO.SIZE.SMALL;

            def.IsFoundation = false;

            def.TileLayer = ObjectLayer.Backwall;
            def.ReplacementLayer = ObjectLayer.Backwall;
            def.ObjectLayer = ObjectLayer.Backwall;
            def.SceneLayer = Grid.SceneLayer.TileMain; //Mod.Settings.UseLogicGatesFrontSceneLayer ? Grid.SceneLayer.LogicGatesFront : Grid.SceneLayer.Backwall;
            def.BlockTileIsTransparent = true;

            def.RequiredDlcIds = original.RequiredDlcIds;

            def.isKAnimTile = true;
            Log.Debuglog("original null:", original is null);

            def.BlockTileMaterial = original.BlockTileMaterial;
            def.BlockTileAtlas = original.BlockTileAtlas;
            def.BlockTilePlaceAtlas = original.BlockTilePlaceAtlas; // todo: replace with custom
            def.BlockTileShineAtlas = original.BlockTileShineAtlas;
            Log.Debuglog("BlockTileAtlas null:", def.BlockTileAtlas is null);

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
            //go.AddOrGet<BWTileable>();
            go.AddOrGet<SimCellOccupier>().doReplaceElement = false;
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = Hash.SDBMLower("tiles_" + tag + "_tops");
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
        }


        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<SimTemperatureTransfer>();
            go.AddComponent<ZoneTile>();
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.AddTag(ModAssets.Tags.backWall);
        }
    }
}
*/