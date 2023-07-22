using FUtility;
using HarmonyLib;

namespace Moonlet.Patches.ZoneTypes
{
    public class TerrainBGPatch
    {
        [HarmonyPatch(typeof(World), "OnPrefabInit")]
        public class World_OnPrefabInit_Patch
        {
            public static void Postfix(World __instance)
            {
                if (!__instance.TryGetComponent(out TerrainBG terrainBg))
                {
                    Log.Warning("Terrrain bg null");
                    return;
                }

                Mod.sharedZoneTypeLoader.StitchBgTextures(terrainBg);
            }
        }
    }
}
