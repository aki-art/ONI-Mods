using FUtility;
using HarmonyLib;
using ProcGen;

namespace Beached.Patches.ZoneTypes
{
    internal class OfflineWorldGenPatch
    {
        [HarmonyPatch(typeof(OfflineWorldGen), "Generate")]
        public static class OfflineWorldGen_Generate_Patch
        {
            public static void Prefix()
            {
                Log.Debuglog("OFFLINEWORLGEN" + (int)ModAssets.ZoneTypes.beach);

                var zoneType = typeof(SubWorld).GetProperty("zoneType");
                zoneType.SetValue(SettingsCache.subworlds["subworlds/oceanaria/OceanariaStart"], ModAssets.ZoneTypes.beach, null);
                zoneType.SetValue(SettingsCache.subworlds["subworlds/depths/DepthsCore"], ModAssets.ZoneTypes.depths, null);
                zoneType.SetValue(SettingsCache.subworlds["subworlds/bambooforest/BambooForestMain"], ModAssets.ZoneTypes.bamboo, null);

            }
        }
    }
}
