using UnityEngine;

namespace BackgroundTiles.BackwallTile
{
    public class BuildingHelper
    {
        public static GameObject CreateFromBaseTemplate(BuildingDef def, GameObject baseTemplate)
        {
            GameObject gameObject = Object.Instantiate(baseTemplate);
            Object.DontDestroyOnLoad(gameObject);
            gameObject.GetComponent<KPrefabID>().PrefabTag = def.Tag;
            gameObject.name = def.PrefabID + "Template";
            gameObject.GetComponent<Building>().Def = def;
            gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets = def.PlacementOffsets;

            ConfigureBuildingTemplate(gameObject, def.Tag);

            // BuildingComplete
            def.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, def);

            // Building under Construction
            def.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(def);
            def.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(def.BuildingUnderConstruction.name);

            // Building Preview
            def.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(def);
            def.BuildingPreview.name += "Preview";


            return gameObject;
        }

        private static void ConfigureBuildingTemplate(GameObject go, Tag tag)
        {
            //var sco = go.AddComponent<SimCellOccupier>();
            //sco.setTransparent = true;
            //sco.doReplaceElement = false;

            //go.AddComponent<Backwall>();

            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

            //go.AddOrGet<SimCellOccupier>().doReplaceElement = false;
            //go.AddOrGet<TileTemperature>();
            //go.AddComponent<BWTileable>();

            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
            //go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = Hash.SDBMLower("tiles_" + tag + "_tops");
        }

        public static void DoPostConfigureUnderConstruction(GameObject go)
        {
        }

        public static void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<SimTemperatureTransfer>();
            go.AddComponent<ZoneTile>();
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.AddTag(ModAssets.Tags.backWall);
        }
    }
}
