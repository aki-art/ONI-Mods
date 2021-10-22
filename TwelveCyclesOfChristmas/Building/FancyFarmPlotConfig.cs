using FUtility;
using TUNING;
using UnityEngine;
using static Artable;
using static TwelveCyclesOfChristmas.Lang.STRINGS.BUILDINGS.PREFABS.TDOC_FANCYFARMPLOT;

namespace TwelveCyclesOfChristmas.Building
{
    class FancyFarmPlotConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = "TDoC_FancyFarmPlot";

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_MENU.FOOD);

        public static string[] BUILD_MATERIALS = new string[]
        {
            MATERIALS.FARMABLE[0],
            MATERIALS.METAL
        };

        public static float[] BUILD_MASS = new float[]
        {
           100f,
           100f
        };

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 1,
               anim: "tdoc_fancy_planter_box_kanim",
               hitpoints: BUILDINGS.HITPOINTS.TIER2,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
               construction_mass: BUILD_MASS,
               construction_materials: BUILD_MATERIALS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: BUILDINGS.DECOR.BONUS.TIER3,
               noise: NOISE_POLLUTION.NONE
            );

            def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
            def.Overheatable = false;
            def.Floodable = false;
            def.AudioCategory = "Glass";
            def.AudioSize = "large";

            return def;
        }
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Storage storage = go.AddOrGet<Storage>();

            PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
            plantablePlot.AddDepositTag(GameTags.CropSeed);
            plantablePlot.SetFertilizationFlags(true, false);

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
            BuildingTemplates.CreateDefaultStorage(go, false);
            storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<PlanterBox>();
            go.AddOrGet<AnimTileable>();

            //go.AddOrGet<BuildingComplete>().isArtable = true;
            go.AddTag(GameTags.Decoration);

            Prioritizable.AddRef(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            /*
            Artable artable = go.AddComponent<Sculpture>();

            artable.stages.Add(new Stage("Default", NAME, "slab", 0, false, Status.Ready));
            artable.stages.Add(new Stage("Bad", POORQUALITYNAME, "bad_1", 5, false, Status.Ugly));
            artable.stages.Add(new Stage("Bad", POORQUALITYNAME, "bad_2", 5, false, Status.Ugly));
            artable.stages.Add(new Stage("Average", AVERAGEQUALITYNAME, "good_1", 10, false, Status.Okay));
            artable.stages.Add(new Stage("Average", AVERAGEQUALITYNAME, "good_2", 10, false, Status.Okay));
            artable.stages.Add(new Stage("Good1", EXCELLENTQUALITYNAME, "amazing_1", 15, true, Status.Great));
            artable.stages.Add(new Stage("Good2", EXCELLENTQUALITYNAME, "amazing_2", 15, true, Status.Great));
            artable.stages.Add(new Stage("Good3", EXCELLENTQUALITYNAME, "amazing_3", 15, true, Status.Great));
            */
        }
    }
}
