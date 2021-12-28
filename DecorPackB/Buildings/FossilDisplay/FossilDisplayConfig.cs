using FUtility;
using TUNING;
using UnityEngine;
using static FUtility.Consts;
using static DecorPackB.STRINGS.BUILDINGS.PREFABS.DECORPACKB_FOSSILDISPLAY;

namespace DecorPackB.Buildings.FossilDisplay
{
    internal class FossilDisplayConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.PREFIX + "FossilDisplay";
        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.FURNITURE, TECH.DECOR.ENVIRONMENTAL_APPRECIATION, MarbleSculptureConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               3,
               2,
               "fossil_display_kanim",
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
            def.AudioCategory = AUDIO_CATEGORY.PLASTIC;
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "base";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isArtable = true;
            go.AddTag(GameTags.Decoration);
        }
        public override void DoPostConfigureComplete(GameObject go)
        {
            Settings.Config.GlassSculpturesConfig config = Mod.Settings.GlassSculpture;
            
            Assemblable assemblable = go.AddComponent<Assemblable>();
            assemblable.stages.Add(new Artable.Stage("Default", NAME, "base", -5, false, Artable.Status.Ready));
            //assemblable.stages.Add(new Artable.Stage("Average", VARIANT.HUMAN.NAME, "okay_1", 10, false, Artable.Status.Okay));
            //assemblable.stages.Add(new Artable.Stage("Average", VARIANT.SPIDER.NAME, "okay_1", 10, false, Artable.Status.Okay));
            assemblable.stages.Add(new Artable.Stage("Human", VARIANT.PACU.NAME, "bad_1", 15, true, Artable.Status.Great));

            assemblable.stages.Add(new Artable.Stage("Parasaur", VARIANT.PARASAUROLOPHUS.NAME, "good_1", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Trilobite", VARIANT.PACU.NAME, "good_3", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Beefalo", VARIANT.PACU.NAME, "good_4", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Pacu", VARIANT.PACU.NAME, "good_2", 15, true, Artable.Status.Great));
        }

    }
}
