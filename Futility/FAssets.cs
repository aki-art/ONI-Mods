using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace FUtility
{
	public class FAssets
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

		public static TextureAtlas GetCustomAtlas(string filePath, TextureAtlas tileAtlas)
		{
			var tex = LoadTexture(filePath);

			if (tex == null)
				return null;

			TextureAtlas atlas;
			atlas = ScriptableObject.CreateInstance<TextureAtlas>();
			atlas.texture = tex;
			atlas.scaleFactor = tileAtlas.scaleFactor;
			atlas.items = tileAtlas.items;
			atlas.name = Path.GetFileNameWithoutExtension(filePath + "_atlas");

			return atlas;
		}

		public static TextureAtlas GetCustomAtlas(string fileName, string folder, TextureAtlas tileAtlas)
		{
			var path = Path.Combine(Utils.ModPath, folder, fileName);
			return GetCustomAtlas(path, tileAtlas);
		}

		public static TextureAtlas GetCustomAtlas(Texture2D texture, TextureAtlas tileAtlas)
		{
			TextureAtlas atlas;
			atlas = ScriptableObject.CreateInstance<TextureAtlas>();
			atlas.texture = texture;
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
