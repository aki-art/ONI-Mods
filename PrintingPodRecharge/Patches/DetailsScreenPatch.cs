using HarmonyLib;
using PrintingPodRecharge.UI;

namespace PrintingPodRecharge.Patches
{
    public class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<BioInkSidescreen>("Telepad Recharge Sidescreen", ModAssets.Prefabs.bioInkSideScreen);
            }
        }
    }
}
