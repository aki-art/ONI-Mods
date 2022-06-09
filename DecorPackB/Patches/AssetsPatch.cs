using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DecorPackB.Patches
{
    internal class AssetsPatch
    {

        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public class Assets_OnPrefabInit_Patch
        {
            public static void Postfix(Assets __instance)
            {
                if(Assets.GetSprite("hat_role_rancher1") is Sprite sprite)
                {
                    Log.Debuglog("SPRITE EXISTS");
                    Log.Debuglog(sprite.rect);
                }

                var path = Path.Combine(Utils.ModPath, "assets", "hat_role_rancher1_icon.png");
                var archeologyHatTexture = FUtility.Assets.LoadTexture(path);
                var archeologyHatSprite = Sprite.Create(archeologyHatTexture, new Rect(0, 0, archeologyHatTexture.width, archeologyHatTexture.height), Vector2.zero);

                Assets.Sprites.Add("hat_role_dpb_archeology", archeologyHatSprite);
            }
        }
    }
}
