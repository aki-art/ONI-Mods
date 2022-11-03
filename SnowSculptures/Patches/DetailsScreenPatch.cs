using HarmonyLib;
using SnowSculptures.Content.UI;

namespace SnowSculptures.Patches
{
    public class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<SnowMachineSideScreen>("Snoe machine Sidescreen", ModAssets.Prefabs.snowmachineSidescreenPrefab);
            }
        }
    }
}
