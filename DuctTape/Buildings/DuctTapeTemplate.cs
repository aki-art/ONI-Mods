using TUNING;

namespace DuctTape.Buildings
{
    public class DuctTapeTemplate
    {
        public static BuildingDef GetDef(string ID, string anim)
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               1,
               1,
               anim,
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               MATERIALS.TRANSPARENTS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.Anywhere,
               DECOR.PENALTY.TIER0,
               NOISE_POLLUTION.NONE
            );

            def.SceneLayer = Grid.SceneLayer.Backwall;
            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;

            return def;
        }
    }
}
