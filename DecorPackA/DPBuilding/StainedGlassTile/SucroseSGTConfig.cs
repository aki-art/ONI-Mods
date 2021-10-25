namespace DecorPackA.DPBuilding.StainedGlassTile
{
    class SucroseSGTConfig : DefaultStainedGlassTileConfig
    {
        private static readonly string name = "Sucrose";
        new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);

        public override string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_EXPANSION1_ONLY;
        }
    }
}