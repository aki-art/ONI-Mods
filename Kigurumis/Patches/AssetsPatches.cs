using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Kigurumis.Patches
{
    public class AssetsPatches
    {
        private const int HAIR_SWAP_HASH = -343348992;

        private static HashSet<string> anims = new HashSet<string>();

        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                var hairs = new AssetsPatches();
                hairs.Generate2();


            }
        }

        [HarmonyPatch(typeof(Assets), "LoadAnims")]
        public class Assets_LoadAnims_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_LoadGroupResourceFile = typeof(KAnimGroupFile).GetMethod("LoadGroupResourceFile");
                var m_CloneAnims = typeof(Assets_LoadAnims_Patch).GetMethod("CloneAnims", BindingFlags.Static | BindingFlags.NonPublic);

                var f_owner = AccessTools.Field(typeof(CreatureCalorieMonitor.Stomach), "owner");

                var index = codes.FindIndex(code => code.operand is MethodInfo m && m == m_LoadGroupResourceFile);

                if (index == -1)
                {
                    return codes;
                }

                codes.InsertRange(index + 1, new[]
                {
                    // Load instance (Assets)
                    new CodeInstruction(OpCodes.Ldarg_0),
                    // call CloneAnims(assets)
                    new CodeInstruction(OpCodes.Call, m_CloneAnims)
                });

                return codes;
            }

            private static void CloneAnims(Assets assets)
            {
                var hairSwap = assets.AnimAssets.Find(a => a.name == "hair_swap_kanim");
                if (hairSwap == null)
                {
                    Log.Warning("no kanim for hair_swap_kanim");
                    return;
                }

                CopyAnim(hairSwap);

                //CloneModded();
            }
        }

        public void Generate2()
        {
            var hoodieHairAnim = Assets.GetAnim("kigurumihood_hair_swap_kanim");
            CropHair(hoodieHairAnim);
        }

        public static class KEEP_PIXEL
        {
            // Define the amount of pixels to keep on each side of the pivot
            public const float FRONT_LEFT = 37f;
            public const float FRONT_RIGHT = 33f;

            public const float BACK_LEFT = 0;
            public const float BACK_RIGHT = 0;

            public const float SIDE_LEFT = 37f;
        }


        private static void CropHair(KAnimFile clonedAnim)
        {
            float texWidth = clonedAnim.GetData().build.GetTexture(0).width;

            foreach (var symbol in clonedAnim.GetData().build.symbols)
            {
                var id = symbol.hash.ToString();

                if (id.StartsWith("hat_h"))
                {
                    CropFrontSymbolFrame(0, texWidth, KEEP_PIXEL.FRONT_LEFT, KEEP_PIXEL.FRONT_RIGHT, symbol);
                    CropBackSymbolFrame(1, symbol);
                    CropSideSymbolFrame(2, texWidth, KEEP_PIXEL.SIDE_LEFT, symbol);
                }
            }
        }

        private static void CropBackSymbolFrame(int index, KAnim.Build.Symbol symbol)
        {
            var frame = symbol.GetFrame(index).symbolFrame;

            frame.bboxMin.x = 0;
            frame.bboxMax.x = 0;
            frame.uvMin.x = 0;
            frame.uvMax.x = 0;
        }

        private static void CropSideSymbolFrame(int index, float texWidth, float leftKeepPixels, KAnim.Build.Symbol symbol)
        {
            var frame = symbol.GetFrame(index).symbolFrame;

            // Code by Romen

            // "bounding box" describes a pivot inside the UV rectangle, so we need to calculate the cropped UVs first
            // But the cropped UV depends on the size we want to keep around the original pivot, so we have to find where that is to determine the cropped UVs
            // We have to determine things in this order:
            // 1. Find the X position inside the texture of the original pivot
            // 2. Set new UVs around that X position
            // 3. Find an entirely new "bounding box" that is relative to the new UVs that puts the pivot in the same texture coordinates as before

            // 1 : Find the pivot X coordinate in the texture
            var kleiPivotX = (frame.bboxMin.x + frame.bboxMax.x) / 2f;
            var kleiPivotWidth = frame.bboxMax.x - frame.bboxMin.x;
            var pivotXPercent = 0.5f - kleiPivotX / kleiPivotWidth; // 0 is left, 0.5 is center, 1 is right
            var uvLeft = frame.uvMin.x;
            var uvRight = frame.uvMax.x;
            var uvWidth = uvRight - uvLeft;
            var texturePivotX = (uvLeft + pivotXPercent * uvWidth) * texWidth;

            // 2 : Set the new UVs around the pivot
            var newLeft = texturePivotX - leftKeepPixels;
            frame.uvMin.x = newLeft / texWidth;

            var rightKeepPixels = texWidth - texturePivotX;
            //var newRight = texturePivotX + rightKeepPixels;
            //frame.uvMax.x = newRight / texWidth;

            // 3 : Set "bounding box" for the new UVs
            var newWidth = leftKeepPixels + rightKeepPixels;
            var newKleiPivotWidth = newWidth * 2f;
            var newKleiPivotX = (0.5f - leftKeepPixels / newWidth) * newKleiPivotWidth;

            frame.bboxMin.x = newKleiPivotX - newKleiPivotWidth * 0.5f;
            //frame.bboxMax.x = newKleiPivotX + newKleiPivotWidth * 0.5f;
        }

        private static void CropFrontSymbolFrame(int index, float texWidth, float leftKeepPixels, float rightKeepPixels, KAnim.Build.Symbol symbol)
        {
            var frame = symbol.GetFrame(index).symbolFrame;

            // Code by Romen

            // "bounding box" describes a pivot inside the UV rectangle, so we need to calculate the cropped UVs first
            // But the cropped UV depends on the size we want to keep around the original pivot, so we have to find where that is to determine the cropped UVs
            // We have to determine things in this order:
            // 1. Find the X position inside the texture of the original pivot
            // 2. Set new UVs around that X position
            // 3. Find an entirely new "bounding box" that is relative to the new UVs that puts the pivot in the same texture coordinates as before

            // 1 : Find the pivot X coordinate in the texture
            var kleiPivotX = (frame.bboxMin.x + frame.bboxMax.x) / 2f;
            var kleiPivotWidth = frame.bboxMax.x - frame.bboxMin.x;
            var pivotXPercent = 0.5f - kleiPivotX / kleiPivotWidth; // 0 is left, 0.5 is center, 1 is right
            var uvLeft = frame.uvMin.x;
            var uvRight = frame.uvMax.x;
            var uvWidth = uvRight - uvLeft;
            var texturePivotX = (uvLeft + pivotXPercent * uvWidth) * texWidth;

            // 2 : Set the new UVs around the pivot
            var newLeft = texturePivotX - leftKeepPixels;
            frame.uvMin.x = newLeft / texWidth;
            var newRight = texturePivotX + rightKeepPixels;
            frame.uvMax.x = newRight / texWidth;

            // 3 : Set "bounding box" for the new UVs
            var newWidth = leftKeepPixels + rightKeepPixels;
            var newKleiPivotWidth = newWidth * 2f;
            var newKleiPivotX = (0.5f - leftKeepPixels / newWidth) * newKleiPivotWidth;

            frame.bboxMin.x = newKleiPivotX - newKleiPivotWidth * 0.5f;
            frame.bboxMax.x = newKleiPivotX + newKleiPivotWidth * 0.5f;
        }

        private static KAnimFile CopyAnim(KAnimFile kAnimFile)
        {
            var file = Traverse.Create(kAnimFile);
            var animFile = file.Field<TextAsset>("animFile").Value;
            var buildFile = file.Field<TextAsset>("buildFile").Value;
            var kanim = ModUtil.AddKAnim("kigurumihood_" + kAnimFile.name, animFile, buildFile, kAnimFile.textureList);

            return kanim;
        }

        public static void CloneModded()
        {
            foreach (var kAnimFile in Assets.ModLoadedKAnims)
            {
                if (true) ////kAnimFile.IsAnimLoaded)// && kAnimFile.GetData().animCount == 0)
                {
                    Log.Debuglog("0 count anim found" + kAnimFile.name + " " + kAnimFile.GetData().build.symbols.Length);

                    foreach (var symbol in kAnimFile.GetData().build.symbols)
                    {
                        var id = HashCache.Get().Get(symbol.hash);

                        if (id.StartsWith("hat_hair_") || id.StartsWith("snapto_hat_hair"))
                        {
                            Log.Debuglog("found hait hair");
                            //CloneAndCache(kAnimFile);
                            var clone = CopyAnim(kAnimFile);
                            anims.Add(clone.name);
                            //Assets.ModLoadedKAnims.Add(clone);

                            break;
                        }
                    }
                }
            }

            Debug.Log($"checked {Assets.ModLoadedKAnims.Count} anims");
        }

        private static void CloneAndCache(KAnimFile kAnimFile)
        {
            var clone = CopyAnim(kAnimFile);
            CropHair(clone);
        }
    }
}
