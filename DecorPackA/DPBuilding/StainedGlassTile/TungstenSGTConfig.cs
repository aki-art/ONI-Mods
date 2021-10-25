namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class TungstenSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Tungsten";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);
	}
}
