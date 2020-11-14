using UnityEngine;

namespace TransparentAluminium.SolarPanelRoads
{
    public class SolarRoad3Config : IBuildingConfig
    {
        public static int level = 3;
        public static string ID = "TAT_SolarPanelRoad" + level;
        public static float MAX_WATT = Tuning.GetLeveledMaxWatt(level);
        public static float LUX_PER_WATT = Tuning.GetLeveledWPL(level);

        public override BuildingDef CreateBuildingDef()
        {
            return Template.CreateBuildingDef(
                ID,
                new float[] { 800f, 400f, 600f },
                new string[] { "Diamond", "RefinedMetal", "Ceramic" },
                "farmtilerotating",
                MAX_WATT);
        }

        public override void DoPostConfigureComplete(GameObject go) => Template.DoPostConfigureComplete(go, level, SolarRoad4Config.ID);
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => Template.ConfigureBuildingTemplate(go, prefab_tag);
        public override void DoPostConfigureUnderConstruction(GameObject go) => Template.DoPostConfigureUnderConstruction(go);
    }
}
