using Moonlet.Asset;
using Moonlet.Utils;
using System.IO;
using UnityEngine;

namespace Moonlet.Loaders
{
	public class SpritesLoader : ContentLoader
	{
		public const string SPRITES = "sprites";

		public SpritesLoader() : base(SPRITES)
		{
		}

		public override void Reload(MoonletMod mod)
		{
			LoadSprites(Assets.instance, mod, true);
		}

		public void LoadSprites(Assets assets, MoonletMod mod, bool removeExisting = false)
		{
			var path = mod.GetAssetPath(this.path);

			if (!Directory.Exists(path))
			{
				Log.Warn($"Sprites directory defined, but not found: {path}", mod.staticID);
				return;
			}

			var count = 0;

			foreach (var file in Directory.GetFiles(path, "*.png"))
			{
				var name = Path.GetFileNameWithoutExtension(file);
				var sprite = LoadSprite(file, name);

				if (removeExisting)
					assets.SpriteAssets.RemoveAll(sprite => sprite.name == name);

				assets.SpriteAssets.Add(sprite);

				var metaPath = Path.Combine(Path.GetDirectoryName(path), name + ".meta.yaml");
				var meta = FileUtil.ReadYaml<SpriteMetaData>(metaPath);

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

						Log.Debug($"Meta data loaded for {name}", mod.staticID);
					}
					else
						Log.Debug($"Meta data exists for sprite {name}, but it's empty.", mod.staticID);
				}

				count++;
			}

			if (count > 0)
				Log.Info($"Loaded {count} sprites for {mod.title}", mod.staticID);
			else
				Log.Debug($"No sprites were found in {path}", mod.staticID);
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
