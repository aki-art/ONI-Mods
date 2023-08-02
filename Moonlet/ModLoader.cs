using FUtility;
using Moonlet.Loaders;
using System.Collections.Generic;
using System.IO;

namespace Moonlet
{
	public class ModLoader
	{
		public static Dictionary<string, string> locstringKeys;
		private readonly MoonletData data;
		public ModElementLoader elementLoader;
		public ModSpriteLoader spriteLoader;
		public ModTranslationsLoader translationsLoader;
		public ModZoneTypeLoader zoneTypesLoader;
		public ModMaterialCategoryLoader materialCategoryLoader;
		public ModEntitiesLoader entitiesLoader;
		public ModGeyserLoader geysersLoader;

		public ModLoader(KMod.Mod mod, MoonletData data)
		{
			locstringKeys = new();

			materialCategoryLoader = new(mod, data); // load before elements
			elementLoader = new(mod, data);
			spriteLoader = new(mod, data);
			translationsLoader = new(mod, data);
			zoneTypesLoader = new(mod, data);
			entitiesLoader = new(mod, data);
			geysersLoader = new(mod,data);

			Log.Info($"Initializing Moonlet mod {mod.title} ({mod.staticID})");
			this.data = data;
		}

		public void OnAllModsLoaded()
		{
			LoadTags();
		}

		private void LoadTags()
		{
			var path = Path.Combine(data.DataPath, "tags.yaml");
			if (File.Exists(path))
			{
				var tags = FileUtil.Read<Dictionary<string, string>>(path);
				if (tags != null)
				{
					foreach (var tag in tags)
					{
						if (Strings.TryGet(tag.Key, out _))
							continue;

						var key = $"STRINGS.MISC.TAGS.{tag.Key.ToUpperInvariant()}";
						Strings.Add(key, tag.Value);
					}
				}
			}
		}
	}
}
