namespace Moonlet
{
	public class MTags
	{
		public static Tag
			// Add this item to basic metal recipes
			CommonOre = TagManager.Create("CommonOre"),
			ExcludeFromSliderScreen = TagManager.Create("MoonletGenerator"),
			// mark a texture to be melty looking like mud
			Melty = TagManager.Create("Melty"),
			// Add this item to basic metal recipes
			CommonMetal = TagManager.Create("CommonMetal"),
			// destroy if foundation is lost
			DestroyWithoutFoundation = TagManager.Create("DestroyWithoutFoundation"),
			// Immediately destroy this item on spawn
			DestroyOnSpawn = TagManager.Create("DestroyOnSpawn"),
			DisallowTagMerge = TagManager.Create("DisallowTagMerge");
	}
}
