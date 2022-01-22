using System;
using System.Collections.Generic;
using TUNING;

namespace FUtility.BuildingUtil
{
    public class Buildings
    {
        public static string baseFolder = "assets/tiles";
        public static void RegisterBuildings(params Type[] buildings)
        {
            foreach (var building in buildings)
            {
                Log.Debuglog("Registering " + building.Name);
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
                Db.Get().Techs.Get(techGroup).unlockedItemIDs.Add(id);
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
            // def.isSolidTile = true;

            def.BlockTileMaterial = global::Assets.GetMaterial("tiles_solid");

            return def;
        }
    }
}
