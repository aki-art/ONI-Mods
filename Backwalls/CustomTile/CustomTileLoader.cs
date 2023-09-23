using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Backwalls.CustomTile
{
	public class CustomTileLoader
	{
		public const int TILE_SIZE = 128;
		public const int ATLAS_SIZE = 1024;

		public const string
			MAIN = "main",
			SHINY = "shiny",
			RAINBOW = "rainbow",
			SPEC = "spec";

		private readonly string path;

		private Dictionary<string, (MetaData, Texture2D)> tileDatas;
		public Dictionary<string, TileConfig> additionalTiles;

		public Texture2D tileSampleTexture;
		public Texture2D tileTexture;
		public Texture2D finalTexture;
		public RenderTexture renderTexture;

		public Material material;

		private static float size = 1024f / (TILE_SIZE + 80f);
		private static Vector2 scale = new Vector2(size, size);

		public CustomTileLoader(string path, Material blitMaterial)
		{
			Log.Debug("custom tile loader start");

			this.path = path;
			renderTexture = new RenderTexture(ATLAS_SIZE, ATLAS_SIZE, 32);
			finalTexture = new Texture2D(ATLAS_SIZE, ATLAS_SIZE, TextureFormat.RGBA32, false);
			material = new Material(blitMaterial);
			material.SetTexture("_BorderTex", ModAssets.borderTex);
			material.SetTexture("_Mask", ModAssets.maskTex);

			LoadTileData();
			GenerateTextures();
		}

		private void GenerateTextures()
		{
			if (tileDatas == null) return;

			additionalTiles = new Dictionary<string, TileConfig>();

			var texturesPath = Path.Combine(Path.GetDirectoryName(path), "generated_textures");

			if (!Directory.Exists(texturesPath))
				Directory.CreateDirectory(texturesPath);

			foreach (var tile in tileDatas)
			{
				var mainTexPath = Path.Combine(texturesPath, tile.Key + ".png");
				var metaData = tile.Value.Item1;

				if (File.Exists(mainTexPath))
				{
					var texture = FAssets.LoadTexture(mainTexPath);
					if (texture == null) continue;

					var textures = new Dictionary<string, Texture2D>() 
					{
						{ MAIN, texture }
					};
/*
					if (metaData.Shader?.Name == SHINY)
					{
						var spec = FAssets.LoadTexture(Path.Combine(texturesPath, $"{tile.Key}_{SPEC}.png"));
						if(spec != null)
							textures.Add(SPEC, spec);
					}
*/
					additionalTiles[tile.Key] = new TileConfig()
					{
						textures = textures,
						name = metaData.Name,
						borderTag = metaData.BorderTag,
						//shaderSettings = metaData.Shader
					};
				}
				else
				{
					var textures = GenerateTextures(tile.Value);
					/*
										foreach (var texture in textures)
										{
											var fileName = texture.Key == MAIN ? $"{tile.Key}.png" : $"{tile.Key}_{texture.Key}.png";
											FAssets.SaveImage(texture.Value, Path.Combine(texturesPath, fileName));
										}*/

					FAssets.SaveImage(textures[MAIN], Path.Combine(texturesPath, $"{tile.Key}.png"));

					additionalTiles[tile.Key] = new TileConfig()
					{
						textures = textures,
						name = metaData.Name
					};
				}
			}
		}

		private Dictionary<string, Texture2D> GenerateTextures((MetaData metaData, Texture2D texture) tile)
		{
			var result = new Dictionary<string, Texture2D>
			{
				{ MAIN, GenerateTexture(tile) }
			};

/*			if (tile.metaData.Shader != null)
			{
				switch(tile.metaData.Shader.Name) 
				{
					case SHINY:
						Texture2D value = GenerateTexture(tile, 128f);
						if(value != null)
							result.Add(SPEC, value);
						break;
				}
			}*/

			return result;
		}

		private Texture2D GenerateTexture((MetaData data, Texture2D texture) tile, float offsetX = 0)
		{
			var color = Util.ColorFromHex(tile.data.BorderColorHex);
			material.SetColor("_BorderColor", color);

			GenerateTiledSmallTexture(tile.texture, offsetX);

			material.SetTexture("_TileTex", tileTexture);
			material.SetTexture("_MainTex", tileTexture);
			material.SetTextureScale("_TileTex", scale);
			material.SetTextureOffset("_TileTex", new Vector2(0, 0.07f));

			Graphics.Blit(finalTexture, renderTexture, material);

			var newTex = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
			newTex.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
			newTex.Apply();

			return newTex;
		}

		private void GenerateTiledSmallTexture(Texture2D tileSampleTexture, float offsetX = 0, float offsetY = 0)
		{
			var renderTexture2 = new RenderTexture(128, 128, 32);

			// grab top left 128x128 for main tex
			var scale = new Vector3(128f / tileSampleTexture.width, 128f / tileSampleTexture.height);

			Log.Debug("scale is" + scale);
			Graphics.Blit(
				tileSampleTexture, 
				renderTexture2,
				scale,
				new Vector3(0f, 0));

			/*
                     0  40      128 168

                168  +--------------+
                     |              |
                128  |  +--------+  |
                     |  |        |  |
                     |  |        |  |
                     |  |        |  |
                40   |  +--------+  |
                     |              |
                0    +--------------+

             */

			var top = 168;
			var bottom = 0;
			var left = 0;
			var right = 168;
			var innerBottom = 40;
			var innerLeft = 40;

			var inner = 128;
			var edge = 40;

			tileTexture = new Texture2D(128 + 40 + 40, 128 + 40 + 40, TextureFormat.RGBA32, false);
			tileTexture.ReadPixels(new Rect(0, 0, inner, inner), innerLeft, innerBottom); // middle
			tileTexture.ReadPixels(new Rect(0, 0, edge, inner), right, innerBottom); // right
			tileTexture.ReadPixels(new Rect(128 - 40, 0, edge, inner), 0, innerBottom); // left
			tileTexture.ReadPixels(new Rect(0, 0, inner, edge), innerLeft, bottom); // bottom
			tileTexture.ReadPixels(new Rect(0, 128 - 40, inner, edge), innerLeft, top); // top
			tileTexture.ReadPixels(new Rect(128 - 40, 0, edge, edge), left, bottom); // bottom left corner
			tileTexture.ReadPixels(new Rect(128 - 40, 128 - 40, edge, edge), left, top); // top left corner
			tileTexture.ReadPixels(new Rect(0, 0, edge, edge), right, bottom); // bottom right corner
			tileTexture.ReadPixels(new Rect(0, 128 - 40, edge, edge), right, top); // top right corner

			tileTexture.Apply();
		}

		public static void Copy(string sourceDirectory, string targetDirectory, bool overWrite)
		{
			var diSource = new DirectoryInfo(sourceDirectory);
			var diTarget = new DirectoryInfo(targetDirectory);

			CopyAll(diSource, diTarget, overWrite);
		}

		public static void CopyAll(DirectoryInfo source, DirectoryInfo target, bool overWrite)
		{
			Directory.CreateDirectory(target.FullName);

			// Copy each file into the new directory.
			foreach (var fi in source.GetFiles())
			{

				Log.Info($"Copying {target.FullName}/{fi.Name}");
				string destFileName = Path.Combine(target.FullName, fi.Name);
				if (File.Exists(destFileName) && !overWrite)
					continue;

				fi.CopyTo(destFileName, true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir, overWrite);
			}
		}

		private void LoadTileData()
		{
			var originals = Path.Combine(Utils.ModPath, "assets", "custom_walls_do_not_edit_here");
			Copy(originals, path, false);

			tileDatas = new Dictionary<string, (MetaData, Texture2D)>();

			foreach (var tile in Directory.GetFiles(path, "*.png"))
			{
				var texture = FAssets.LoadTexture(tile);

				if (texture == null) continue;

				var fileName = Path.GetFileNameWithoutExtension(tile);

				var metaDataPath = Path.Combine(path, fileName + ".metadata.json");

				var meta = new MetaData();

				if (File.Exists(metaDataPath))
				{
					var json = File.ReadAllText(metaDataPath);
					meta = JsonConvert.DeserializeObject<MetaData>(json);
				}

				tileDatas[fileName] = (meta, texture);
			}
		}

		public class TileConfig
		{
			public string name;
			public string borderTag;
			public string borderColor;
			//public Texture2D texture;
			public Dictionary<string, Texture2D> textures;
			//public MetaData.ShaderSettings shaderSettings;
		}
	}
}
