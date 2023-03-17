using Backwalls.Settings;
using FUtility;
using FUtility.FUI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backwalls.UI
{
    public class ModSettingsDialog : FScreen
    {
        private FInputField2 colorInput;
        private FInputField2 patternInput;
        private FInputField2 decorativeDecorInput;
        private FInputField2 decorativeRangeInput;
        private FInputField2 sealedDecorInput;
        private FInputField2 sealedRangeInput;
        private FCycle renderCycle;

        private bool init;
        private bool patternValid;
        private bool colorValid;

        private void Init()
        {
            colorInput = transform.Find("Content/Color/Input").gameObject.AddOrGet<FInputField2>();
            patternInput = transform.Find("Content/Pattern/Input").gameObject.AddOrGet<FInputField2>();

            decorativeDecorInput = transform.Find("Content/DecorativeBackwall/Decor/Input").gameObject.AddOrGet<FInputField2>();
            decorativeRangeInput = transform.Find("Content/DecorativeBackwall/Range/Input").gameObject.AddOrGet<FInputField2>();
            sealedDecorInput = transform.Find("Content/SealedBackwall/Decor/Input").gameObject.AddOrGet<FInputField2>();
            sealedRangeInput = transform.Find("Content/SealedBackwall/Range/Input").gameObject.AddOrGet<FInputField2>();

            renderCycle = transform.Find("Content/RenderLayerPreset").gameObject.AddOrGet<FCycle>();
            renderCycle.Initialize(
                renderCycle.transform.Find("Left").gameObject.AddOrGet<FButton>(),
                renderCycle.transform.Find("Right").gameObject.AddOrGet<FButton>(),
                renderCycle.transform.Find("ChoiceLabel").gameObject.AddOrGet<LocText>());

            transform.Find("VersionLabel").GetComponent<LocText>().text = $"v{Log.GetVersion()}";

            init = true;
        }

        private void OnPatternChanged(string value)
        {
            patternValid = Mod.Settings.ValidatePattern(value);
            CheckValidity();
        }

        private void OnColorChanged(string value)
        {
            colorValid = Mod.Settings.ValidateColor(value);
            CheckValidity();
        }

        private void CheckValidity()
        {
            SetInputFieldColors(patternInput, patternValid);
            SetInputFieldColors(colorInput, colorValid);
            confirmButton.SetInteractable(patternValid && colorValid);
        }

        private void SetInputFieldColors(FInputField2 inputField, bool valid)
        {
            if(inputField?.inputField?.textComponent == null)
            {
                return;
            }

            inputField.inputField.textComponent.color = valid ? Color.black : Color.red;
        }

        public override void ShowDialog()
        {
            base.ShowDialog();

            if (!init)
            {
                Init();
            }

            colorInput.inputField.onValueChanged.AddListener(OnColorChanged);
            patternInput.inputField.onValueChanged.AddListener(OnPatternChanged);

            colorInput.Text = Mod.Settings.DefaultColor;
            patternInput.Text = Mod.Settings.DefaultPattern;

            decorativeDecorInput.Text = Mod.Settings.DecorativeWall.Decor.Amount.ToString();
            decorativeRangeInput.Text = Mod.Settings.DecorativeWall.Decor.Range.ToString();
            sealedDecorInput.Text = Mod.Settings.SealedWall.Decor.Amount.ToString();
            sealedRangeInput.Text = Mod.Settings.SealedWall.Decor.Range.ToString();

            decorativeDecorInput.inputField.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
            decorativeRangeInput.inputField.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
            sealedDecorInput.inputField.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;
            sealedRangeInput.inputField.contentType = TMPro.TMP_InputField.ContentType.IntegerNumber;

            renderCycle.Options = new List<FCycle.Option>()
            {
                new FCycle.Option(Config.WallLayer.Automatic.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.AUTOMATIC.TITLE, STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.AUTOMATIC.TITLE),
                new FCycle.Option(Config.WallLayer.BehindPipes.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.BEHINDPIPES.TITLE, STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.BEHINDPIPES.TITLE),
                new FCycle.Option(Config.WallLayer.HidePipes.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.HIDEPIPES.TITLE, STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.HIDEPIPES.TITLE)
            };

            renderCycle.Value = Mod.Settings.Layer.ToString();
        }

        public override void OnClickApply()
        {
            Mod.Settings.DefaultColor = colorInput.Text.ToUpperInvariant();
            Mod.Settings.DefaultPattern = patternInput.Text;
            Mod.Settings.Layer = Enum.TryParse<Config.WallLayer>(renderCycle.Value, out var result) ? result : Config.WallLayer.Automatic;

            Mod.SaveSettings();
            Deactivate();
        }
    }
}
