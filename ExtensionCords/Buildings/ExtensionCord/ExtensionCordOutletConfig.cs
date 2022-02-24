using FUtility;
using FUtility.BuildingUtil;
using TUNING;
using UnityEngine;

namespace ExtensionCords.Buildings.ExtensionCord
{
    public class ExtensionCordOutletConfig : IBuildingConfig, IModdedBuilding
    {
        public const string ID = "ExtensionCords_ExtensionCordOutlet";

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
               MATERIALS.ALL_METALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.Anywhere,
               BUILDINGS.DECOR.PENALTY.TIER1,
               NOISE_POLLUTION.NONE
           );

            def.AudioCategory = Consts.AUDIO_CATEGORY.METAL;
            def.ViewMode = OverlayModes.Power.ID;

            return def;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<PowerCordNetworkLink>().link1 = CellOffset.none;
        }
    }
}
