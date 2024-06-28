using HarmonyLib;
using Rendering;
using System;
using System.Reflection;
using TrueTiles.Cmps;
using UnityEngine;

namespace TrueTiles.Patches
{
	public class RenderInfoPatch
	{
		[HarmonyPatch]
		public static class RenderInfo_Ctor_Patch
		{
			// Private type so it needs to be targeted like this
			public static MethodBase TargetMethod()
			{
				var type = AccessTools.TypeByName("Rendering.BlockTileRenderer+RenderInfo");
				return AccessTools.Constructor(type, new Type[] { typeof(BlockTileRenderer), typeof(int), typeof(int), typeof(BuildingDef), typeof(SimHashes) });
			}

			public static void Postfix(BuildingDef def, SimHashes element, Material ___material, object ___decorRenderInfo)
			{
				var asset = TileAssets.Instance.Get(def.PrefabID, element);

				if (asset != null)
				{
					ShaderPropertyUtil.SetMainTexProperty(___material, asset.main);
					ShaderPropertyUtil.SetSpecularProperties(___material, asset.specular, asset.specularFrequency, asset.specularColor);

					if (asset.top != null || asset.topSpecular != null)
					{
						// private type also so needs to be traversed
						var topMaterial = Traverse.Create(___decorRenderInfo).Field<Material>("material").Value;

						ShaderPropertyUtil.SetMainTexProperty(topMaterial, asset.top);
						ShaderPropertyUtil.SetSpecularProperties(topMaterial, asset.topSpecular, asset.specularFrequency, asset.topSpecularColor);
					}

					if (asset.normalMap != null)
						___material.SetTexture("_NormalTex", asset.normalMap);
				}
			}
		}
	}
}
