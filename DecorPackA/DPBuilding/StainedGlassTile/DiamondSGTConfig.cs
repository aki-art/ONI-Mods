/*using UnityEngine;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
    class DiamondSGTConfig : DefaultStainedGlassTileConfig
    {
        private static readonly string name = "Diamond";
        new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = StainedGlassHelper.GetDef(name);
            def.BlockTileMaterial = new Material(def.BlockTileMaterial);
            def.BlockTileMaterial.SetColor("_ShineColour", ModAssets.Colors.lavender);
            return def;
        }
    }
}*/