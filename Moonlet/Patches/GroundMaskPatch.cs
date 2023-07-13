using HarmonyLib;
using Moonlet.ZoneTypes;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class GroundMaskPatch
	{
		// adds a new atlas to the groundmasks, just copies sandstone
		[HarmonyPatch(typeof(GroundMasks), "Initialize")]
		public static class GroundMasks_Initialize_Patch
		{
			public static void Prefix(GroundMasks __instance)
			{
				var atlas = __instance.maskAtlas;

				if (atlas is null)
					return;

				var items = new List<TextureAtlas.Item>(atlas.items);

				foreach (var item in atlas.items)
				{
					if (item.name.Contains("sand_stone"))
					{
						foreach (var zone in ZoneTypeUtil.zones)
						{
							items.Add(new TextureAtlas.Item
							{
								indices = item.indices,
								name = item.name.Replace("sand_stone", zone.id.ToLowerInvariant()),
								uvs = item.uvs,
								uvBox = item.uvBox,
								vertices = item.vertices
							});
						}
					}
				}

				atlas.items = items.ToArray();
			}
		}
	}
}
