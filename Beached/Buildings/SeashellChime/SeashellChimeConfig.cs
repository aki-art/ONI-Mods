using TUNING;
using UnityEngine;

namespace Beached.Buildings.SeashellChime
{
    internal class SeashellChimeConfig : IBuildingConfig
    {
        public const string ID = "Beached_SeashellChime";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "hammer_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                MATERIALS.ALL_METALS,
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.OnCeiling,
                DECOR.BONUS.TIER2,
                NOISE_POLLUTION.NOISY.TIER1);

            def.AudioCategory = AUDIO.CATEGORY.GLASS;
            def.AudioSize = AUDIO.SIZE.SMALL;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            var chime = go.AddOrGet<Chime>();
            chime.minDelay = 5f / 600f;
            chime.minPressureChange = 25f;

            go.AddTag(GameTags.Decoration);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}
