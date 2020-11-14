using FUtility;
using UnityEngine;

namespace TransparentAluminium.SolarPanelRoads
{
    public class SolarRoad0Config : IBuildingConfig, IModdedBuilding
    {
        public static int level = 0;
        public static string ID = "TAT_SolarPanelRoad" + level;
        public static float MAX_WATT = Tuning.GetLeveledMaxWatt(level);
        public static float LUX_PER_WATT = Tuning.GetLeveledWPL(level);

        public MBInfo Info => new MBInfo(ID, "Base");

        public override BuildingDef CreateBuildingDef()
        {
            return Template.CreateBuildingDef(
                ID,
                new float[] { 100f, 50f, 50f },
                new string[] { "Transparent", "RefinedMetal", "Ceramic" },
                "farmtilerotating",
                MAX_WATT);
        }

        public override void DoPostConfigureComplete(GameObject go) => Template.DoPostConfigureComplete(go, level, SolarRoad1Config.ID);
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => Template.ConfigureBuildingTemplate(go, prefab_tag);
        public override void DoPostConfigureUnderConstruction(GameObject go) => Template.DoPostConfigureUnderConstruction(go);
    }
}
