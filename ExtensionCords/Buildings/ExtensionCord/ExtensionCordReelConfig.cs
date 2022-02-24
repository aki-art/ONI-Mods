using FUtility;
using FUtility.BuildingUtil;
using TUNING;
using UnityEngine;

namespace ExtensionCords.Buildings.ExtensionCord
{
    public class ExtensionCordReelConfig : IBuildingConfig, IModdedBuilding
    {
        public const string ID = "ExtensionCords_ExtensionCordReel";

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.POWER);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               1,
               1,
               "farmtile_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
               MATERIALS.RAW_METALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               BUILDINGS.DECOR.PENALTY.TIER1,
               NOISE_POLLUTION.NONE
           );

            def.AudioCategory = Consts.AUDIO_CATEGORY.METAL;
            def.ViewMode = OverlayModes.Power.ID;

            def.RequiresPowerInput = true;
            def.ExhaustKilowattsWhenActive = 0;
            def.EnergyConsumptionWhenActive = 0;
            def.SelfHeatKilowattsWhenActive = 0;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            //go.AddOrGet<EnergyConsumer>();
            //go.AddOrGet<LoopingSounds>();

            go.AddComponent<PlaceableOutlet>();

            Battery battery = go.AddOrGet<Battery>();
            battery.capacity = 1000f;
            battery.joulesLostPerSecond = 0;

            //go.AddComponent<MoodLamp>();
            //go.AddOrGetDef<PoweredController.Def>();
        }
    }
}
