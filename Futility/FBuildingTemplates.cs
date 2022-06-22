using TUNING;

namespace FUtility
{
    public class FBuildingTemplates
    {
        public static BuildingDef CreateTestingDef(string ID, int width = 1, int height = 1, BuildLocationRule locationRule = BuildLocationRule.Anywhere)
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                width,
                height,
                "farmtile_kanim",
                100,
                5f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                MATERIALS.ALL_METALS,
                1400f,
                locationRule,
                default,
                default);

            return def;
        }
    }
}
