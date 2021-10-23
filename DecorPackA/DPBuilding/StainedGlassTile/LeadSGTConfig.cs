namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class LeadSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Lead";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
	}
}