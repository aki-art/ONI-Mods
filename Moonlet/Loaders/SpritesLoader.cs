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
		/*
				public override void Reload(MoonletMod mod)
				{
					LoadSprites(Assets.instance, mod, true);
				}
		*/
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

				var metaPath = Path.Combine(path, name + ".meta.yaml");
				var meta = FileUtil.ReadYaml<SpriteMetaData>(metaPath);
				var pivot = meta == null ? Vector2.zero : new Vector2(meta.PivotX, meta.PivotY);
				var sprite = LoadSprite(file, name, pivot);

				if (removeExisting)
					assets.SpriteAssets.RemoveAll(sprite => sprite.name == name);

				assets.SpriteAssets.Add(sprite);


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

		private static Sprite LoadSprite(string path, string spriteName, Vector2 offset)
		{
			var texture = FUtility.Assets.LoadTexture(path, true);

			if (texture == null)
				return null;

			var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), offset);
			sprite.name = spriteName;

			return sprite;
		}
	}
}
