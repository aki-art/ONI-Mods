using System.IO;
using System.Reflection;
using UnityEngine;

namespace FUtility
{
    public class Assets
    {
        public static Texture2D LoadTexture(string name, string directory = null)
        {
            Texture2D texture = null;
            if (directory == null)
                directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
            var texFile = Path.Combine(directory, name + ".png");

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                texture = new Texture2D(1, 1);
                texture.LoadImage(data);
            }
            else
                Debug.LogError($"Could not load texture at path {texFile}.");
            return texture;
        }
        /*
        public static TextureAtlas GetCustomAtlas(string fileName, string folder, TextureAtlas tileAtlas)
        {
            string path = Utils.ModPath;
            if (folder != null)
                path = Path.Combine(path, folder);
            var tex = LoadTexture(fileName, path);
            if (tex == null) return null;

            TextureAtlas atlas;
            atlas = ScriptableObject.CreateInstance<TextureAtlas>();
            atlas.texture = tex;
            atlas.vertexScale = tileAtlas.vertexScale;
            atlas.items = tileAtlas.items;

            return atlas;
        }
        */

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
            AssetBundle assetBundle =  AssetBundle.LoadFromFile(path);

            if (assetBundle == null)
            {
                Log.Warning($"Failed to load AssetBundle from path {path}");
                return null;
            }

            return assetBundle;
        }

    }
}
