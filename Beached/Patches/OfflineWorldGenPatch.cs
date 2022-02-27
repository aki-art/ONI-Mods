using FUtility;
using HarmonyLib;
using ProcGen;
using System;
using static ProcGen.SubWorld;

namespace Beached.Patches
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

            }
        }
    }
}
