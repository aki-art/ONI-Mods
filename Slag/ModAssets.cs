﻿using System.IO;
using System.Reflection;
using UnityEngine;

namespace Slag
{
    class ModAssets
    {
        public static readonly SimHashes slagSimHash = (SimHashes)Hash.SDBMLower("Slag");
        public static readonly SimHashes slagGlassSimHash = (SimHashes)Hash.SDBMLower("SlagGlass");
        public static readonly Tag slagWoolTag = TagManager.Create("MineralWool");
/*        public static readonly Tag unravelable = TagManager.Create("Unravelable");
        public static readonly Tag unravelMarktag = TagManager.Create("UnravelMark");*/
        public static GameObject pikachu;
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
                Log.Error($"Could not load texture at path {texFile}.");
            return texture;
        }

        public static void OnLoad()
        {
            Debug.Log("LOADING ASSET");
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", "pikachu");
            AssetBundle AssetBundle = AssetBundle.LoadFromFile(path);

            if (AssetBundle == null)
            {
                Debug.LogWarning($"Failed to load AssetBundle from path {path}");
                return;
            }

            pikachu = AssetBundle.LoadAsset<GameObject>("Image");

        }

    }
}
