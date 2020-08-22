using FUtility.FUI;
using Harmony;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CenterOverlay
{
    class SettingsScreen : FDialog
    {
        private FNumberInputField offsetField;
        private FButton plusOffsetButton;
        private FButton minusOffsetButton;
        private FButton flipButton;
        private FHSVColorSelector vanillaColorSelector;
        private FHSVColorSelector moddedColorSelector;

        public override void SetObjects()
        {
            base.SetObjects();

            // Unity would do this via Unity magic if the Editor was used
            offsetField = transform.Find("GeneralSettingsPanel/Offset/InputField").gameObject.AddComponent<FNumberInputField>();
            plusOffsetButton = transform.Find("GeneralSettingsPanel/Offset/InputField/PlusButton").gameObject.AddComponent<FButton>();
            minusOffsetButton = transform.Find("GeneralSettingsPanel/Offset/InputField/MinusButton").gameObject.AddComponent<FButton>();
            flipButton = transform.Find("GeneralSettingsPanel/FlipTogglePanel/OKButton").gameObject.AddComponent<FButton>();
            vanillaColorSelector = transform.Find("AdvancedSettingsPanel/VanillaColor").gameObject.AddComponent<FHSVColorSelector>();
            moddedColorSelector = transform.Find("AdvancedSettingsPanel/ModdedColor").gameObject.AddComponent<FHSVColorSelector>();

            offsetField.minValue = -1 * (Grid.WidthInCells / 2);
            offsetField.maxValue = Grid.WidthInCells / 2;
            offsetField.contentType = InputField.ContentType.IntegerNumber;

            vanillaColorSelector.Initialize(ModAssets.Settings.VanillaColorHex);
            moddedColorSelector.Initialize(ModAssets.Settings.ModdedColorHex);
        }

        public override void ShowDialog()
        {
            base.ShowDialog();

            offsetField.SetDisplayValue(ModAssets.Offset);
            offsetField.inputField.text = ModAssets.Offset.ToString();
            offsetField.OnEndEdit += () => ModAssets.Offset = offsetField.GetValue<int>();
            plusOffsetButton.OnClick += () => offsetField.SetDisplayValue(offsetField.GetFloat + 1, true);
            minusOffsetButton.OnClick += () => offsetField.SetDisplayValue(offsetField.GetFloat - 1, true);
            flipButton.OnClick += () => ModAssets.moddedOnLeft = !ModAssets.moddedOnLeft;

            vanillaColorSelector.OnChange += RefreshColors;
            moddedColorSelector.OnChange += RefreshColors;
        }

        private void RefreshColors()
        {
            ModAssets.SetColors(vanillaColorSelector.Hex, moddedColorSelector.Hex);
        }

        public override void Reset()
        {
            ModAssets.Offset = ModAssets.Settings.Offset;
            ModAssets.SetColors(ModAssets.Settings.VanillaColorHex, ModAssets.Settings.ModdedColorHex);
            ModAssets.moddedOnLeft = ModAssets.Settings.ModdedLeftSide;
        }

        public override void OnClickApply()
        {
            base.OnClickApply();
            ModAssets.Settings.VanillaColorHex = vanillaColorSelector.Hex;
            ModAssets.Settings.ModdedColorHex = moddedColorSelector.Hex;
            ModAssets.Settings.Offset = ModAssets.Offset;
            ModAssets.Settings.ModdedLeftSide = ModAssets.moddedOnLeft;

            ModAssets.SaveSettings();
            RefreshLegendPreviews();
            Deactivate();
        }

        private static void RefreshLegendPreviews()
        {
            if (OverlayScreen.Instance.GetMode() != SymmetryMode.ID) return;
            var activeUnitObjs = Traverse.Create(OverlayLegend.Instance).Field("activeUnitObjs").GetValue<List<GameObject>>();
            foreach (GameObject unit in activeUnitObjs)
            {
                LocText description = unit.GetComponentInChildren<LocText>();
                Image image = unit.transform.Find("Icon").GetComponent<Image>();
                image.color = description.text == "Modded" ? ModAssets.moddedColor : ModAssets.vanillaColor;
            }
        }
    }
}
