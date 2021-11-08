using FUtility;
using TUNING;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
    class MoodLampConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.PREFIX + "MoodLamp";

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.FURNITURE, "InteriorDecor", FloorLampConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               1,
               2,
               "moodlamp_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               MATERIALS.TRANSPARENTS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               new EffectorValues(25, 4),
               NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "slab";
            def.PermittedRotations = PermittedRotations.FlipH;

            def.RequiresPowerInput = true;
            def.ExhaustKilowattsWhenActive = .5f;
            def.EnergyConsumptionWhenActive = 7f;
            def.SelfHeatKilowattsWhenActive = .5f;

            def.DefaultAnimState = "variant_1_off";

            return def;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
            lightShapePreview.lux = 400;
            lightShapePreview.radius = 3;
            lightShapePreview.shape = LightShape.Circle;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(RoomConstraints.ConstraintTags.LightSource);
            go.AddTag(RoomConstraints.ConstraintTags.Decor20);
            go.AddTag(ModAssets.Tags.noPaintTag);
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGet<LoopingSounds>();

            Light2D light2d = go.AddOrGet<Light2D>();
            light2d.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
            light2d.Color = Color.white;
            light2d.Range = 3f;
            light2d.shape = LightShape.Circle;
            light2d.Offset = new Vector2(0, 1f);
            light2d.drawOverlay = true;

            go.AddComponent<MoodLamp>();
        }
    }
}
