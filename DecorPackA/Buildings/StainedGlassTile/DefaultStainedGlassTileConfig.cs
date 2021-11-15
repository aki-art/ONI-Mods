using FUtility;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    // This one exists as a fall back / base for others. should not be buildable in game
    public class DefaultStainedGlassTileConfig : IBuildingConfig, IModdedBuilding
    {
        private const string name = "Default";
        public const string ID = Mod.PREFIX + name + "StainedGlassTile";

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.BASE, "GlassFurnishings", GlassTileConfig.ID);

        public override BuildingDef CreateBuildingDef() => StainedGlassHelper.CreateGlassTileDef(name, ID);

        public override void ConfigureBuildingTemplate(GameObject go, Tag tag) => StainedGlassHelper.ConfigureBuildingTemplate(go, tag);

        public override void DoPostConfigureComplete(GameObject go) => StainedGlassHelper.DoPostConfigureComplete(go);

        public override void DoPostConfigureUnderConstruction(GameObject go) => StainedGlassHelper.DoPostConfigureUnderConstruction(go);
    }
}
