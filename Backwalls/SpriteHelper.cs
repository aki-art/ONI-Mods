using System;
using System.IO;
using UnityEngine;

namespace Backwalls
{
	public class SpriteHelper
	{
		public static Sprite GetSpriteForAtlas(TextureAtlas atlas)
		{
			if (true) //Application.platform != RuntimePlatform.WindowsPlayer)
			{
				// TODO: this is a mess that will work 95% of the time
				// need to figure out why this breaks with the proper cropping
				var uvBox = GetUVBox(atlas);

				var tex = atlas.texture;
				var y = 1f - Mathf.FloorToInt(uvBox.y);
				y = Mathf.Clamp01(y);
				y *= tex.height;
				y -= 208;
				y = Mathf.Clamp(y, 0, 1024 - 208);

				return Sprite.Create(tex, new Rect(0, y, 208, 208), Vector3.zero, 100);
			}

			var cropped = GetUITexture(atlas);

			var cacheFolder = Path.Combine(Utils.ModPath, "ui_cache");
			if (!Directory.Exists(cacheFolder))
				Directory.CreateDirectory(cacheFolder);

			FUtility.Assets.SaveImage(cropped, Path.Combine(cacheFolder, atlas.name + ".png"));

			return Sprite.Create(cropped, new Rect(0, 0, cropped.width, cropped.height), Vector3.zero, 100);
		}

		public static Texture2D GetUITexture(TextureAtlas atlas)
		{
			var uvBox = GetUVBox(atlas);

			var texWidth = atlas.texture.width;
			var tex = atlas.texture;

			var renderTexture = new RenderTexture(tex.width, tex.height, 32);
			Graphics.Blit(tex, renderTexture);

			var xOffset = (int)(uvBox.x * texWidth);
			var size = (int)(uvBox.z * texWidth - xOffset);

			var texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false);
			texture2D.ReadPixels(new Rect(0, 0, size, size), 0, 0);
			texture2D.Apply();

			return texture2D;
		}

		private static Vector4 GetUVBox(TextureAtlas atlas)
		{
			var num3 = atlas.items[0].name.Length - 4 - 8;
			var startIndex = num3 - 1 - 8;
			var uvBox = Vector4.zero;

			for (var k = 0; k < atlas.items.Length; k++)
			{
				var item = atlas.items[k];

				var value = item.name.Substring(startIndex, 8);
				var requiredConnections = Convert.ToInt32(value, 2);

				if (requiredConnections == 0)
				{
					uvBox = item.uvBox;
					break;
				}
			}

			return uvBox;
		}
	}
}