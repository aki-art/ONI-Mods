using UnityEngine;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
    class DepletedUraniumSGTConfig : DefaultStainedGlassTileConfig
    {
        private static readonly string name = "DepletedUranium";
        new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = StainedGlassHelper.GetDef(name);
            def.BlockTileMaterial = new Material(def.BlockTileMaterial);
            def.BlockTileMaterial.SetColor("_ShineColour", ModAssets.Colors.uraniumGreen);
            return def;
        }

        public override string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_EXPANSION1_ONLY;
        }
    }
}