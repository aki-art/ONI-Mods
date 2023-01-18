using DecorPackB.Content.Scripts;
using TUNING;
using UnityEngine;

namespace DecorPackB.Content.Buildings
{
    internal class FossilDisplayConfig : IBuildingConfig
    {
        public const string ID = "DecorPackB_FossilDisplay";

        public override BuildingDef CreateBuildingDef()
        {
            var functionalFossils = Mod.LiteModeSettings.FunctionalFossils;

            var def = BuildingTemplates.CreateBuildingDef(
               ID,
               3,
               2,
               "decorpackb_fossildisplay_base_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               functionalFossils ? BUILDINGS.CONSTRUCTION_MASS_KG.TIER4 : BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
                functionalFossils ? ModDb.Materials.FOSSIL : ModDb.Materials.FOSSIL_LITE,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               //new EffectorValues(Mod.Settings.FossilDisplay.BaseDecor.Amount, Mod.Settings.FossilDisplay.BaseDecor.Range),
               DECOR.BONUS.TIER2,
               NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = AUDIO.CATEGORY.PLASTIC;
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "base";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(GameTags.Decoration);
            go.AddTag(DPTags.FossilBuilding);
            go.AddOrGet<BuildingComplete>().isArtable = true;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<Exhibition>();
            go.AddComponent<Inspiring>();
        }
    }
}
