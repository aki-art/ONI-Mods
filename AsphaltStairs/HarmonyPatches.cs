using FUtility;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsphaltStairs
{
    partial class HarmonyPatches
    {
        private static float asphaltSpeedMultiplier = 2f;
        public static Harmony harmony;

        [HarmonyPatch(typeof(Game), "OnSpawn")]
        public static class Game_OnSpawn_Patch
        {
            public static void Postfix()
            {
                var settingsType = Type.GetType("Asphalt.SettingsManager, Asphalt", false, false);

                if (settingsType != null)
                {
                    var val = settingsType.GetProperty("Settings").GetValue(null);
                    var speed = Traverse.Create(val).Property("SpeedMultiplier").GetValue();

                    var MapValueMethod = Type.GetType("Asphalt.FSpeedSlider, Asphalt", false, false)
                        .GetMethod("MapValue", new Type[] { typeof(float) });

                    var mappedSpeed = MapValueMethod.Invoke(null, new object[] { speed });
                    asphaltSpeedMultiplier = (float)mappedSpeed;
                }
                else
                    Debug.Log($"[Asphalt Stairs] Asphalt Tiles mod not found. Leaving speed multiplier to a default {asphaltSpeedMultiplier}x.");
            }
        }

        private static GameObject FindACanvas()
        {
            GameObject parent;

            if (FrontEndManager.Instance != null)
            {
                parent = FrontEndManager.Instance.gameObject;
            }
            else
            {
                if (GameScreenManager.Instance != null && GameScreenManager.Instance.ssOverlayCanvas != null)
                {
                    parent = GameScreenManager.Instance.ssOverlayCanvas;
                }
                else
                {
                    parent = new GameObject();
                    parent.name = "AsphaltStairsErrorCanvas";
                    UnityEngine.Object.DontDestroyOnLoad(parent);
                    var canvas = parent.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                    canvas.sortingOrder = 10;
                    parent.AddComponent<GraphicRaycaster>();
                }
            }

            return parent;
        }

        private static void ShowStairsMissingDialog()
        {
            var parent = FindACanvas();
            string buttonText;
            string msgText;

            var locale = Localization.GetLocale();
            if (locale != null && locale.Code == "zh")
            {
                buttonText = "在Steam上打开Stairs";
                msgText = "柏油梯级依靠Stairs上班\n请订阅并启用Stairs。";
            }
            else
            {
                buttonText = "Open Stairs on Steam Workshop";
                msgText = "Asphalt Stairs relies on Stairs to work.\nPlease subscribe to and enable Stairs.";
            }

            var UIIcon = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("puft_kanim"), "anti_ui");

            var screen = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>();
            screen.PopupConfirmDialog(
                msgText,
                null,
                null,
                buttonText,
                OpenStairsWorkshop,
                image_sprite: UIIcon);

            UnityEngine.Object.DontDestroyOnLoad(screen.gameObject);
        }

        private static void OpenStairsWorkshop()
        {
            Application.OpenURL("https://steamcommunity.com/sharedfiles/filedetails/?id=2012810337");
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                var stairsType = Type.GetType("Stairs.Patches+Navigator_BeginTransition_Patch, Stairs", false, false);
                if (stairsType == null)
                {
                    ShowStairsMissingDialog();
                    return;
                }

                var AsphaltStairsID = AsphaltStairsConfig.ID.ToUpperInvariant();
                var locale = Localization.GetLocale();

                if (locale != null && locale.Code == "zh")
                {
                    // Translation by Soundslikeanadverb from Reddit
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AsphaltStairsID}.NAME",
                        "柏油梯级");
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AsphaltStairsID}.DESC",
                        "\"庄严公司不会因利用柏油梯级造成的受伤或损坏而负责任\"");
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AsphaltStairsID}.EFFECT",
                        $"可以用来斜方行走。高幅加快崩跑速度。\n\n <b><color=#F44A4A>需要砖块或支撑物在底下!</b></color>");
                }
                else
                {
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AsphaltStairsID}.NAME",
                        "Asphalt stairs");
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AsphaltStairsID}.DESC",
                        "\"Gravitas is not responsible for any injuries or damages caused when using the Asphalt Stairs.\"");
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AsphaltStairsID}.EFFECT",
                        $"Allows for diagonal movement. Majorly increases walking speed.\n\n <b><color=#F44A4A>Requires foundation tiles or support beneath!</b></color>");
                }

            }


            // manual patching, this way mod load order doesn't matter
            public static void Postfix()
            {
                var stairsType = Type.GetType("Stairs.Patches+Navigator_BeginTransition_Patch, Stairs", false, false);
                if (stairsType != null)
                {
                    var original = stairsType.GetMethod("Postfix");
                    var postfix = typeof(Patches.StairsPatch.Stairs_Patches_Navigator_BeginTransition_Patch_Postfix_Patch).GetMethod("Postfix");
                    harmony.Patch(original, new HarmonyMethod(postfix));
                }
                else
                {
                    Log.Warning("Stairs mod not found. Mod will not function properly.");
                }
            }
        }
    }
}