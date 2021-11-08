namespace DecorPackA.Buildings.StainedGlassTile
{
    class IgneousRockSGTConfig : DefaultStainedGlassTileConfig
    {
        private static readonly string name = "IgneousRock";
        new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
    }
}