using System;
using UnityEngine;

namespace Backwalls
{
	public class SpriteHelper
	{
		public static Sprite GetSpriteForAtles(TextureAtlas atlas)
		{
			var cropped = GetUITexture(atlas);

			return Sprite.Create(cropped, new Rect(0, 0, cropped.width, cropped.height), Vector3.zero, 100);
		}

		public static Texture2D GetUITexture(TextureAtlas atlas)
		{
			var uvBox = GetUVBox(atlas);

			var tw = atlas.texture.width;
			var tex = atlas.texture;

			var renderTexture = new RenderTexture(tex.width, tex.height, 32);
			Graphics.Blit(tex, renderTexture);

			var xo = (int)(uvBox.x * tw);
			var size = (int)(uvBox.z * tw - xo);

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