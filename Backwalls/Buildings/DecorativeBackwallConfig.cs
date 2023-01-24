using Backwalls.Cmps;
using TUNING;
using UnityEngine;

namespace Backwalls.Buildings
{
    public class DecorativeBackwallConfig : IBuildingConfig
    {
        public const string ID = "Backwall_DecorativeBackwall";

        public override BuildingDef CreateBuildingDef()
        {
            return BackwallTemplate.CreateDef(
                ID,
                "decorative_backwall_kanim",
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY,
                MATERIALS.RAW_MINERALS);
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override  void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);
            go.AddComponent<BackwallLink>();
            go.AddComponent<Backwall>();
        }
    }
}
