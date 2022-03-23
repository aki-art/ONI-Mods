using HarmonyLib;
using MoreMarbleSculptures.UI;

namespace MoreMarbleSculptures.Patches
{
    public class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<PaintableSidescreen>("Paintable Side Screen", ModAssets.Prefabs.colorPickerSidescreen);
            }
        }
    }
}
