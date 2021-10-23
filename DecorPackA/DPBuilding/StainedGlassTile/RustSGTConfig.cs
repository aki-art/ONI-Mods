namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class RustSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Rust";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
	}
}