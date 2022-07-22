using HarmonyLib;
using UnityEngine;

namespace Backwalls.Patches
{
    public class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<BackwallSideScreen>("BackwallSideScreen", ModAssets.wallSidescreenPrefab);
                FUtility.FUI.SideScreen.AddCustomSideScreen<DyeSideScreen>("SwatchSideScreen", ModAssets.swatchSidescreenPrefab);
            }
        }
    }
}
