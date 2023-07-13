using FUtility;
using Moonlet.Loaders;
using System.Collections.Generic;

namespace Moonlet
{
	public class ModLoader
	{
		public static Dictionary<string, string> locstringKeys;

		public ModElementLoader elementLoader;
		public ModSpriteLoader spriteLoader;
		public ModTranslationsLoader translationsLoader;
		public ModZoneTypeLoader zoneTypesLoader;

		public ModLoader(KMod.Mod mod, MoonletData data)
		{
			locstringKeys = new();

			elementLoader = new(mod, data);
			spriteLoader = new(mod, data);
			translationsLoader = new(mod, data);
			zoneTypesLoader = new(mod, data);

			Log.Info($"Initializing Moonlet mod {mod.title} ({mod.staticID})");
		}

		public void OnAllModsLoaded()
		{
			elementLoader.CollectElementsFromYAML();
		}
	}
}
