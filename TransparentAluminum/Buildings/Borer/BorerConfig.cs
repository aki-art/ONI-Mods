using FUtility;
using FUtility.BuildingUtil;
using TUNING;
using UnityEngine;

namespace TransparentAluminum.Buildings.Borer
{
    public class BorerConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.Prefix("Borer");

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.UTILITIES, Consts.TECH.SOLIDS.REFINED_OBJECTS);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "ta_borer_kanim",
                BUILDINGS.HITPOINTS.TIER2,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                new float[]
                {
                    200f,
                    50f
                },
                new string[] 
                { 
                    "Metal", 
                    ModAssets.Tags.Coating.ToString()
                },
                BUILDINGS.MELTING_POINT_KELVIN.TIER4,
                BuildLocationRule.Anywhere,
                DECOR.PENALTY.TIER2,
                NOISE_POLLUTION.NOISY.TIER2);

            def.CanMove = true;
            def.Floodable = false;
            def.Entombable = false;
            def.Disinfectable = false;
            def.ObjectLayer = ObjectLayer.Building;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
        {
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

        }
        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<Borer>().maxIntegrity = 10f;
        }
    }
}
