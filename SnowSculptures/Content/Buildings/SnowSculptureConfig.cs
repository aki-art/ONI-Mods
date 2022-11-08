using Database;
using TUNING;
using UnityEngine;

namespace SnowSculptures.Content.Buildings
{
    internal class SnowSculptureConfig : IBuildingConfig
    {
        public static string ID = "SnowSculptures_SnowSculpture";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
               ID,
               2,
               3,
               "sm_sculpture_snow_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
               new[] { SimHashes.Snow.ToString() },
               BUILDINGS.MELTING_POINT_KELVIN.TIER0,
               BuildLocationRule.OnFloor,
               DECOR.BONUS.TIER3,
               NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = AUDIO.CATEGORY.PLASTIC;
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "pile";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isArtable = true;
            go.AddTag(GameTags.Decoration);
            //go.AddComponent<GlassCaseSealable>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<SnowPile>().defaultAnimName = "slab";
        }

        public static void RegisterArtableStages(ArtableStages stages)
        {
            var config = Mod.Settings.Snowman;
            var anim = "sm_sculpture_snow_kanim";
            var artableStatuses = Db.Get().ArtableStatuses;
            var ID = SnowSculptureConfig.ID;

            var badName = STRINGS.BUILDINGS.PREFABS.SNOWSCULPTURES_SNOWSCULPTURE.POORQUALITYNAME;
            var excellentName = STRINGS.BUILDINGS.PREFABS.SNOWSCULPTURES_SNOWSCULPTURE.EXCELLENTQUALITYNAME;
            var snowDog = STRINGS.BUILDINGS.PREFABS.SNOWSCULPTURES_SNOWSCULPTURE.SNOWDOG;

            stages.Add("SnowSculptures_SnowSculpture_Muckroot", badName, anim, "variant_4", config.BadSculptureDecorBonus, true, artableStatuses.Ugly, ID);
            stages.Add("SnowSculptures_SnowSculpture_Derp", badName, anim, "derp", config.BadSculptureDecorBonus, true, artableStatuses.Ugly, ID);
            
            stages.Add("SnowSculptures_SnowSculpture_RomenCat", excellentName, anim, "variant_0", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Pip", excellentName, anim, "variant_1", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_SwoleMeep", excellentName, anim, "variant_2", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_WorkerSnowman", excellentName, anim, "variant_3", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Kitty", excellentName, anim, "variant_5", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Hassan", excellentName, anim, "variant_6", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Pufts", excellentName, anim, "variant_7", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Classic", excellentName, anim, "variant_8", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Snowdog", snowDog, anim, "variant_9", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Pei", excellentName, anim, "pei", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_SnowGolem", excellentName, anim, "snowgolem", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
            stages.Add("SnowSculptures_SnowSculpture_Slicksters", excellentName, anim, "slicksters", config.MediocreSculptureDecorBonus, true, artableStatuses.Okay, ID);
        }
    }
}
