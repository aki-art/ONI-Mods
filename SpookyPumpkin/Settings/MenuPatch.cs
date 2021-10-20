using HarmonyLib;
using FUtility.FUI;

namespace SpookyPumpkin.Settings
{
    class MenuPatch
    {
        //[HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods)
            {
                ModMenuButton.AddModSettingsButton(___displayedMods, ModAssets.ModPath, OpenModSettingsScreen);
            }

            private static void OpenModSettingsScreen()
            {
                Helper.OpenFDialog<SettingsScreen>(ModAssets.Prefabs.settingsDialogPrefab, "SpookyPumpkinSettings");
            }
        }
    }
}
