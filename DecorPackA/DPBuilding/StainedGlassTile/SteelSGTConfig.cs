namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class SteelSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Steel";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.getDef(name);
	}
}
