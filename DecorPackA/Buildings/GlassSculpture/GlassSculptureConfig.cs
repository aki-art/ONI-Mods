using FUtility;
using TUNING;
using UnityEngine;
using static Artable;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_GLASSSCULPTURE;
using static FUtility.Consts;

namespace DecorPackA.Buildings.GlassSculpture
{
    class GlassSculptureConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.PREFIX + "GlassSculpture";
        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.FURNITURE, "GlassFurnishings", MarbleSculptureConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               2,
               2,
               "sculpture_glass_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               MATERIALS.TRANSPARENTS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               new EffectorValues(Mod.Settings.GlassSculpture.BaseDecor.Amount, Mod.Settings.GlassSculpture.BaseDecor.Range),
               NOISE_POLLUTION.NONE
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
            go.AddTag(GameTags.Decoration);
            go.AddTag(ModAssets.Tags.noPaintTag);
            go.AddComponent<Fabulous>().offset = new Vector3(.5f, .5f, .4f);
        }
        public override void DoPostConfigureComplete(GameObject go)
        {
            Artable artable = go.AddComponent<Sculpture>();

            Settings.Config.GlassSculpturesConfig config = Mod.Settings.GlassSculpture;

            artable.stages.Add(new Stage("Default", NAME, "slab", 0, false, Status.Ready));
            artable.stages.Add(new Stage("Bad", POORQUALITYNAME, "crap_1", config.BadSculptureDecorBonus, false, Status.Ugly));
            artable.stages.Add(new Stage("Average", AVERAGEQUALITYNAME, "good_1", config.MediocreSculptureDecorBonus, false, Status.Okay));
            artable.stages.Add(new Stage("Good1", EXCELLENTQUALITYNAME, "amazing_1", config.GeniousSculptureDecorBonus, true, Status.Great));
            artable.stages.Add(new Stage("Good2", EXCELLENTQUALITYNAME, "amazing_2", config.GeniousSculptureDecorBonus, true, Status.Great));
            artable.stages.Add(new Stage("Good3", EXCELLENTQUALITYNAME, "amazing_3", config.GeniousSculptureDecorBonus, true, Status.Great));
            artable.stages.Add(new Stage("Good4", EXCELLENTQUALITYNAME, "amazing_4", config.GeniousSculptureDecorBonus, true, Status.Great));
            artable.stages.Add(new Stage("Good5", EXCELLENTQUALITYNAME, "amazing_5", config.GeniousSculptureDecorBonus, true, Status.Great));
        }
    }
}
