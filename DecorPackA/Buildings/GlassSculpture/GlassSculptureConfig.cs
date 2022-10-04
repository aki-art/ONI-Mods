using Database;
using TUNING;
using UnityEngine;
using static Artable;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_GLASSSCULPTURE;

namespace DecorPackA.Buildings.GlassSculpture
{
    internal class GlassSculptureConfig : IBuildingConfig
    {
        public static string ID = Mod.PREFIX + "GlassSculpture";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
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
            go.AddTag(ModAssets.Tags.noPaint);
            go.AddComponent<Fabulous>().offset = new Vector3(.5f, .5f, .4f);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<Sculpture>().defaultAnimName = "slab";
        }

        public static void RegisterStages(ArtableStages stages)
        {
            var config = Mod.Settings.GlassSculpture;
            var anim = "sculpture_glass_kanim";
            var artableStatuses = Db.Get().ArtableStatuses;

            //stages.Add("Default", NAME, anim, "slab", 0, false, artableStatuses.Ready, ID);
            stages.Add("Bad", POORQUALITYNAME, anim, "crap_1", config.BadSculptureDecorBonus, false, artableStatuses.Ugly, ID);
            stages.Add("Average", AVERAGEQUALITYNAME, anim, "good_1", config.MediocreSculptureDecorBonus, false, artableStatuses.Okay, ID);
            stages.Add("Good1", EXCELLENTQUALITYNAME, anim, "amazing_1", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("Good2", EXCELLENTQUALITYNAME, anim, "amazing_2", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("Good3", EXCELLENTQUALITYNAME, anim, "amazing_3", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("Good4", EXCELLENTQUALITYNAME, anim, "amazing_4", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("Good5", EXCELLENTQUALITYNAME, anim, "amazing_5", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
        }
    }
}
