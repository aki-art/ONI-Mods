using FUtility;
using Harmony;
using KMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static FUtility.FUI.Helper;

namespace SpookyPumpkin.Settings
{
    class MenuPatch
    {
        [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
        public static class ModsScreen_BuildDisplay_Patch
        {
            public static void Postfix(object ___displayedMods)
            {
                foreach (var modEntry in (IEnumerable)___displayedMods)
                {
                    int index = Traverse.Create(modEntry).Field("mod_index").GetValue<int>();
                    Mod mod = Global.Instance.modManager.mods[index];

                    // checks if the current mod entry is this mod
                    if (index >= 0 && mod.file_source.GetRoot() == ModAssets.ModPath)
                    {
                        Transform transform = Traverse.Create(modEntry).Field("rect_transform").GetValue<RectTransform>();
                        if (transform != null)
                        {
                            Transform manageButton = transform.Find("ManageButton");
                            if(manageButton == null)
                            {
                                Log.Warning("Could not create button in Mods Menu. Is another mod interfering?");
                                return;
                            }

                            // copy the subscription button
                            KButton configButton = MakeKButton(
                                new ButtonInfo("Settings", OpenModSettingsScreen, 14),
                                manageButton.gameObject,
                                manageButton.parent.gameObject,
                                manageButton.GetSiblingIndex() - 1);
                        }
                    }
                }
            }

            private static void OpenModSettingsScreen()
            {
                OpenFDialog<SettingsScreen>(ModAssets.Prefabs.settingsDialogPrefab, "SpookyPumpkinSettings");
            }
        }
    }
}
