using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsphaltStairsAddon
{
    class HarmonyPatches
    {
        private static float asphaltSpeedMultiplier = 2f;
        private static HarmonyInstance harmony;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Debug.Log("[Asphalt Stairs] Loaded Asphalt Stairs version " + typeof(HarmonyPatches).Assembly.GetName().Version.ToString());
            }
        }

        public static class Mod_PostPatch
        {
            public static void PostPatch(HarmonyInstance harmonyInstance)
            {
                harmony = harmonyInstance;
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                Stairs.Patches.AddBuildingToPlanScreen("Base", AsphaltStairsConfig.ID, FirePoleConfig.ID);
            }
        }

        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch("OnSpawn")]
        public static class World_OnLoadLevel_Patch
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
                else Debug.Log($"[Asphalt Stairs] Asphalt Tiles mod not found. Leaving speed multiplier to a default {asphaltSpeedMultiplier}x.");
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
                    Canvas canvas = parent.AddComponent<Canvas>();
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
            GameObject parent = FindACanvas();
            string buttonText;
            string msgText;

            Localization.Locale locale = Localization.GetLocale();
            if(locale != null && locale.Code == "zh")
            {
                buttonText = "在Steam上打开Stairs";
                msgText = "柏油梯级依靠Stairs上班\n请订阅并启用Stairs。";
            }
            else
            {
                buttonText = "Open Stairs on Steam Workshop";
                msgText = "Asphalt Stairs relies on Stairs to work.\nPlease subscribe to and enable Stairs.";
            }

            Sprite UIIcon = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("puft_kanim"), "anti_ui");
            
            ConfirmDialogScreen screen = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>();
            screen.PopupConfirmDialog(
                text: msgText,
                on_confirm: null,
                on_cancel: null,
                configurable_text: buttonText,
                on_configurable_clicked: OpenStairsWorkshop,
                title_text: null,
                confirm_text: null,
                cancel_text: null,
                image_sprite: UIIcon,
                activateBlackBackground: true);
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
                Type stairsType = Type.GetType("Stairs.Patches+Navigator_BeginTransition_Patch, Stairs", false, false);
                if (stairsType == null) ShowStairsMissingDialog();

                string AsphaltStairsID = AsphaltStairsConfig.ID.ToUpperInvariant();
                Localization.Locale locale = Localization.GetLocale();

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

                List<string> list = new List<string>(Database.Techs.TECH_GROUPING["RefinedObjects"])
                {
                    AsphaltStairsConfig.ID
                };
                Database.Techs.TECH_GROUPING["RefinedObjects"] = list.ToArray();
            }


            // manual patching, this way mod load order doesn't matter
            public static void Postfix()
            {
                Type stairsType = Type.GetType("Stairs.Patches+Navigator_BeginTransition_Patch, Stairs", false, false);
                if (stairsType != null)
                {
                    var original = stairsType.GetMethod("Postfix");
                    var postfix = typeof(Stairs_Patches_Navigator_BeginTransition_Patch_Postfix_Patch).GetMethod("Postfix");
                    harmony.Patch(original, new HarmonyMethod(postfix));
                }
                else
                    Debug.LogError("[Asphalt Stairs] Stairs mod not found. Mod will not function properly.");
            }
        }

        public static class Stairs_Patches_Navigator_BeginTransition_Patch_Postfix_Patch
        {
            public static void Postfix(Navigator navigator, Navigator.ActiveTransition transition)
            {
                if (transition.navGridTransition.isCritter) return;

                int cell = Grid.CellBelow(Grid.PosToCell(navigator));
                var tile = Grid.Objects[cell, (int)ObjectLayer.LadderTile];

                if (tile != null)
                {
                    if (tile.GetComponent<KPrefabID>().HasTag(AsphaltStairsConfig.ID))
                    {
                        transition.speed *= asphaltSpeedMultiplier;
                        transition.animSpeed *= asphaltSpeedMultiplier;
                    }
                }
            }
        }
    }
}