using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpookyPumpkinSO.Patches
{
    internal class AssetsPatch
    {

        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public class Assets_OnPrefabInit_Patch
        {
            public static void Prefix(Assets __instance)
            {
                var path = Path.Combine(Utils.ModPath, "assets");
                var texture = FUtility.FAssets.LoadTexture("spice_pumpkin", path);
                var reference = __instance.SpriteAssets.Find(s => s.name == "spice_recipe4");
                Log.Debuglog("spice ref: ", reference.rect, reference.pivot);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector3.zero);
                sprite.name = "spookypumpkin_spice_pumpkin";

                __instance.SpriteAssets.Add(sprite);
            }
        }
    }
}
