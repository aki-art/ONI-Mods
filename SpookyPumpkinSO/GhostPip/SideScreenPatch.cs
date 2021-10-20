using HarmonyLib;
using SpookyPumpkinSO.GhostPip.Spawning;

namespace SpookyPumpkinSO.GhostPip
{
    class SideScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<GhostSquirrelSideScreen>("GhostSquirrelSideScreen", ModAssets.Prefabs.sideScreenPrefab);
                //FUtility.FUI.SideScreen.AddCustomSideScreen<GhostSquirrelSideScreen>("GhostSquirrelSideScreen", ModAssets.Prefabs.sideScreenPrefab);
                //PUIUtils.AddSideScreenContent<GhostPipSidescreen>();
                //Futility.SideScreen.AddClonedSideScreen<MysteryCallSidescreen>("Mysterious Call Screen", "ButtonMenuSideScreen", typeof(ButtonMenuSideScreen));
            }
        }
    }
}
