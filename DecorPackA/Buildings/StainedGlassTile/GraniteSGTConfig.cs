namespace DecorPackA.Buildings.StainedGlassTile
{
    class GraniteSGTConfig : DefaultStainedGlassTileConfig
    {
        private static readonly string name = "Granite";
        new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
    }
}