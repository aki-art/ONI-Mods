using TUNING;
using UnityEngine;
using static DecorPackB.STRINGS.BUILDINGS.PREFABS.DECORPACKB_FOSSILDISPLAY;
using static FUtility.Consts;

namespace DecorPackB.Buildings.FossilDisplays
{
    internal class FossilDisplayConfig : IBuildingConfig
    {
        public static string ID = Mod.PREFIX + "FossilDisplay";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
               ID,
               3,
               2,
               "dp_fossildisplay_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               ModDb.Materials.FOSSIL,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               //new EffectorValues(Mod.Settings.FossilDisplay.BaseDecor.Amount, Mod.Settings.FossilDisplay.BaseDecor.Range),
               DECOR.BONUS.TIER2,
               NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = AUDIO_CATEGORY.PLASTIC;
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            //def.DefaultAnimState = "base";
            def.DefaultAnimState = "base";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(GameTags.Decoration);
            go.AddTag(ModDb.Tags.FossilBuilding);
            go.AddOrGet<BuildingComplete>().isArtable = true;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            //Settings.Config.FossilDisplayConfig config = Mod.Settings.FossilDisplay;
            go.AddComponent<MediumFossilDisplay>();

            var assemblable = go.AddComponent<Assemblable>();
            assemblable.stages.Add(new Artable.Stage("Default", NAME, "base", -5, false, Artable.Status.Ready));

            assemblable.stages.Add(new Artable.Stage("Human", VARIANT.PACU.NAME, "bad_1", 15, true, Artable.Status.Ugly));

            assemblable.stages.Add(new Artable.Stage("Parasaur", VARIANT.PARASAUROLOPHUS.NAME, "good_1", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Pacu", VARIANT.PACU.NAME, "good_2", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Trilobite", VARIANT.TRILOBITE.NAME, "good_3", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Beefalo", VARIANT.BEEFALO.NAME, "good_4", 15, true, Artable.Status.Great));
            assemblable.stages.Add(new Artable.Stage("Dodo", VARIANT.DODO.NAME, "good_5", 15, true, Artable.Status.Great));
        }
    }
}
