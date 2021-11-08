using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    class GoldSGTConfig : DefaultStainedGlassTileConfig
    {
        private static readonly string name = "Gold";
        new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = StainedGlassHelper.GetDef(name);
            def.BlockTileMaterial = new Material(def.BlockTileMaterial);
            def.BlockTileMaterial.SetColor("_ShineColour", ModAssets.Colors.gold);
            return def;
        }
    }
}