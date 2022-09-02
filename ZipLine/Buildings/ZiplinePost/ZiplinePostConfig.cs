using System;
using TUNING;
using UnityEngine;

namespace ZipLine.Buildings.ZiplinePost
{
    public class ZiplinePostConfig : IBuildingConfig
    {
        public const string ID = "Zipline_ZiplinePost";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                3,
                "zipline_post_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
                MATERIALS.RAW_METALS,
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.OnFloor,
                DECOR.NONE,
                NOISE_POLLUTION.NONE);

            def.Floodable = false;
            def.Entombable = true;
            def.Breakable = false;

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<ZiplineAnchor>();
        }
    }
}
