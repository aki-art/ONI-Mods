using FUtility;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace Beached.Patches
{
    internal class TerrainBGPatch
    {
        [HarmonyPatch(typeof(TerrainBG), "OnSpawn")]
        public static class TerrainBG_OnSpawn_Patch
        {
            public static void Postfix(TerrainBG __instance, MaterialPropertyBlock[] ___propertyBlocks)
            {
                var texArray = __instance.backgroundMaterial.GetTexture("images") as Texture2DArray;

                var newArray = new Texture2DArray(texArray.width, texArray.height, texArray.depth + 3, texArray.format, false);

                for (var i = 0; i < texArray.depth; i++)
                {
                    Graphics.CopyTexture(texArray, i, 0, newArray, i, 0);
                }


                // temporary test
                var beach = LoadTexture(Path.Combine(Mod.Path, "assets", "textures", "BGbeach.png"), newArray.format);
                var depths = LoadTexture(Path.Combine(Mod.Path, "assets", "textures", "BGdepths.png"), newArray.format);
                var bamboo = LoadTexture(Path.Combine(Mod.Path, "assets", "textures", "BGbamboo.png"), newArray.format);

                newArray.SetPixels(beach.GetPixels(), newArray.depth - 3);
                newArray.SetPixels(depths.GetPixels(), newArray.depth - 2);
                newArray.SetPixels(bamboo.GetPixels(), newArray.depth - 1);

                newArray.Apply(true);

                __instance.backgroundMaterial.SetTexture("images", newArray);
            }

            public static Texture2D LoadTexture(string path, TextureFormat format, bool warnIfFailed = true)
            {
                Texture2D texture = null;

                if (File.Exists(path))
                {
                    var data = FUtility.Assets.TryReadFile(path);
                    texture = new Texture2D(1, 1, format, false);
                    texture.LoadImage(data);
                }
                else if (warnIfFailed)
                {
                    Log.Warning($"Could not load texture at path {path}.");
                }

                return texture;
            }
        }
    }
}
