namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class AluminumSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Aluminum";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
	}
}