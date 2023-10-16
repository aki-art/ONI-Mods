using HarmonyLib;
using System;
using static GroundMasks;

namespace Moonlet.Patches.ZoneTypes
{
	public class GroundRendererPatch
	{
		// sets the mask data. this is also responsible for selecting which border type will be rendered on ground tiles
		[HarmonyPatch(typeof(GroundRenderer), "OnPrefabInit")]
		public static class GroundRenderer_OnPrefabInit_Patch
		{
			public static void Postfix(GroundRenderer __instance, ref BiomeMaskData[] ___biomeMasks)
			{
				if (!Mod.zoneTypesLoader.IsActive() || ___biomeMasks == null)
					return;

				Array.Resize(ref ___biomeMasks, ___biomeMasks.Length + Mod.zoneTypesLoader.GetCount());

				Mod.zoneTypesLoader.AddBorders(__instance, ___biomeMasks);
			}
		}
	}
}
