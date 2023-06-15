using FUtility;
using FUtility.FUI;
using HarmonyLib;
using PrintingPodRecharge.UI;

namespace PrintingPodRecharge.Patches
{
    class ModsScreenPatch
    {
        [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods)
            {
                ModMenuButton.AddModSettingsButton(___displayedMods, "PrintingPodRecharge", OpenModSettingsScreen);
            }

            private static void OpenModSettingsScreen()
            {
                Helper.CreateFDialog<ModSettingsScreen>(ModAssets.Prefabs.settingsDialog, "BioInkSettings");
            }
        }
    }
}
