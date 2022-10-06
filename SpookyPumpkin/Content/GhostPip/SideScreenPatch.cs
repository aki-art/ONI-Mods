using HarmonyLib;

namespace SpookyPumpkinSO.Content.GhostPip
{
    internal class SideScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<GhostSquirrelSideScreen>("GhostSquirrelSideScreen", ModAssets.Prefabs.sideScreenPrefab);
            }
        }
    }
}
