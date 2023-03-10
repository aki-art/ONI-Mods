using HarmonyLib;
using Randomizer.Content.Scripts;

namespace Randomizer.Patches
{
    public class ClusterCategorySelectionScreenPatch
    {
        [HarmonyPatch(typeof(ClusterCategorySelectionScreen), "OnSpawn")]
        public class ClusterCategorySelectionScreen_OnSpawn_Patch
        {
            public static void Postfix(ClusterCategorySelectionScreen __instance)
            {
                __instance.gameObject.AddOrGet<RandomizerClusterCategorySelectionScreen>();
            }
        }
    }
}
