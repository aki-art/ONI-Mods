using FUtility;
using System.IO;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class ModSpriteLoader : BaseLoader
	{
		public string SpritesFolder => Path.Combine(path, data.AssetsPath, SPRITES);

		public ModSpriteLoader(KMod.Mod mod, MoonletData data) : base(mod, data)
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
	}
}
