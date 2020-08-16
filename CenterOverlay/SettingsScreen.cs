using FUtility.FUI;
using System;
using UnityEngine.UI;

namespace CenterOverlay
{
    class SettingsScreen : FDialog
    {
        private FNumberInputField offsetField;
        private FButton plusOffsetButton;
        private FButton minusOffsetButton;
        public override void SetObjects()
        {
            base.SetObjects();
            offsetField = transform.Find("GeneralSettingsPanel/Offset/InputField").gameObject.AddComponent<FNumberInputField>();
            plusOffsetButton = transform.Find("GeneralSettingsPanel/Offset/InputField/PlusButton").gameObject.AddComponent<FButton>();
            minusOffsetButton = transform.Find("GeneralSettingsPanel/Offset/InputField/MinusButton").gameObject.AddComponent<FButton>();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
            offsetField.OnEndEdit += OnOffsetChanged;
            offsetField.minValue = -1 * (Grid.WidthInCells / 2);
            offsetField.maxValue = Grid.WidthInCells / 2;
            offsetField.contentType = InputField.ContentType.IntegerNumber;

            plusOffsetButton.OnClick += () => offsetField.SetDisplayValue(offsetField.GetFloat + 1, true);
            minusOffsetButton.OnClick += () => offsetField.SetDisplayValue(offsetField.GetFloat - 1, true);

        }
        private void OnOffsetChanged()
        {
            ModAssets.Settings.Offset = offsetField.GetValue<int>();
        }

        public override void OnClickApply()
        {
            base.OnClickApply();
            ModAssets.Settings.Offset = offsetField.GetValue<int>();
        }
    }
}
