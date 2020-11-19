using FUtility;
using TUNING;
using UnityEngine;

namespace WorldCreep.Buildings
{
    public class SeismicStabilizerConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Tuning.PREFIX + "SeismicStabilizer";
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
               construction_materials: MATERIALS.RAW_METALS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: BUILDINGS.DECOR.BONUS.TIER3,
               noise: NOISE_POLLUTION.NONE
           );

            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<SeismicStabilizer>().radius = 20;
        }
    }
}
