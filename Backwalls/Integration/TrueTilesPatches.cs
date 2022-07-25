using Backwalls.Buildings;
using HarmonyLib;
using System;
using UnityEngine;

namespace Backwalls.Integration
{
    public class TrueTilesPatches
    {
        public static void Patch(Harmony harmony)
        {
            var t_TileAssets = Type.GetType("TrueTiles.Cmps.TileAssets, TrueTiles", false, false);
            if (t_TileAssets != null)
            {
                var m_Add = t_TileAssets.GetMethod("Add");
                var postfix = typeof(TrueTilesPatches).GetMethod("Postfix");
                harmony.Patch(m_Add, null, new HarmonyMethod(postfix));
            }
        }

        public static void Postfix(string def, SimHashes material, object asset)
        {
            if (asset != null)
            {
                var mainTex = Traverse.Create(asset).Field<Texture2D>("main").Value;
                var buildingDef = Assets.GetBuildingDef(def);

                if (def == null || mainTex == null)
                {
                    return;
                }

                var item = new BackwallPattern(def + material, buildingDef.Name, mainTex, null, 1);
                item.UISprite = SpriteHelper.GetSpriteForDef(item.atlas);

                Mod.variants.Add(item);
            }
        }
    }
}
