namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class SaltSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Salt";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
	}
}