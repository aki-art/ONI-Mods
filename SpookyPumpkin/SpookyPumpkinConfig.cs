using FUtility;
using TUNING;
using UnityEngine;

namespace SpookyPumpkin
{
    public class SpookyPumpkinConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = "SP_SpookyPumpkin";
        public MBInfo Info => new MBInfo(ID, "Furniture", null, FloorLampConfig.ID);
        const int LUX = 700;
        const float RANGE = 2f;

        public override BuildingDef CreateBuildingDef()
        {
            string ores = MATERIALS.RAW_METALS[0];
            string pumpkin = ModAssets.buildingPumpkinTag.ToString();

            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 1,
               anim: "sp_spookypumpkin_kanim",
               hitpoints: BUILDINGS.HITPOINTS.TIER2,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
               construction_mass: new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0], 1f },
               construction_materials: new string[] { ores, pumpkin},
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: BUILDINGS.DECOR.BONUS.TIER3,
               noise: NOISE_POLLUTION.NONE
           );

            def.AudioCategory = "Glass";
            def.ViewMode = OverlayModes.Decor.ID;
            def.PermittedRotations = PermittedRotations.FlipH;

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 3f;
            def.SelfHeatKilowattsWhenActive = .2f;

            return def;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
            lightShapePreview.lux = LUX;
            lightShapePreview.radius = RANGE;
            lightShapePreview.shape = LightShape.Circle;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            var prefabId = go.GetComponent<KPrefabID>();
            prefabId.AddTag(RoomConstraints.ConstraintTags.LightSource);
            prefabId.AddTag(RoomConstraints.ConstraintTags.Decor20);
            prefabId.AddTag(GameTags.Decoration);

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
            go.AddOrGet<Spooks>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGet<LoopingSounds>();

            Light2D light2d = go.AddOrGet<Light2D>();
            light2d.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
            light2d.Color = new Color(2, 1.5f, 0.3f, 1);
            light2d.Range = RANGE;
            light2d.shape = LightShape.Circle;
            light2d.Offset = new Vector2(0, 0.5f);
            light2d.drawOverlay = true;
            light2d.Lux = LUX;

            go.AddOrGetDef<LightController.Def>();
        }
    }
}
