using Beached.Components;
using HarmonyLib;

namespace Beached.Patches
{
    internal class GlobalPatch
    {
#if DEBUG
        [HarmonyPatch(typeof(BuildingConfigManager), "OnPrefabInit")]
        public static class BuildingConfigManager_OnPrefabInit_Patch
        {
            public static void Postfix(BuildingConfigManager __instance)
            {
                __instance.gameObject.AddComponent<ZoneTypeColorPreviewer>();
            }
        }
    }
#endif
}
