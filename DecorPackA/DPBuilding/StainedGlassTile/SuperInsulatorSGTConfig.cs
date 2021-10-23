namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class SuperInsulatorSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "SuperInsulator";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.getDef(name);
	}
}