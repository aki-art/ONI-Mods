using UnityEngine;

namespace TransparentAluminium.SolarPanelRoads
{
    public class SolarRoad2Config : IBuildingConfig
    {
        public static int level = 2;
        public static string ID = "TAT_SolarPanelRoad" + level;
        public static float MAX_WATT = Tuning.GetLeveledMaxWatt(level);
        public static float LUX_PER_WATT = Tuning.GetLeveledWPL(level);

        public override BuildingDef CreateBuildingDef()
        {
            return Template.CreateBuildingDef(
                ID,
                new float[] { 2400f, 1200f, 3200f },
                new string[] { "TransparentAluminum", "RefinedMetal", "Ceramic" },
                "farmtilerotating",
                MAX_WATT);
        }

        public override void DoPostConfigureComplete(GameObject go) => Template.DoPostConfigureComplete(go, level, SolarRoad3Config.ID);
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => Template.ConfigureBuildingTemplate(go, prefab_tag);
        public override void DoPostConfigureUnderConstruction(GameObject go) => Template.DoPostConfigureUnderConstruction(go);
    }
}
