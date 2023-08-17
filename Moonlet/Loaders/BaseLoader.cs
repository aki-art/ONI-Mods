namespace Moonlet.Loaders
{
	public class BaseLoader
	{
		public const string
			ELEMENTS = "elements",
			GEYSERS = "geysers",
			ITEMS = "items",
			RECIPES = "recipes",
			EFFECTS = "effects",
			SOUNDS = "sounds",
			SPRITES = "sprites",
			FX_ANIMS = "fx",
			TEXTURES = "textures",
			WORLDGEN = "worldgen",
			ZONETYPES = "zonetypes",
			KEYS = "keys.yaml";

		public string path;
		public MoonletData data;
		public string title;
		public string staticID;

		public BaseLoader(KMod.Mod mod, MoonletData data)
		{
			this.data = data;
			staticID = mod.staticID;
			title = mod.title;
			path = mod.ContentPath;
		}
	}
}
