using FUtility;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Moonlet
{
	public class ModLoader
	{
		public const string
			ELEMENTS = "elements",
			GEYSERS = "geysers",
			ITEMS = "items",
			RECIPES = "recipes",
			SOUNDS = "sounds",
			SPRITES = "sprites",
			TEXTURES = "textures",
			WORLDGEN = "worldgen",
			ZONETYPES = "zonetypes",
			KEYS = "keys.yaml";

		public MoonletData data;
		public string path;
		public string title;
		public string staticID;

		public string TranslationsFolder => Path.Combine(path, data.TranslationsPath);

		public string SpritesFolder => Path.Combine(path, data.AssetsPath, SPRITES);

		public string SoundsFolder => Path.Combine(path, data.AssetsPath, SOUNDS);

		public string ElementsFolder => Path.Combine(path, data.DataPath, ELEMENTS);

		public string ElementTexturesFolder => Path.Combine(path, data.AssetsPath, ELEMENTS);

		public string WorldgenFolder => Path.Combine(path, data.DataPath, WORLDGEN);

		public string ZoneTypesFolder => Path.Combine(path, data.DataPath, ZONETYPES);

		public ModLoader(KMod.Mod mod, MoonletData data)
		{
			this.data = data;
			staticID = mod.staticID;
			title = mod.title;
			path = mod.ContentPath;
			Log.Info($"Initializing Moonlet mod {mod.title} ({mod.staticID})");
		}

		public void OnAllModsLoaded()
		{
		}

		public void LoadSprites(Assets assets)
		{
			LoadSprites(assets, SpritesFolder);
		}

		private void LoadSprites(Assets assets, string path)
		{
			if (!Directory.Exists(path))
			{
				Log.Warning($"Sprites directory defined, but not found: {path}");
				return;
			}

			var count = 0;

			foreach (var file in Directory.GetFiles(path, "*.png"))
			{
				var name = Path.GetFileNameWithoutExtension(file);
				var sprite = LoadSprite(file, name);
				assets.SpriteAssets.Add(sprite);

				var metaPath = Path.Combine(Path.GetDirectoryName(path), name + ".meta.yaml");
				var meta = FileUtil.Read<AssetMetaData>(metaPath);

				if (meta != null)
				{
					if (!meta.ColorHex.IsNullOrWhiteSpace())
					{
						assets.TintedSpriteAssets.Add(new TintedSprite()
						{
							sprite = sprite,
							name = name,
							color = Util.ColorFromHex(meta.ColorHex)
						});

						if (data.DebugLogging)
							Log.Info($"(debug) Meta data loaded for {name}");
					}
					else if (data.DebugLogging)
						Log.Warning($"(debug) Meta data exists for sprite {name}, but it's empty.");
				}

				count++;
			}

			if (count > 0)
				Log.Info($"Loaded {count} sprites for {title}");
			else if (data.DebugLogging)
				Log.Warning($"(debug) No sprites were found in {path}");
		}

		public class AssetMetaData
		{
			public string ColorHex { get; set; } = "FFFFFF";
		}

		private static Sprite LoadSprite(string path, string spriteName)
		{
			var texture = FUtility.Assets.LoadTexture(path, true);

			if (texture == null)
				return null;

			var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector3.zero);
			sprite.name = spriteName;

			return sprite;
		}

		public void LoadTranslations()
		{
			var keysFile = Path.Combine(TranslationsFolder, KEYS);

			var defaultKeys = FileUtil.Read<Dictionary<string, string>>(keysFile);
			if (defaultKeys != null)
			{
				foreach (var key in defaultKeys)
					Strings.Add(key.Key, key.Value);
			}

			var translationsFolder = TranslationsFolder;

			if (File.Exists(translationsFolder))
			{
				if (LoadStrings(translationsFolder))
					Log.Debuglog($"Loaded translations for {staticID}");
			}
		}

		// Loads user created translations
		private static bool LoadStrings(string translationsPath)
		{
			var code = Localization.GetLocale()?.Code;

			if (code.IsNullOrWhiteSpace())
				code = "en";

			var path = Path.Combine(translationsPath, "translations", code + ".po");

			if (File.Exists(path))
			{
				Localization.OverloadStrings(Localization.LoadStringsFile(path, false));
				Log.Info($"Loaded translation file for {code}.");

				return true;
			}

			return false;
		}
	}
}
