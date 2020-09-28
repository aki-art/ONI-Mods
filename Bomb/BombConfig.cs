using FUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace Bomb
{
    public class BombConfig : IBuildingConfig, IModdedBuilding
    {
        public static readonly string ID = "AB_Bomb";
        public MBInfo Info => new MBInfo(ID, "Base");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 1,
               anim: "frost_burger_kanim",
               hitpoints: 100,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.RAW_METALS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: new EffectorValues(20, 8),
               noise: NOISE_POLLUTION.NONE
           );

            def.Floodable = true;
            def.Entombable = true;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "off";
            def.PermittedRotations = PermittedRotations.Unrotatable;
            def.DefaultAnimState = "object";

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<Bomb>();
        }
    }
}
