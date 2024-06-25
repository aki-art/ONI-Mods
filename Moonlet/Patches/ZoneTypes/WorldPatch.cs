using HarmonyLib;

namespace Moonlet.Patches.ZoneTypes
{
	public class WorldPatch
	{
		[HarmonyPatch(typeof(World), nameof(World.OnPrefabInit))]
		public class World_OnPrefabInit_Patch
		{
			public static void Postfix(World __instance)
			{
				if (!__instance.TryGetComponent(out TerrainBG terrainBg))
				{
					Log.Warn("Terrrain bg null");
					return;
				}

				Log.Debug("WORLD ONPREFAB INIT");
				Mod.zoneTypesLoader.StitchBgTextures(terrainBg);
			}
		}
	}
}
