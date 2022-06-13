using FUtility;
using HarmonyLib;
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
                    // call PoopSlag(owner, originalPoopMass)
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
            }
        }

        public void Generate2()
        {
            var hoodieHairAnim = Assets.GetAnim("kigurumihood_hair_swap_kanim");
            //var hoodieHairAnim = CopyAnim("hair_swap_kanim");

            float texWidth = hoodieHairAnim.GetData().build.GetTexture(0).width;

            // Define the amount of pixels to keep on each side of the pivot
            var leftKeepPixels = 37f;
            var rightKeepPixels = 33f;

            foreach (var symbol in hoodieHairAnim.GetData().build.symbols)
            {
                var id = symbol.hash.ToString();

                if (id.StartsWith("hat_h"))
                {
                    /*
                    //var tex = symbol.build.GetTexture(0);
                    var data = KAnimBatchManager.Instance().GetBatchGroupData(symbol.build.batchTag);

                    //tex = data.GetTexure(symbol.build.textureStartIdx);
                    var tex = new Texture2D(100, 100);
                    tex.SetPixels(Enumerable.Repeat(Color.red, tex.GetPixels().Length).ToArray());
                    tex.Apply();

                    data.textures[0] = tex;
                    */
                    var frame = symbol.GetFrame(0).symbolFrame;


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
            }
        }

        private static KAnimFile CopyAnim(KAnimFile kAnimFile)
        {
            var file = Traverse.Create(kAnimFile);
            var animFile = file.Field<TextAsset>("animFile").Value;
            var buildFile = file.Field<TextAsset>("buildFile").Value;
            var kanim = ModUtil.AddKAnim("kigurumihood_" + kAnimFile.name, animFile, buildFile, kAnimFile.textureList);

            return kanim;
        }

        public void Generate()
        {
            foreach (var kAnimFile in Assets.ModLoadedKAnims)
            {
                if (kAnimFile.IsAnimLoaded && kAnimFile.GetData().animCount == 0)
                {
                    //animNames.Add(name);
                    Log.Debuglog("0 count anim found" + kAnimFile.name + " " + kAnimFile.GetData().build.symbols.Length);

                    foreach (var symbol in kAnimFile.GetData().build.symbols)
                    {
                        var id = HashCache.Get().Get(symbol.hash);
                        Log.Debuglog(id);

                        if (id.StartsWith("hat_hair_") || id.StartsWith("snapto_hat_hair"))
                        {
                            Log.Debuglog("found hait hair");
                        }

                        //if(symbol.)
                    }
                }
            }

            Debug.Log($"checked {Assets.ModLoadedKAnims.Count} anims");
        }
    }
}
