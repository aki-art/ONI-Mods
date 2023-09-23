using Backwalls.Settings;
using FUtility.FUI;
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
		private FCycle shinyCycle;

		private bool init;
		private bool patternValid;
		private bool colorValid;
		private bool hasDecorChanged;

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

			shinyCycle = transform.Find("Content/ShinyTileCycle").gameObject.AddOrGet<FCycle>();
			shinyCycle.Initialize(
				shinyCycle.transform.Find("Left").gameObject.AddOrGet<FButton>(),
				shinyCycle.transform.Find("Right").gameObject.AddOrGet<FButton>(),
				shinyCycle.transform.Find("ChoiceLabel").gameObject.AddOrGet<LocText>());

			transform.Find("VersionLabel").GetComponent<LocText>().text = $"v{Utils.AssemblyVersion}";

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
			if (inputField?.inputField?.textComponent == null)
				return;

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

			decorativeDecorInput.OnValueChanged.AddListener(OnDecorChanged);
			decorativeRangeInput.OnValueChanged.AddListener(OnDecorChanged);
			sealedDecorInput.OnValueChanged.AddListener(OnDecorChanged);
			sealedRangeInput.OnValueChanged.AddListener(OnDecorChanged);

			renderCycle.Options = new List<FCycle.Option>()
			{
				new FCycle.Option(Config.WallLayer.Automatic.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.AUTOMATIC.TITLE, STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.AUTOMATIC.TITLE),

				new FCycle.Option(Config.WallLayer.BehindPipes.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.BEHINDPIPES.TITLE, STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.BEHINDPIPES.TITLE),

				new FCycle.Option(Config.WallLayer.HidePipes.ToString(), STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.HIDEPIPES.TITLE, STRINGS.UI.SETTINGSDIALOG.CONTENT.RENDERLAYERPRESET.LAYER.HIDEPIPES.TITLE)
			};

			renderCycle.Value = Mod.Settings.Layer.ToString();

			shinyCycle.Options = new List<FCycle.Option>()
			{
				new FCycle.Option(
					 Config.ShinySetting.On.ToString(), 
					STRINGS.UI.SETTINGSDIALOG.CONTENT.SHINYTILECYCLE.OPTION.ON.TITLE,
					STRINGS.UI.SETTINGSDIALOG.CONTENT.SHINYTILECYCLE.OPTION.ON.TITLE),

				new FCycle.Option(
					 Config.ShinySetting.Dull.ToString(),
					STRINGS.UI.SETTINGSDIALOG.CONTENT.SHINYTILECYCLE.OPTION.DULL.TITLE,
					STRINGS.UI.SETTINGSDIALOG.CONTENT.SHINYTILECYCLE.OPTION.DULL.TITLE),

				new FCycle.Option(
					 Config.ShinySetting.Off.ToString(),
					STRINGS.UI.SETTINGSDIALOG.CONTENT.SHINYTILECYCLE.OPTION.OFF.TITLE,
					STRINGS.UI.SETTINGSDIALOG.CONTENT.SHINYTILECYCLE.OPTION.OFF.TITLE),
			};

			shinyCycle.Value = Mod.Settings.Shiny.ToString();
		}

		private void OnDecorChanged(string _)
		{
			hasDecorChanged = Mod.Settings.DecorativeWall.Decor.Amount.ToString() != decorativeDecorInput.Text
				|| Mod.Settings.DecorativeWall.Decor.Range.ToString() != decorativeRangeInput.Text
				|| Mod.Settings.SealedWall.Decor.Amount.ToString() != sealedDecorInput.Text
				|| Mod.Settings.SealedWall.Decor.Range.ToString() != sealedRangeInput.Text;
		}

		public override void OnClickApply()
		{
			if (hasDecorChanged)
			{
				var dlg = Util.KInstantiateUI<ConfirmDialogScreen>(
					ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject,
					FrontEndManager.Instance.gameObject,
					true);

				dlg.PopupConfirmDialog(
					global::STRINGS.UI.FRONTEND.MOD_DIALOGS.RESTART.MESSAGE.Replace("{0}\n", ""),
					() => Apply(true),
					() => { });

				return;
			}

			Apply(false);
		}

		private void Apply(bool restart)
		{
			Mod.Settings.DefaultColor = colorInput.Text.ToUpperInvariant();
			Mod.Settings.DefaultPattern = patternInput.Text;
			Mod.Settings.Layer = Enum.TryParse<Config.WallLayer>(renderCycle.Value, out var result) ? result : Config.WallLayer.Automatic;

			Mod.Settings.Shiny = Enum.TryParse<Config.ShinySetting>(shinyCycle.Value, out var shiny) ? shiny : Config.ShinySetting.On;

			Mod.Settings.DecorativeWall.Decor = new Config.DecorConfig(
				int.TryParse(decorativeRangeInput.Text, out int value2) ? value2 : 0,
				int.TryParse(decorativeDecorInput.Text, out int value1) ? value1 : 10);

			Mod.Settings.SealedWall.Decor = new Config.DecorConfig(
				int.TryParse(sealedRangeInput.Text, out int value3) ? value3 : 0,
				int.TryParse(sealedDecorInput.Text, out int value4) ? value4 : 10);

			Mod.SaveSettings();
			Deactivate();

			if (restart)
				App.instance.Restart();
		}
	}
}
