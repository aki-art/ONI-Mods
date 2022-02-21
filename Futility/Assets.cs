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

        public static bool TryLoadTexture(string path, out Texture2D texture)
        {
            texture = LoadTexture(path, false);
            return texture != null;
        }

        public static Texture2D TintTexture(Texture2D texture, Color color)
        {
            return null;
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

        private static byte[] TryReadFile(string texFile)
        {
            try
            {
                return File.ReadAllBytes(texFile);
            }
            catch(Exception e)
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


        public static AssetBundle LoadAssetBundle(string assetBundleName)
        {
            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == assetBundleName)
                {
                    return bundle;
                }
            }

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", assetBundleName);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

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
    }
}
