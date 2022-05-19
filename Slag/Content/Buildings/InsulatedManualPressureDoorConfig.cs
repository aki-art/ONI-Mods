using TUNING;

namespace Slag.Content.Buildings
{
    public class InsulatedManualPressureDoorConfig : ManualPressureDoorConfig
    {
        public new const string ID = "Slag_InsulatedManualPressureDoor";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(ID,
                1,
                2,
                "insulated_manual_door_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
                new[] { 200f, 8f },
                new[] { MATERIALS.METAL, ModAssets.Tags.slagWool.ToString() },
                BUILDINGS.MELTING_POINT_KELVIN.TIER2,
                BuildLocationRule.Tile,
                BUILDINGS.DECOR.PENALTY.TIER3,
                NOISE_POLLUTION.NONE,
                1f);

            def.Overheatable = false;
            def.Floodable = false;
            def.Entombable = false;
            def.IsFoundation = true;
            def.TileLayer = ObjectLayer.FoundationTile;
            def.AudioCategory = AUDIO.CATEGORY.METAL;
            def.PermittedRotations = PermittedRotations.R90;
            def.SceneLayer = Grid.SceneLayer.TileMain;
            def.ForegroundLayer = Grid.SceneLayer.InteriorWall;

            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_gear_LP", NOISE_POLLUTION.NOISY.TIER1);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_open", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_close", NOISE_POLLUTION.NOISY.TIER2);

            return def;
        }
    }
}
