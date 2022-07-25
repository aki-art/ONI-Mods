using TUNING;
using UnityEngine;

namespace Backwalls.Buildings
{
    public class BackwallConfig : IBuildingConfig
    {
        public const string ID = "Backwall_Backwall";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "floor_glass_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.WORK_TIME_SECONDS.SHORT_WORK_TIME,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                MATERIALS.ANY_BUILDABLE,
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.Anywhere,
                DECOR.BONUS.TIER1,
                NOISE_POLLUTION.NONE);

            def.ObjectLayer = ObjectLayer.Backwall;
            def.Floodable = false;
            def.Breakable = false;
            def.Entombable = false;

            return def;
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddComponent<BackwallPlanner>();
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<BackwallLink>();
            go.AddComponent<Backwall>();
        }
    }
}
