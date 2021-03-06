﻿using FUtility;
using TUNING;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.MoodLamp
{
    class MoodLampConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = ModAssets.PREFIX + "MoodLamp";
        public MBInfo Info => new MBInfo(ID, "Furniture", "InteriorDecor", FloorLampConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 2,
               anim: "moodlamp_kanim",
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

            def.RequiresPowerInput = true;
            def.ExhaustKilowattsWhenActive = .5f;
            def.EnergyConsumptionWhenActive = 8f;
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
            var prefabId = go.GetComponent<KPrefabID>();
            prefabId.AddTag(RoomConstraints.ConstraintTags.LightSource, false);
            prefabId.AddTag(RoomConstraints.ConstraintTags.Decor20, false); //??
            prefabId.AddTag(ModAssets.NoPaintTag);
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
