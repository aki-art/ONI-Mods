using HarmonyLib;
using Moonlet.ZoneTypes;
using System;

namespace Moonlet.Patches
{
	[HarmonyPatch(typeof(SubworldZoneRenderData), "GenerateTexture")]
	public static class SubworldZoneRenderData_GenerateTexture_Patch
	{
		public static void Prefix(SubworldZoneRenderData __instance)
		{
			var textureIndex = ZoneTypeUtil.LAST_INDEX;

			Array.Resize(ref __instance.zoneColours, __instance.zoneColours.Length + ZoneTypeUtil.GetCount());
			Array.Resize(ref __instance.zoneTextureArrayIndices, __instance.zoneTextureArrayIndices.Length + ZoneTypeUtil.GetCount());

			foreach (var zone in ZoneTypeUtil.zones)
			{
				__instance.zoneColours[(int)zone.type] = zone.color32;
				__instance.zoneTextureArrayIndices[(int)zone.type] = textureIndex++;
			}
		}
	}
}
