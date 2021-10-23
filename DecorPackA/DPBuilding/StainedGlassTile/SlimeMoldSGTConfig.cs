namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class SlimeMoldSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "SlimeMold";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
	}
}