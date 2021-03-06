﻿using FUtility;
using TUNING;
using UnityEngine;

namespace WorldCreep.Buildings
{
    public class SeismoGraphConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Tuning.PREFIX + "SeismoGraph";
        public MBInfo Info => new MBInfo(ID, "Base");

        public override BuildingDef CreateBuildingDef()
        {

            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 1,
               anim: "meteor_detector_kanim",
               hitpoints: BUILDINGS.HITPOINTS.TIER2,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
               construction_materials:MATERIALS.RAW_METALS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: BUILDINGS.DECOR.BONUS.TIER3,
               noise: NOISE_POLLUTION.NONE
           );

            def.AudioCategory = "Glass";
            def.ViewMode = OverlayModes.Decor.ID;
            def.PermittedRotations = PermittedRotations.FlipH;

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 120f;
            def.SelfHeatKilowattsWhenActive = .2f;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<SeismoGraph>();
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGet<LoopingSounds>();
        }
    }
}
