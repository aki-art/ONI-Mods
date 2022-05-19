using TUNING;

namespace Slag.Content.Buildings
{
    public class InsulatedPressureDoorConfig : PressureDoorConfig
    {
        public new const string ID = "Slag_InsulatedPressureDoorConfig";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(ID,
                1,
                2,
                "insulated_mechanic_door_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
                new[] { 200f, 8f },
                new[] { MATERIALS.METAL, ModAssets.Tags.slagWool.ToString() },
                BUILDINGS.MELTING_POINT_KELVIN.TIER2,
                BuildLocationRule.Tile,
                BUILDINGS.DECOR.PENALTY.TIER3,
                NOISE_POLLUTION.NONE,
                1f);


            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 120f;

            def.Overheatable = false;
            def.Floodable = false;
            def.Entombable = false;

            def.IsFoundation = true;

            def.ViewMode = OverlayModes.Power.ID;

            def.TileLayer = ObjectLayer.FoundationTile;
            def.SceneLayer = Grid.SceneLayer.TileMain;
            def.ForegroundLayer = Grid.SceneLayer.InteriorWall;

            def.AudioCategory = AUDIO.CATEGORY.METAL;

            def.PermittedRotations = PermittedRotations.R90;

            def.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));

            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);

            return def;
        }
    }
}
