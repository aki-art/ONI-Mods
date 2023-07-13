using FUtility;
using HarmonyLib;
using Moonlet.ZoneTypes;
using System.Linq;
using UnityEngine;

namespace Moonlet.Patches
{
	public class TerainBGPatch
	{
		// Adding backgrounds
		[HarmonyPatch(typeof(TerrainBG), "OnSpawn")]
		public static class TerrainBG_OnSpawn_Patch
		{
			public static void Postfix(TerrainBG __instance)
			{
				var zones = ZoneTypeUtil.zones.Where(z => z.texture != null).ToList();

				var srcArray = __instance.backgroundMaterial.GetTexture("images") as Texture2DArray;
				var extraDepth = zones.Count;
				var startDepth = srcArray.depth;
				var newDepth = srcArray.depth + extraDepth;

				Log.Debuglog($"zones count: {extraDepth}");
				Log.Debuglog($"startDepth: {startDepth}");

				Log.Debuglog(srcArray.format.ToString());

				// make new array
				var newArray = new Texture2DArray(srcArray.width, srcArray.height, newDepth, srcArray.format, false);

				// copy existing textures over
				for (var i = 0; i < srcArray.depth; i++)
					Graphics.CopyTexture(srcArray, i, 0, newArray, i, 0);

				// insert new textures
				for (var i = 0; i < extraDepth; i++)
				{
					var zoneTex = zones[i].texture;
					Graphics.CopyTexture(src: zoneTex, 0, 0, newArray, startDepth + i, 0);

					zones[i].texture = null;
				}

				newArray.Apply();

				__instance.backgroundMaterial.SetTexture("images", newArray);
			}
		}
	}
}
