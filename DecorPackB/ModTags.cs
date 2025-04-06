namespace DecorPackB
{
	public class ModTags
	{
		public static Tag
			floorLampPaneMaterial = TagManager.Create("DecorPackB_FloorLampPaneMaterial"),
			floorLamp = TagManager.Create("DecorPackB_FloorLamp"),
			noPaint = TagManager.Create("NoPaint"), // MaterialColor mod uses this
			noBackwall = TagManager.Create("NoBackwall"); // Background Tiles mod uses this

		public static TagSet frameMaterials = [];
	}
}
