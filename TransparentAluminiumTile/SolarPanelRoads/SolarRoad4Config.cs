using UnityEngine;

namespace TransparentAluminium.SolarPanelRoads
{
    public class SolarRoad4Config : IBuildingConfig
    {
        public static int level = 4;
        public static string ID = "TAT_SolarPanelRoad" + level;

        public override BuildingDef CreateBuildingDef()
        {
            return Template.CreateBuildingDef(
                ID,
                new float[] { 1600f, 800f, 1600f },
                new string[] { "Diamond", "RefinedMetal", "Ceramic" },
                "farmtilerotating",
                Tuning.GetLeveledMaxWatt(level));
        }

        public override void DoPostConfigureComplete(GameObject go) => Template.DoPostConfigureComplete( go, level, null);
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) => Template.ConfigureBuildingTemplate(go, prefab_tag);
        public override void DoPostConfigureUnderConstruction(GameObject go) => Template.DoPostConfigureUnderConstruction(go);
    }
}
