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

                Texture2DArray newArray = new Texture2DArray(texArray.width, texArray.height, texArray.depth + 1, texArray.format, false);

                for (int i = 0; i < texArray.depth; i++)
                {
                    Graphics.CopyTexture(texArray, i, 0, newArray, i, 0);
                }


                // temporary test
                var biomeTex = LoadTexture(Path.Combine(Mod.Path, "assets", "textures", "BGbeach.png"), newArray.format);

                newArray.SetPixels(biomeTex.GetPixels(), newArray.depth - 1);

                //Graphics.CopyTexture(biomeTex, 0, newArray, newArray.depth - 1);

                newArray.Apply(true);

                __instance.backgroundMaterial.SetTexture("images", newArray);
            }

            public static Texture2D LoadTexture(string path, TextureFormat format, bool warnIfFailed = true)
            {
                Texture2D texture = null;

                if (File.Exists(path))
                {
                    byte[] data = FUtility.Assets.TryReadFile(path);
                    texture = new Texture2D(1, 1, format, false);
                    texture.LoadImage(data);
                }
                else if (warnIfFailed)
                {
                    Log.Warning($"Could not load texture at path {path}.");
                }

                return texture;
            }
            /*
             *                 Log.Debuglog("TerrainBG POST -----------------------");
                Log.Debuglog(__instance.backgroundMaterial?.shader?.name);
                foreach (var block in ___propertyBlocks)
                {
                    Log.Debuglog("   ___propertyBlocks");
                    var tex = __instance.backgroundMaterial.GetTexture("images") as Texture2DArray;

                    foreach (var prop in __instance.backgroundMaterial.GetTexturePropertyNames())
                    {
                        Log.Debuglog("      " + prop);
                    }
                    if (tex != null)
                    {
                        Log.Debuglog("type: " + tex.GetType());
                        Log.Debuglog("texture: " + ((Texture2DArray)tex)?.name);
                    }
                }
             * */
        }
    }
}
