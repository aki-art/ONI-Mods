using System;
using System.Collections.Generic;
using TUNING;

namespace FUtility
{
    public class Buildings
    {
        public static void RegisterBuildings(params Type[] buildings)
        {
            foreach (var building in buildings)
            {
                if (typeof(IModdedBuilding).IsAssignableFrom(building))
                {
                    object obj = Activator.CreateInstance(building);
                    Register(obj as IModdedBuilding);
                }
            }
        }

        [Obsolete]
        public static void RegisterSingleBuilding(Type building)
        {
            if (typeof(IModdedBuilding).IsAssignableFrom(building))
            {
                object obj = Activator.CreateInstance(building);
                Register(obj as IModdedBuilding);
            }
        }

        private static void Register(IModdedBuilding b)
        {
            AddToBuildMenu(b);
            AddToResearch(b.Info.Research, b.Info.ID);
        }

        private static void AddToBuildMenu(IModdedBuilding b)
        {
            if (b.Info.BuildMenu.IsNullOrWhiteSpace())
                return;

            if (!b.Info.Following.IsNullOrWhiteSpace())
            {
                IList<string> category = FindCategory(b);
                int index = category.IndexOf(b.Info.Following);
                if (index != -1)
                {
                    category.Insert(index + 1, b.Info.ID);
                    return;
                }
            }

            ModUtil.AddBuildingToPlanScreen(b.Info.BuildMenu, b.Info.ID);
        }

        private static void AddToResearch(string techGroup, string id)
        {
            if (!techGroup.IsNullOrWhiteSpace())
            {
                var techList = new List<string>(Database.Techs.TECH_GROUPING[techGroup]) { id };
                Database.Techs.TECH_GROUPING[techGroup] = techList.ToArray();
            }
        }

        private static IList<string> FindCategory(IModdedBuilding b)
        {
            return BUILDINGS.PLANORDER.Find(x => x.category == b.Info.BuildMenu).data as IList<string>;
        }

        public static BuildingDef CreateTileDef(string ID, string anim, float constructionMass, string material, EffectorValues decor, bool transparent)
        {
            return CreateTileDef(ID, anim, new float[] { constructionMass }, new string[] { material }, decor, transparent);
        }

        public static BuildingDef CreateTileDef(string ID, string anim, float[] constructionMass, string[] material, EffectorValues decor, bool transparent)
        {

            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 1,
                height: 1,
                anim: anim + "_kanim",
                hitpoints: BUILDINGS.HITPOINTS.TIER1,
                construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                construction_mass: constructionMass,
                construction_materials: material,
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
                build_location_rule: BuildLocationRule.Tile,
                decor: decor,
                noise: NOISE_POLLUTION.NONE
                );

            BuildingTemplates.CreateFoundationTileDef(def);
            def.Floodable = false;
            def.Overheatable = false;
            def.Entombable = false;
            def.UseStructureTemperature = false;
            def.AudioCategory = "Glass";
            def.AudioSize = "small";
            def.BaseTimeUntilRepair = -1f;
            def.SceneLayer = transparent ? Grid.SceneLayer.GlassTile : Grid.SceneLayer.TileMain;
            def.isKAnimTile = true;
            def.BlockTileIsTransparent = transparent;
            def.isSolidTile = true;

            def.BlockTileMaterial = global::Assets.GetMaterial("tiles_solid");

            return def;
        }

        public static void AddCustomTileAtlas(
            BuildingDef def, 
            string referenceAtlas, 
            string defaultAssetName,
            bool shiny = false,
            string BlockTileAtlas = null,
            string BlockTilePlaceAtlas = null,
            string BlockTileShineAtlas = null
            )
        {
            TextureAtlas reference = global::Assets.GetTextureAtlas(referenceAtlas);
            if (BlockTileAtlas == null)
                BlockTileAtlas = $"tiles_{defaultAssetName}"; 
            if (BlockTilePlaceAtlas == null)
                BlockTilePlaceAtlas = $"tiles_{defaultAssetName}_place";

            def.BlockTileAtlas = Assets.GetCustomAtlas(BlockTileAtlas, "assets", reference);
            def.BlockTilePlaceAtlas = Assets.GetCustomAtlas(BlockTilePlaceAtlas, "assets", reference);

            if (shiny)
            {
                if (BlockTileShineAtlas == null)
                    BlockTileShineAtlas = $"tiles_{defaultAssetName}_spec";
                def.BlockTileShineAtlas = Assets.GetCustomAtlas(BlockTileShineAtlas, "assets", reference);
            }
        }

        public static void AddCustomTops(
      BuildingDef def,
      string referenceTileInfo,
      string defaultAssetName,
      bool shiny = false,
      string TopsAtlas = null,
      string TopsShineAtlas = null,
      bool useExistingPlace = true,
      string topsPlaceAtlas = null
      )
        {
            BlockTileDecorInfo decorBlockTileInfo = UnityEngine.Object.Instantiate(global::Assets.GetBlockTileDecorInfo(referenceTileInfo));
            if (TopsAtlas == null)
                TopsAtlas = $"tiles_{defaultAssetName}_tops";

            decorBlockTileInfo.atlas = Assets.GetCustomAtlas(TopsAtlas, "assets", decorBlockTileInfo.atlas);
            def.DecorBlockTileInfo = decorBlockTileInfo;

            if(useExistingPlace)
            {
                def.DecorPlaceBlockTileInfo = global::Assets.GetBlockTileDecorInfo(topsPlaceAtlas);
            }
            else
            {
                if (topsPlaceAtlas == null)
                {
                    topsPlaceAtlas = $"tiles_{defaultAssetName}_tops_place";
                }

                BlockTileDecorInfo decorPlaceTileInfo = UnityEngine.Object.Instantiate(global::Assets.GetBlockTileDecorInfo(referenceTileInfo));
                decorPlaceTileInfo.atlas = Assets.GetCustomAtlas(topsPlaceAtlas, "assets", decorBlockTileInfo.atlas);
                def.DecorPlaceBlockTileInfo = decorPlaceTileInfo;
            }

            if (shiny)
            {
                if (TopsShineAtlas == null)
                    TopsShineAtlas = $"tiles_{defaultAssetName}_tops_spec";
                decorBlockTileInfo.atlasSpec = Assets.GetCustomAtlas(TopsShineAtlas, "assets", decorBlockTileInfo.atlasSpec);
            }
        }
    }
}
