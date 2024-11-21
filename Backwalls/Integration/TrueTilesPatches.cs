using Backwalls.Buildings;
using HarmonyLib;
using System;
using UnityEngine;

namespace Backwalls.Integration
{
	// add true tiles variants in
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
				var traverse = Traverse.Create(asset);
				var mainTex = traverse.Field<Texture2D>("main").Value;
				var specularTex = traverse.Field<Texture2D>("specular").Value;
				var buildingDef = Assets.GetBuildingDef(def);

				if (def == null || mainTex == null)
				{
					return;
				}

				var item = new BackwallPattern(def + material, buildingDef.Name, mainTex, null, 1);

				if (specularTex != null)
				{
					item.specularTexture = specularTex;
					item.specularColor = traverse.Field<Color>("specularColor").Value;
				}

				Log.Debug("setting true tiles icon");
				item.UISprite = SpriteHelper.GetSpriteForAtlas(item.atlas);

				Mod.variants[def + material] = item;
			}
		}
	}
}
