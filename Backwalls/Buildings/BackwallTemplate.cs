using FUtility;
using TUNING;

namespace Backwalls.Buildings
{
    // building def used by both variants of the backwall
    public static class BackwallTemplate
    {
        public static BuildingDef CreateDef(string ID, string anim, float[] mass, string[] materials)
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                anim,
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.WORK_TIME_SECONDS.SHORT_WORK_TIME,
                mass,
                materials,
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.Anywhere,
                new EffectorValues(5, 1),
                NOISE_POLLUTION.NONE);

            def.ObjectLayer = ObjectLayer.Backwall;
            def.SceneLayer = Mod.sceneLayer;

            def.Floodable = false;
            def.Breakable = false;
            def.Entombable = false;
            def.Overheatable = false;

            def.AudioCategory = Consts.AUDIO_CATEGORY.PLASTIC;
            def.AudioSize = AUDIO.SIZE.SMALL;

            def.BaseTimeUntilRepair = -1f;

            return def;
        }
    }
}
