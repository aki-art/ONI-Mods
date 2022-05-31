using FUtility;
using FUtility.FUI;
using HarmonyLib;
using TrueTiles.Settings;

namespace TrueTiles.Patches
{
    public class ModsScreenPatch
    {
        [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods)
            {
                ModMenuButton.AddModSettingsButton(___displayedMods, Utils.ModPath, OpenModSettingsScreen);
            }

            private static void OpenModSettingsScreen()
            {
                Helper.CreateFDialog<SettingsScreen>(ModAssets.Prefabs.settingsDialog, "TrueTilesSettings");
            }
        }
    }
}
