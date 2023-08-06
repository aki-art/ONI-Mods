using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace FUtility
{
	public class Assets
	{
		public static Texture2D LoadTexture(string name, string directory)
		{
			if (directory == null)
			{
				directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
			}

			string path = Path.Combine(directory, name + ".png");

			return LoadTexture(path);
		}

		public static void LoadSprites(global::Assets assets, string path = null)
		{
			path = path.IsNullOrWhiteSpace() ? Path.Combine(Utils.ModPath, "assets", "sprites") : path;

			if (!Directory.Exists(path))
				return;

			foreach (var file in Directory.GetFiles(path, "*.png"))
			{
				var name = Path.GetFileNameWithoutExtension(file);
				var sprite = LoadSprite(file, name);
				assets.SpriteAssets.Add(sprite);

				var metaPath = Path.Combine(Path.GetDirectoryName(path), name + ".meta.json");
				if (File.Exists(metaPath))
				{
					var json = File.ReadAllText(metaPath);
					if (json != null)
					{
						var data = JsonConvert.DeserializeObject<AssetMetaData>(json);
						if (data.TintedSprite)
						{
							assets.TintedSpriteAssets.Add(new TintedSprite()
							{
								sprite = sprite,
								name = name,
								color = Util.ColorFromHex(data.ColorHex)
							});
						}
					}
				}
			}
		}

		public class AssetMetaData
		{
			public bool TintedSprite { get; set; }

			public string ColorHex { get; set; } = "FFFFFF";
		}

		private static Sprite LoadSprite(string path, string spriteName)
		{
			var texture = LoadTexture(path, true);

			if (texture == null)
			{
				return null;
			}

			var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector3.zero);
			sprite.name = spriteName;

			return sprite;
		}

		public static bool TryLoadTexture(string path, out Texture2D texture)
		{
			texture = LoadTexture(path, false);
			return texture != null;
		}

		public static Texture2D LoadTexture(string path, bool warnIfFailed = true)
		{
			Texture2D texture = null;

			if (File.Exists(path))
			{
				byte[] data = TryReadFile(path);
				texture = new Texture2D(1, 1);
				texture.LoadImage(data);
			}
			else if (warnIfFailed)
			{
				Log.Warning($"Could not load texture at path {path}.");
			}

			return texture;
		}

		public static byte[] TryReadFile(string texFile)
		{
			try
			{
				return File.ReadAllBytes(texFile);
			}
			catch (Exception e)
			{
				Log.Warning("Could not read file: " + e);
				return null;
			}
		}

		public static TextureAtlas GetCustomAtlas(string fileName, string folder, TextureAtlas tileAtlas)
		{
			string path = Utils.ModPath;

			if (folder != null)
			{
				path = Path.Combine(path, folder);
			}

			var tex = LoadTexture(fileName, path);

			if (tex == null)
			{
				return null;
			}

			TextureAtlas atlas;
			atlas = ScriptableObject.CreateInstance<TextureAtlas>();
			atlas.texture = tex;
			atlas.scaleFactor = tileAtlas.scaleFactor;
			atlas.items = tileAtlas.items;

			return atlas;
		}


		public static AssetBundle LoadAssetBundle(string assetBundleName, string path = null, bool platformSpecific = false)
		{
			foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
			{
				if (bundle.name == assetBundleName)
				{
					return bundle;
				}
			}

			if (path.IsNullOrWhiteSpace())
			{
				path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
			}

			if (platformSpecific)
			{
				switch (Application.platform)
				{
					case RuntimePlatform.WindowsPlayer:
						path = Path.Combine(path, "windows");
						break;
					case RuntimePlatform.LinuxPlayer:
						path = Path.Combine(path, "linux");
						break;
					case RuntimePlatform.OSXPlayer:
						path = Path.Combine(path, "mac");
						break;
				}
			}

			path = Path.Combine(path, assetBundleName);

			var assetBundle = AssetBundle.LoadFromFile(path);

			if (assetBundle == null)
			{
				Log.Warning($"Failed to load AssetBundle from path {path}");
				return null;
			}

			return assetBundle;
		}

		public static Mesh Square(GameObject parent, float width = 1f, float height = 1f)
		{
			MeshFilter meshFilter = parent.AddComponent<MeshFilter>();
			Mesh mesh = new Mesh
			{
				vertices = new Vector3[4]
				{
					new Vector3(0, 0, 0),
					new Vector3(width, 0, 0),
					new Vector3(0, height, 0),
					new Vector3(width, height, 0)
				},

				triangles = new int[6]
				{
					0, 2, 1,
					2, 3, 1
				},

				normals = new Vector3[4]
				{
					-Vector3.forward,
					-Vector3.forward,
					-Vector3.forward,
					-Vector3.forward
				},

				uv = new Vector2[4]
				{
					new Vector2(0, 0),
					new Vector2(1, 0),
					new Vector2(0, 1),
					new Vector2(1, 1)
				}
			};

			meshFilter.mesh = mesh;

			return mesh;
		}

		public static void SaveImage(Texture textureToWrite, string path)
		{
			var texture2D = new Texture2D(textureToWrite.width, textureToWrite.height, TextureFormat.RGBA32, false);

			var renderTexture = new RenderTexture(textureToWrite.width, textureToWrite.height, 32);
			Graphics.Blit(textureToWrite, renderTexture);

			texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
			texture2D.Apply();

			var bytes = texture2D.EncodeToPNG();

			File.WriteAllBytes(path, bytes);

			Log.Info("Saved image to " + path);
		}
	}
}
