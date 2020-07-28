using TUNING;
using UnityEngine;
using Utils;

namespace Curtain
{
    [StringsPath(typeof(CurtainStrings.BUILDINGS.PREFABS.PLASTICCURTAIN))]
    [BuildMenu("Base")]
    class PlasticCurtainConfig : IBuildingConfig
    {
        public static readonly string ID = "AC_PlasticCurtain";
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                 id: ID,
                 width: 1,
                 height: 2,
                 anim: "stripdooroverlay_kanim",
                 hitpoints: BUILDINGS.HITPOINTS.TIER1,
                 construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                 construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                 construction_materials: MATERIALS.PLASTICS,
                 melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
                 build_location_rule: BuildLocationRule.Tile,
                 decor: BUILDINGS.DECOR.BONUS.TIER0,
                 noise: NOISE_POLLUTION.NONE
              );

            def.Overheatable = false;
            def.Floodable = false;
            def.Entombable = false;
            def.IsFoundation = true;
            def.TileLayer = ObjectLayer.FoundationTile;
            def.AudioCategory = "Glass";
            def.PermittedRotations = PermittedRotations.R90;
            def.SceneLayer = Grid.SceneLayer.Ground;
            def.ForegroundLayer = Grid.SceneLayer.Ground;
            def.ThermalConductivity = 5f;
            def.UseStructureTemperature = true;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Prioritizable.AddRef(go);

            Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            go.AddOrGet<ZoneTile>();
            go.AddOrGet<KBoxCollider2D>();
            go.AddOrGet<Workable>().workTime = 1f;

            go.AddOrGet<Curtain>();
        }
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<KBatchedAnimController>().initialAnim = "closed";

        }

    }
}
