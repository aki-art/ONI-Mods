using HarmonyLib;
using System.Collections.Generic;

namespace Moonlet.Patches.ZoneTypes
{
	public class GroundMasksPath
	{
		// adds a new atlas to the groundmasks, just copies sandstone
		[HarmonyPatch(typeof(GroundMasks), nameof(GroundMasks.Initialize))]
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
						Mod.zoneTypesLoader.ApplyToActiveTemplates(zone =>
						{
							items.Add(new TextureAtlas.Item
							{
								indices = item.indices,
								name = item.name.Replace("sand_stone", zone.template.Id.ToLowerInvariant()),
								uvs = item.uvs,
								uvBox = item.uvBox,
								vertices = item.vertices
							});
						});
					}

					break;
				}

				atlas.items = [.. items];
			}
		}
	}
}
