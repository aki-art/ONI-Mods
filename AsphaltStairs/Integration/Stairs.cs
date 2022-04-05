using HarmonyLib;
using System;
using UnityEngine;

namespace AsphaltStairs.Integration
{
    public class Stairs
    {
        public static void TryPatch(Harmony harmony)
        {
            var stairsType = Type.GetType("Stairs.Patches+Navigator_BeginTransition_Patch, Stairs", false, false);
            if (stairsType == null)
            {
                ShowStairsMissingDialog();
                return;
            }

            var original = stairsType.GetMethod("Postfix");
            var postfix = typeof(Stairs).GetMethod("Postfix");
            harmony.Patch(original, new HarmonyMethod(postfix));
        }

        public static void Postfix(Navigator navigator, Navigator.ActiveTransition transition)
        {
            if (transition.navGridTransition.isCritter)
            {
                return;
            }

            var cell = Grid.CellBelow(Grid.PosToCell(navigator));
            var tile = Grid.Objects[cell, (int)ObjectLayer.LadderTile];

            if (tile != null)
            {
                if (tile.GetComponent<KPrefabID>().HasTag(AsphaltStairsConfig.ID))
                {
                    transition.speed *= Asphalt.speedModifier;
                    transition.animSpeed *= Asphalt.speedModifier;
                }
            }
        }

        private static void ShowStairsMissingDialog()
        {
            var parent = FUtility.FUI.Helper.GetACanvas("dialog_which_complains_about_no_stairs_mod");
            var UIIcon = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("puft_kanim"), "anti_ui");

            var screen = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject, true).GetComponent<ConfirmDialogScreen>();
            screen.PopupConfirmDialog(
                STRINGS.UI.NO_STAIRS_MOD.BUTTON,
                null,
                null,
                STRINGS.UI.NO_STAIRS_MOD.BUTTON,
                OpenStairsWorkshop,
                image_sprite: UIIcon);

            UnityEngine.Object.DontDestroyOnLoad(screen.gameObject);
        }

        private static void OpenStairsWorkshop()
        {
            Application.OpenURL("https://steamcommunity.com/sharedfiles/filedetails/?id=2012810337");
        }
    }
}
