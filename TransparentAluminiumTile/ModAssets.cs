using FUtility;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TransparentAluminium
{
    class ModAssets
    {
        public static string ModPath;

        public static readonly SimHashes transparentAluminumHash = (SimHashes)Hash.SDBMLower("TransparentAluminum");
        public static readonly Tag TransparentAluminum = TagManager.Create("TransparentAluminum");

        public static TextureAtlas GetCustomAtlas(string fileName, TextureAtlas tileAtlas)
        {
            var tex = GetTexture(fileName, tileAtlas.texture.width, tileAtlas.texture.height);
            if (tex == null) return null;

            TextureAtlas atlas;
            atlas = ScriptableObject.CreateInstance<TextureAtlas>();
            atlas.texture = tex;
            atlas.vertexScale = tileAtlas.vertexScale;
            atlas.items = tileAtlas.items;

            return atlas;
        }

        public static Texture2D GetTexture(string name, int width = 1, int height = 1)
        {
            Texture2D tex = null;
            string texFile = Path.Combine(ModPath, "assets", name) + ".png";

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                tex = new Texture2D(width, height);
                tex.LoadImage(data);
            }

            return tex;
        }
    }
}
