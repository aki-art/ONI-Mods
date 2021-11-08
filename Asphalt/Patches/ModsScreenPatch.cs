using HarmonyLib;
using FUtility.FUI;
using Asphalt.Settings;

namespace Asphalt.Patches
{
    class ModsScreenPatch
    {
        [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods)
            {
                ModMenuButton.AddModSettingsButton(___displayedMods, Mod.modPath, OpenModSettingsScreen);
            }

            private static void OpenModSettingsScreen() => Helper.OpenFDialog<SettingsScren>(ModAssets.Prefabs.settingsDialog, "AsphaltSettings");
        }
    }
}
