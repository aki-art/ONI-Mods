namespace DecorPackA.Buildings.StainedGlassTile
{
    class CopperSGTConfig : DefaultStainedGlassTileConfig
    {
        private static readonly string name = "Copper";
        new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
    }
}