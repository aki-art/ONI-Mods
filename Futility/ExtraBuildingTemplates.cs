using TUNING;

namespace FUtility
{
    public class ExtraBuildingTemplates
    {
        public static BuildingDef CreateFoundationTileDef(string ID, string anim, int hitPoints, float constructionTime, float[] constructionMass, string[] materials, float meltingPoint, EffectorValues decor)
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                anim,
                hitPoints,
                constructionTime,
                constructionMass,
                materials,
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.Tile,
                decor,
                NOISE_POLLUTION.NONE);

            BuildingTemplates.CreateFoundationTileDef(def);

            def.Floodable = false;
            def.Entombable = false;
            def.Overheatable = false;

            def.BaseTimeUntilRepair = -1f;

            def.SceneLayer = Grid.SceneLayer.TileMain;

            def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;

            def.DragBuild = true;

            return def;
        }
    }
}
