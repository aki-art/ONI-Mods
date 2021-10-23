namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class GoldSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Gold";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
	}
}