using HarmonyLib;

namespace BackgroundTiles.Patches
{
    public class BuildingConfigManagerPatch
    {
        [HarmonyPatch(typeof(BuildingConfigManager), "OnPrefabInit")]
        public static class BuildingConfigManager_OnPrefabInit_Patch
        {
            // Happens once per game launch, persistent between world loads
            public static void Prefix(BuildingConfigManager __instance)
            {
                __instance.gameObject.AddComponent<BackgroundTilesManager>();
            }
        }
    }
}
