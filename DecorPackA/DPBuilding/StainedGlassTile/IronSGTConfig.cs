namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class IronSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Iron";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef() => StainedGlassHelper.getDef(name);
    }
}
