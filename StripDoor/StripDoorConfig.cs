using TUNING;
using UnityEngine;

namespace StripDoor
{
    public class StripDoorConfig : IBuildingConfig
    {
        public static readonly string ID = "StripDoor";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                 id: ID,
                 width: 1,
                 height: 2,
                 anim: "stripdoor_kanim",
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
            def.BlockTileIsTransparent = true;
            def.TileLayer = ObjectLayer.FoundationTile;
            def.ViewMode = OverlayModes.Decor.ID;
            def.AudioCategory = "Glass";
            def.PermittedRotations = PermittedRotations.R90;
            def.SceneLayer = Grid.SceneLayer.BuildingFront;
            def.ForegroundLayer = Grid.SceneLayer.BuildingFront;
            def.isSolidTile = false;
            def.ThermalConductivity = .25f;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Prioritizable.AddRef(go);

            Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            Door door = go.AddOrGet<Door>();
            door.unpoweredAnimSpeed = .25f;
            door.doorType = Door.DoorType.ManualPressure;
            door.doorOpeningSoundEventName = "Open_DoorInternal";
            door.doorClosingSoundEventName = "Close_DoorInternal";


        go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 2f;
            go.AddOrGet<TileTemperature>();
            go.AddOrGet<SimCellOccupier>().setTransparent = true;

            go.GetComponent<KPrefabID>().AddTag(GameTags.Window, false);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<AccessControl>().controlEnabled = true;

            KBatchedAnimController kBatched = go.GetComponent<KBatchedAnimController>();
            kBatched.sceneLayer = Grid.SceneLayer.BuildingFront;
            kBatched.initialAnim = "closed";

            go.AddComponent<StripDoor>();
        }
    }
}
