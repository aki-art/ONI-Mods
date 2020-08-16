using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;
using FUtility;

namespace CenterOverlay
{
    public class HarmonyPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Log.PrintVersion();
                ModAssets.Initialize();
            }
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.settingsScreenPrefab = FUtility.Assets.LoadUIPrefab("centeroverlaysettings", "CenterOverlaySettingsDialog");

                Sprite sprite = Sprite.Create(
                    texture: ModAssets.symmetryOverlayTexture,
                    rect: new Rect(0, 0, 256, 202),
                    pivot: new Vector2(128, 128));

                Assets.Sprites.Add(new HashedString("overlay_symmetry"), sprite);
            }
        }

        [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
        public static class PauseScreen_OnPrefabInit_Patch
        {
            public static void Postfix(ref List<KButtonMenu.ButtonInfo> ___buttons)
            {
                var buttons = new List<KButtonMenu.ButtonInfo>(___buttons);
                buttons.Insert(
                    buttons.Count - 2, 
                    new KButtonMenu.ButtonInfo(
                        "Symmetry Overlay Settings",
                        Action.NumActions,
                        OpenSettingsDialog));
                ___buttons = buttons;
            }

            private static void OpenSettingsDialog()
            {
                if (ModAssets.settingsScreenPrefab == null)
                {
                    Log.Warning("Could not display UI: Mod Settings screen prefab is null.");
                    return;
                }

                Transform parent = FUtility.FUI.Helper.GetACanvas("SymmetryModSettings").transform;
                GameObject settingsScreen = UnityEngine.Object.Instantiate(ModAssets.settingsScreenPrefab.gameObject, parent);
                SettingsScreen settingsScreenComponent = settingsScreen.AddComponent<SettingsScreen>();
                settingsScreenComponent.ShowDialog();
            }
        }
    }
}
