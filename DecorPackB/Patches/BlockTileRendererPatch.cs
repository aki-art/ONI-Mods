using DecorPackB.Content.Scripts;
using HarmonyLib;
using Rendering;
using UnityEngine;

namespace DecorPackB.Patches
{
	public class BlockTileRendererPatch
	{
		[HarmonyPatch(typeof(BlockTileRenderer), nameof(BlockTileRenderer.GetCellColour))]
		private static class BlockTileRendererPatches
		{
			private static void Postfix(ref Color __result, int cell)
			{
				if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var tile)
					&& tile.TryGetComponent(out FloorLamp floorLamp))
				{
					__result *= floorLamp.overrideColor;
				}
			}
		}
	}
}
