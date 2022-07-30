using FUtility;
using TUNING;

namespace Backwalls.Buildings
{
    public static class BackwallTemplate
    {
        public static BuildingDef CreateDef(string ID, string anim)
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                anim,
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.WORK_TIME_SECONDS.SHORT_WORK_TIME,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                MATERIALS.ANY_BUILDABLE,
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.Anywhere,
                DECOR.BONUS.TIER1,
                NOISE_POLLUTION.NONE);

            def.ObjectLayer = ObjectLayer.Backwall;
            def.SceneLayer = Mod.Settings.SceneLayer;

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
