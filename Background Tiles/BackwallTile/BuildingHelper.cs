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
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
            go.AddComponent<ZoneTile>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = Hash.SDBMLower("tiles_" + tag + "_tops");
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);
        }

        public static void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
        }

        public static void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.AddTag(ModAssets.Tags.backWall);
        }
    }
}
