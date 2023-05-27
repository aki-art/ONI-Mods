using Bestagons.Content.Scripts;
using HarmonyLib;

namespace Bestagons.Patches
{
    public class WorldGenPatch
    {
        [HarmonyPatch(typeof(ClusterManager), "OnWorldGenComplete")]
        public class ClusterManager_OnWorldGenComplete_Patch
        {
            public static void Postfix(ClusterManager __instance)
            {
            }
        }
    }
}
