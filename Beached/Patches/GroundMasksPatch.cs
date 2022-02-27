using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Beached.Patches
{
    internal class GroundMasksPatch
    {
        [HarmonyPatch(typeof(GroundMasks), "Initialize")]
        public static class GroundMasks_Initialize_Patch
        {
            public static void Prefix(GroundMasks __instance)
            {
                Log.Debuglog("GroundMasks ------------------------- ");
                var atlas = __instance.maskAtlas;

                if (atlas is null)
                {
                    return;
                }

                var items = new List<TextureAtlas.Item>(atlas.items);

                foreach (var item in atlas.items)
                {
                    if (item.name.Contains("sand_stone"))
                    {
                        var beach = new TextureAtlas.Item
                        {
                            indices = item.indices,
                            name = item.name.Replace("sand_stone", "beach"),
                            uvs = item.uvs,
                            uvBox = item.uvBox,
                            vertices = item.vertices
                        };

                        items.Add(beach);


                        var depths = new TextureAtlas.Item
                        {
                            indices = item.indices,
                            name = item.name.Replace("sand_stone", "depths"),
                            uvs = item.uvs,
                            uvBox = item.uvBox,
                            vertices = item.vertices
                        };

                        items.Add(depths);
                    }
                }

                atlas.items = items.ToArray();

                // Log.Debuglog(atlas.texture.name);

                if (atlas.texture != null)
                {
                    SaveImage(atlas.texture);

                }
            }

            public static void Postfix(GroundMasks __instance)
            {
                foreach (var item in __instance.biomeMasks)
                {
                    Log.Debuglog("MASK: " + item.Key);
                }
            }

            private static void SaveImage(Texture2D textureToWrite)
            {
                Log.Debuglog("...");
                var texture2D = new Texture2D(textureToWrite.width, textureToWrite.height, TextureFormat.RGBA32, false);

                var renderTexture = new RenderTexture(textureToWrite.width, textureToWrite.height, 32);
                Graphics.Blit(textureToWrite, renderTexture);

                texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture2D.Apply();

                var bytes = texture2D.EncodeToPNG();
                var dirPath = Mod.Path;

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                File.WriteAllBytes(Path.Combine(dirPath, "Image") + ".png", bytes);

                Log.Debuglog("Saved to " + dirPath);
            }
        }
    }
}
