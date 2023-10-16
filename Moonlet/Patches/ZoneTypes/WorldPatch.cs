using HarmonyLib;

namespace Moonlet.Patches.ZoneTypes
{
	public class WorldPatch
	{
		[HarmonyPatch(typeof(World), "OnPrefabInit")]
		public class World_OnPrefabInit_Patch
		{
			public static void Postfix(World __instance)
			{
				if (!__instance.TryGetComponent(out TerrainBG terrainBg))
				{
					Log.Warn("Terrrain bg null");
					return;
				}

				Mod.zoneTypesLoader.StitchBgTextures(terrainBg);
			}
		}
	}
}
