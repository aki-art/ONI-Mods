using STRINGS;
using TUNING;
using UnityEngine;

namespace Slag.Buildings
{
    public class ManualInsulatedDoorConfig : IBuildingConfig
    {
        public static readonly string ID = "ManualInsulatingDoor";

        public static LocString NAME = "Manual Insulating Door";
        public static LocString DESC = "Airlocks can quarter off dangerous areas and prevent gases from seeping into the colony.";
        public static LocString EFFECT = string.Concat(new string[]
                {
                    "Blocks ",
                    UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
                    " and ",
                    UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
                    " flow, maintaining pressure between areas.\n\nWild ",
                    UI.FormatAsLink("Critters", "CRITTERS"),
                    " cannot pass through doors."
                });
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 2,
               anim: "manualInsulationDoor_kanim",
               hitpoints: 30,
               construction_time: TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
               construction_mass: new float[] { 400f, 4f },
               construction_materials: new string[] { "RefinedMetal", ModAssets.slagWoolTag.ToString() },
               melting_point: 1600f,
               build_location_rule: BuildLocationRule.Tile,
               decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2,
               noise: NOISE_POLLUTION.NONE
           );
            def.Overheatable = false;
            def.Floodable = false;
            def.Entombable = false;
            def.IsFoundation = true;
            def.TileLayer = ObjectLayer.FoundationTile;
            def.AudioCategory = "Metal";
            def.PermittedRotations = PermittedRotations.R90;
            def.ThermalConductivity = .025f;
            def.SceneLayer = Grid.SceneLayer.Building;
            def.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_gear_LP", NOISE_POLLUTION.NOISY.TIER1);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_open", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_close", NOISE_POLLUTION.NOISY.TIER2);
            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.unpoweredAnimSpeed = 1f;
            door.doorType = Door.DoorType.ManualPressure;

            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 5f;

            Prioritizable.AddRef(go);

            Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        }
        public override void DoPostConfigureComplete(GameObject go)
        {
            AccessControl access = go.GetComponent<AccessControl>();
            access.controlEnabled = true;
            KBatchedAnimController kanim = go.GetComponent<KBatchedAnimController>();
            kanim.initialAnim = "closed";
            kanim.fgLayer = Grid.SceneLayer.BuildingFront;
        }
    }
}
