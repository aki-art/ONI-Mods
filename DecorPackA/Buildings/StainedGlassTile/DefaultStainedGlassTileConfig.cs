using FUtility;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    // This one exists as a fall back / base for others. should not be buildable in game
    class DefaultStainedGlassTileConfig : IBuildingConfig, IModdedBuilding
    {
        private static readonly string name = "Default";
        public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.BASE, "GlassFurnishings", GlassTileConfig.ID);

        public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
            //simCellOccupier.doReplaceElement = false;
            simCellOccupier.notifyOnMelt = true;
            simCellOccupier.setTransparent = true;

            go.AddComponent<DyeInsulator>();
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.AddTag(GameTags.FloorTiles);
            go.AddTag(GameTags.Window);
            go.AddTag(ModAssets.Tags.stainedGlass);
            go.AddTag(ModAssets.Tags.noPaintTag);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}
