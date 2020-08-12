using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static Artable;
using static InteriorDecorationVolI.STRINGS.BUILDINGS.PREFABS.GLASS_SCULPTURE;

namespace InteriorDecorationVolI.Buildings.GlassSculpture
{
    class GlassSculptureConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = ModAssets.Prefix + "GlassSculpture" + ModAssets.NoPaintSuffix;
        public MBInfo Info => new MBInfo(ID, "Furniture", "GlassFurnishings", MarbleSculptureConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 2,
               height: 2,
               anim: "sculpture_glass_kanim",
               hitpoints: BUILDINGS.HITPOINTS.TIER2,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.TRANSPARENTS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: new EffectorValues(20, 8),
               noise: NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "slab";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isArtable = true;
            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
            go.AddComponent<Fabulous>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Artable artable = go.AddComponent<Sculpture>();

            artable.stages.Add(new Stage("Default", NAME, "slab", 0, false, Status.Ready));
            artable.stages.Add(new Stage("Bad", POORQUALITYNAME, "bad_1", 5, false, Status.Ugly));
            artable.stages.Add(new Stage("Average", AVERAGEQUALITYNAME, "good_1", 10, false, Status.Okay));
            artable.stages.Add(new Stage("Good1", EXCELLENTQUALITYNAME, "amazing_1", 15, true, Status.Great));
            artable.stages.Add(new Stage("Good2", EXCELLENTQUALITYNAME, "amazing_2", 15, true, Status.Great));
            artable.stages.Add(new Stage("Good3", EXCELLENTQUALITYNAME, "amazing_3", 15, true, Status.Great));
            artable.stages.Add(new Stage("Good4", EXCELLENTQUALITYNAME, "amazing_4", 15, true, Status.Great));
            artable.stages.Add(new Stage("Good5", EXCELLENTQUALITYNAME, "amazing_5", 15, true, Status.Great));
        }
    }
}
