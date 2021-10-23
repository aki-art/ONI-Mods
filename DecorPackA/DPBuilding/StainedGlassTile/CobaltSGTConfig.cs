namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class CobaltSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Cobalt";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.getDef(name);

        public override string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_EXPANSION1_ONLY;
        }
    }
}