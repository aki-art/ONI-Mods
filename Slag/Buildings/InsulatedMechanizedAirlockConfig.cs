using STRINGS;
using TUNING;
using UnityEngine;

namespace Slag.Buildings
{
    class InsulatedMechanizedAirlockConfig : IBuildingConfig
    {
        public static readonly string ID = "MechanizedInsulatingDoor";

        public static LocString NAME = "Mechanized Insulating Door";
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
               anim: "mechanizedInsulationDoor_kanim",
               hitpoints: TUNING.BUILDINGS.HITPOINTS.TIER1,
               construction_time: TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
               construction_mass: new float[] { 400f, 4f },
               construction_materials: new string[] { "RefinedMetal", ModAssets.slagWoolTag.ToString() },
               melting_point: 1600f,
               build_location_rule: BuildLocationRule.Tile,
               decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2,
               noise: NOISE_POLLUTION.NONE
           );

            def.Overheatable = false;
            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 120f;
            def.Floodable = false;
            def.Entombable = false;
            def.IsFoundation = true;
            def.ViewMode = OverlayModes.Power.ID;
            def.TileLayer = ObjectLayer.FoundationTile;
            def.AudioCategory = "Metal";
            def.PermittedRotations = PermittedRotations.R90;
            def.SceneLayer = Grid.SceneLayer.Building;
            def.ForegroundLayer = Grid.SceneLayer.InteriorWall;
            def.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
            def.ThermalConductivity = .025f;

            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Door door = go.AddOrGet<Door>();
            door.hasComplexUserControls = true;
            door.unpoweredAnimSpeed = 0.65f;
            door.poweredAnimSpeed = 5f;
            door.doorClosingSoundEventName = "MechanizedAirlock_closing";
            door.doorOpeningSoundEventName = "MechanizedAirlock_opening";

            go.AddOrGet<ZoneTile>();
            go.AddOrGet<AccessControl>();
            go.AddOrGet<KBoxCollider2D>();

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
            go.AddOrGet<Workable>().workTime = 5f;
            go.GetComponent<AccessControl>().controlEnabled = true;

            KBatchedAnimController kanim = go.GetComponent<KBatchedAnimController>();
            kanim.initialAnim = "closed";
            kanim.fgLayer = Grid.SceneLayer.BuildingFront;

            Prioritizable.AddRef(go);
            Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
        }
    }
}
