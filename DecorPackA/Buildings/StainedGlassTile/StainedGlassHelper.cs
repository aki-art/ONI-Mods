using FUtility.BuildingHelper;
using TUNING;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    public class StainedGlassHelper
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


        internal static void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
            simCellOccupier.notifyOnMelt = true;
            simCellOccupier.setTransparent = true;
            simCellOccupier.movementSpeedMultiplier = 1.25f;

            go.AddComponent<DyeInsulator>();
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
            go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
        }

        internal static void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.AddTag(GameTags.FloorTiles);
            go.AddTag(GameTags.Window);
            go.AddTag(ModAssets.Tags.stainedGlass);
            go.AddTag(ModAssets.Tags.noPaintTag);
        }

        internal static void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
        }

        public static readonly EffectorValues DECOR = new EffectorValues
        {
            amount = 10,
            radius = 3
        };

        public static BuildingDef CreateGlassTileDef(string name, string ID)
        {
            string[] materials = new string[] { MATERIALS.TRANSPARENT, ModAssets.Tags.stainedGlassDye.ToString() };
            float[] mass = new float[] { 50f, 50f };

            var def = FUtility.Buildings.CreateTileDef(ID, "floor_stained_glass", mass, materials, DECOR, true);

            def.ShowInBuildMenu = true; // this makes the Blueprints mod happy

            Tiles.AddCustomTileAtlas(def, name + "_glass", true);
            Tiles.AddCustomTileTops(def, name + "_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

            return def;
        }
    }
}
