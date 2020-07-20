using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Asphalt
{
    public class ModAssets
    {
        public static Texture bitumenSubstanceTexture;
        private const string ASSET_BUNDLE_FILE_NAME = "settingsui";
       
        public static class Prefabs
        {
            public static GameObject modSettingsScreenPrefab;
            public static GameObject nukeScreenPrefab;
        }

        public static void LoadTextures()
        {
            bitumenSubstanceTexture = LoadTexture("solid_bitumen");
        }

        // Loads a Unity Assetbundle
        public static void LoadAssetBundle()
        {
            Log.Info("Loading asset files... ");

            foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == ASSET_BUNDLE_FILE_NAME)
                {
                    Log.Info("Asset bundle loaded multiple times. Ignoring duplicates.");
                    return;
                }
            }

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", ASSET_BUNDLE_FILE_NAME);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

            if (assetBundle == null)
            {
                Log.Warning($"Failed to load AssetBundle from path {path}");
                return;
            }

            Prefabs.modSettingsScreenPrefab = assetBundle.LoadAsset<GameObject>("ModSettingsDialog");
            Prefabs.nukeScreenPrefab = assetBundle.LoadAsset<GameObject>("NukeDialog");
        }

        // Thanks for CynicalBusiness for help with this code.
        // Used with permission.
        // Original: https://lab.vevox.io/games/oxygen-not-included/matts-mods/blob/master/IndustrializationFundementals/Building/TileWoodConfig.cs
        // Loads a custom texture atlas.
        public static TextureAtlas GetCustomAtlas(string name, Type type, TextureAtlas tileAtlas)
        {
            var dir = Path.GetDirectoryName(type.Assembly.Location);
            var texFile = Path.Combine(dir, name + ".png");

            TextureAtlas atlas = null;

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(data);

                atlas = ScriptableObject.CreateInstance<TextureAtlas>();
                atlas.texture = tex;
                atlas.vertexScale = tileAtlas.vertexScale;
                atlas.items = tileAtlas.items;
            }
            else
                Debug.LogError($"ASPHALT: Could not load atlas image at path {texFile}.");

            return atlas;
        }

        // Loads a texture file from assembly directory.
        public static Texture2D LoadTexture(string name, string directory = null)
        {
            Texture2D texture = null;
            if (directory == null)
                directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "anim", "assets");
            var texFile = Path.Combine(directory, name + ".png");

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                texture = new Texture2D(1, 1);
                texture.LoadImage(data);
            }
            else
                Debug.LogError($"ASPHALT: Could not load texture at path {texFile}.");
            return texture;
        }

    }
}
