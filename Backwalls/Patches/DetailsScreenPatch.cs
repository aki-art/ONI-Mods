using HarmonyLib;

namespace Backwalls.Patches
{
    public class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<BackwallSidescreen>("BackwallSideScreen", ModAssets.wallSidescreenPrefab);
            }
        }
    }
}
