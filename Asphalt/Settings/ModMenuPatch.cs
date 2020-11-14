using System;
using Harmony;
using FUtility.FUI;
using UnityEngine;

namespace Asphalt.Settings
{
    public class ModMenuPatch
    {
        [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods)
            {
                ModMenuButton.AddModSettingsButton(___displayedMods, ModAssets.ModPath, OpenModSettingsScreen);
            }

            private static void OpenModSettingsScreen() => Helper.OpenFDialog<SettingsScreen>(ModAssets.settingsDialogPrefab, "AsphaltSettings");
        }
    }
}
