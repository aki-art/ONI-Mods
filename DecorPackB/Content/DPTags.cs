namespace DecorPackB.Content
{
	public class DPTags
	{
		public static readonly Tag
			fossilMaterial = TagManager.Create("DecorPackB_FossilMaterial"),

			// For rooms expanded, it uses this to recognize fossil buildings
			fossilBuilding = TagManager.Create("FossilBuilding"),

			// items need a special tag that is marked as "buildable material" for the game
			buildingFossilNodule = TagManager.Create("DecorPackB_BuildingFossilNodule", STRINGS.ITEMS.DECORPACKB_FOSSILNODULE.NAME),

			digYieldModifier = TagManager.Create("DecorPackB_DigYieldModifier");
	}
}
