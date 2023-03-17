using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Backwalls.CustomTile
{
    public class CustomTileLoader
    {
        private readonly string path;

        private Dictionary<string, (MetaData, Texture2D)> tileDatas;
        public Dictionary<string, TileConfig> additionalTiles;

        public Texture2D tileSampleTexture;
        public Texture2D tileTexture;
        public Texture2D finalTexture;
        public RenderTexture renderTexture;

        public Material material;

        private static float size = 1024f / (128f + 80f);
        private static Vector2 scale = new Vector2(size, size);

        public CustomTileLoader(string path, Material blitMaterial)
        {
            Log.Debuglog("custom tile loader start");

            this.path = path;
            renderTexture = new RenderTexture(1024, 1024, 32);
            finalTexture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
            material = new Material(blitMaterial);
            material.SetTexture("_BorderTex", ModAssets.borderTex);
            material.SetTexture("_Mask", ModAssets.maskTex);

            LoadTileData();
            GenerateTextures();
        }

        private void GenerateTextures()
        {
            Log.Debuglog("GenerateTextures");
            if (tileDatas == null) return;

            additionalTiles = new Dictionary<string, TileConfig>();

            var texturesPath = Path.Combine(Path.GetDirectoryName(path), "generated_textures");

            if(!Directory.Exists(texturesPath))
            {
                Directory.CreateDirectory(texturesPath);
            }

            foreach(var tile in tileDatas)
            {
                var texturePath = Path.Combine(texturesPath, tile.Key + ".png");

                if(File.Exists(texturePath))
                {
                    additionalTiles[tile.Key] = new TileConfig()
                    {
                        texture = FUtility.Assets.LoadTexture(texturePath),
                        name = tile.Value.Item1.Name
                    };
                }
                else
                {
                    var texture = GenerateTexture(tile.Value);
                    FUtility.Assets.SaveImage(texture, texturePath);

                    var name = Strings.TryGet(tile.Value.Item1.Name, out var translatedName) ? translatedName.String : tile.Value.Item1.Name;
                    additionalTiles[tile.Key] = new TileConfig()
                    {
                        texture = texture,
                        name = name
                    };
                }
            }
        }

        private Texture2D GenerateTexture((MetaData data, Texture2D texture) tile)
        {
            Log.Debuglog("generating " + tile.data.Name);

            var color = Util.ColorFromHex(tile.data.BorderColorHex);
            material.SetColor("_BorderColor", color);

            GenerateTiledSmallTexture(tile.texture);

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

        private void GenerateTiledSmallTexture(Texture2D tileSampleTexture)
        {
            var renderTexture2 = new RenderTexture(tileSampleTexture.width, tileSampleTexture.height, 32);

            var scale = new Vector2(tileSampleTexture.width / 128f, tileSampleTexture.height / 128f);
            Graphics.Blit(tileSampleTexture, renderTexture2, scale, Vector2.zero);

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

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Log.Info(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void LoadTileData()
        {
            if (!Directory.Exists(path))
            {
                var originals = Path.Combine(Utils.ModPath, "assets", "custom_walls_do_not_edit_here");
                Copy(originals, path);
            }

            tileDatas = new Dictionary<string, (MetaData, Texture2D)>();

            foreach (var tile in Directory.GetFiles(path, "*.png"))
            {
                var texture = FUtility.Assets.LoadTexture(tile);

                if(texture == null) continue;

                var fileName = Path.GetFileNameWithoutExtension(tile);
                var metaDataPath = Path.Combine(path, fileName + ".metadata.json");

                MetaData meta = new MetaData();

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
            public Texture2D texture;
        }
    }
}
