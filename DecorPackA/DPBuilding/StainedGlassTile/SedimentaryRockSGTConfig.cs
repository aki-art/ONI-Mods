namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class SedimentaryRockSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "SedimentaryRock";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.getDef(name);
	}
}