using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using DecorPackA.Buildings.StainedGlassTile;
using DecorPackA.Settings;
using FUtility.FUI;
using TMPro;
using UnityEngine;
using static DecorPackA.STRINGS.UI.DECORPACKA_SETTINGS;

namespace DecorPackA.UI
{
	public class SettingsScreen : FScreen
	{
		private bool initialized;
		private UITabs tabs;

		public FInputField2 glassSculptures_decor;
		public FInputField2 glassSculptures_decorRange;

		public FToggle2 mood_vibrantColors;
		public FToggle2 mood_enableLightRays;
		public FInputField2 mood_decor;
		public FInputField2 mood_decorRange;
		public FInputField2 mood_power;
		public FInputField2 mood_selfHeat;
		public FInputField2 mood_exhaust;
		public FInputField2 mood_lux;
		public FInputField2 mood_luxRange;

		public FToggle2 tiles_useDyeTC;
		public FToggle2 tiles_nerfAbyssalite;
		public FToggle2 tiles_disableShift;
		public FToggle2 tiles_insulate;
		public FInputField2 tiles_dyeRatio;
		public FInputField2 tiles_decor;
		public FInputField2 tiles_decorRange;
		public FInputField2 tiles_speed;

		public override void SetObjects()
		{
			base.SetObjects();

			tabs = transform.Find("Tabs").gameObject.AddComponent<UITabs>();

			var contentPrefab = transform.Find("Content").gameObject.AddComponent<ContentPanel>();

			InitGlassSculptures(contentPrefab);
			InitMoodLamps(contentPrefab);
			InitTiles(contentPrefab);

			SetValuesFromConfig();

			transform.Find("VersionRow/VersionLabel").GetComponent<LocText>().SetText($"v{Utils.AssemblyVersion}");

			// roughly center it vertically based on my largest screen size
			var maxHeight = 300f;
			transform.localPosition = new Vector3F(0f, -Screen.height / 2f + maxHeight * 2f, transform.localPosition.z);
		}

		private void InitTiles(ContentPanel contentPrefab)
		{
			var content = Instantiate(contentPrefab, transform);
			content.transform.SetSiblingIndex(1);

			content.AddTitle(STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE.NAME);
			content.AddInputField(SPEED, 1.25f, out tiles_speed, SPEED_TOOLTIP);
			content.AddDoubleInputField(DECOR, 10, 2, out tiles_decor, out tiles_decorRange);
			content.AddToggle(USE_DYE_TC, true, out tiles_useDyeTC, USE_DYE_TC_TOOLTIP);
			content.AddInputField(DYE_RATIO, 0.5f, out tiles_dyeRatio, DYE_RATIO_TOOLTIP);
			tiles_dyeRatio.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
			content.AddToggle(NERF_ABYSSALITE, true, out tiles_nerfAbyssalite, NERF_ABYSSALITE_TOOLTIP);
			content.AddToggle(DISABLE_COLORSHIFT, false, out tiles_disableShift, DISABLE_COLORSHIFT_TOOLTIP);
			content.AddToggle(INSULATE_STORAGE, true, out tiles_insulate, INSULATE_STORAGE_TOOLTIP);

			// todo speed


			content.gameObject.SetActive(true);
			tabs.AddTab(STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE.NAME, StainedGlassTiles.GetID(SimHashes.Lead.ToString()), content.gameObject);
		}

		private void InitMoodLamps(ContentPanel contentPrefab)
		{
			var content = Instantiate(contentPrefab, transform);
			content.transform.SetSiblingIndex(1);

			content.AddTitle(STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP.NAME);
			content.AddToggle(VIBRANT, true, out mood_vibrantColors, VIBRANT_TOOLTIP);
			content.AddToggle(ENABLE_LIGHTRAYS, true, out mood_enableLightRays, ENABLE_LIGHTRAYS_TOOLTIP);
			content.AddDoubleInputField(DECOR, 25, 8, out mood_decor, out mood_decorRange);
			content.AddDoubleInputField(LUX, 400, 3, out mood_lux, out mood_luxRange);
			content.AddUnitInputField(ENERGY, "W", 6, out mood_power, ENERGY_TOOLTIP);
			mood_power.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
			content.AddUnitInputField(SELFHEAT_KW, "kW", 0, out mood_selfHeat, SELFHEAT_KW_TOOLTIP);
			mood_selfHeat.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
			content.AddUnitInputField(EXHAUSTKILOWATTSWHENACTIVE, "kW", 0.5f, out mood_exhaust, EXHAUSTKILOWATTSWHENACTIVE_TOOLTIP);
			mood_exhaust.inputField.contentType = TMP_InputField.ContentType.DecimalNumber;

			content.gameObject.SetActive(true);
			tabs.AddTab(STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP.NAME, MoodLampConfig.ID, content.gameObject);
		}

		private void InitGlassSculptures(ContentPanel contentPrefab)
		{
			var content = Instantiate(contentPrefab, transform);
			content.transform.SetSiblingIndex(1);

			content.AddTitle(STRINGS.BUILDINGS.PREFABS.DECORPACKA_GLASSSCULPTURE.NAME);
			content.AddDoubleInputField(DECOR, 40, 8, out glassSculptures_decor, out glassSculptures_decorRange);

			content.gameObject.SetActive(true);

			tabs.AddTab(STRINGS.BUILDINGS.PREFABS.DECORPACKA_GLASSSCULPTURE.NAME, GlassSculptureConfig.ID, content.gameObject);
		}

		public override void OnClickApply()
		{
			SaveConfig();

			Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GetComponentInParent<Canvas>().gameObject, true)
				.AddDefaultCancel()
				.SetHeader(global::STRINGS.UI.FRONTEND.MOD_EVENTS.REQUIRES_RESTART)
				.AddPlainText(STRINGS.UI.DECORPACKA_SETTINGSDIALOGB_TMPCONVERTED.REQUIRE_RESTART)
				.AddOption(global::STRINGS.UI.CONFIRMDIALOG.OK, _ => App.instance.Restart());
		}

		public override void Reset()
		{
			SetValuesFromConfig();
			base.Reset();
		}

		private void SetValuesFromConfig()
		{
			glassSculptures_decor.Text = Mod.Settings.GlassSculpture.BaseDecor.Amount.ToString();
			glassSculptures_decorRange.Text = Mod.Settings.GlassSculpture.BaseDecor.Range.ToString();

			mood_vibrantColors.On = Mod.Settings.MoodLamp.VibrantColors;
			mood_decor.Text = Mod.Settings.MoodLamp.Decor.Amount.ToString();
			mood_decorRange.Text = Mod.Settings.MoodLamp.Decor.Range.ToString();
			mood_power.Text = Mod.Settings.MoodLamp.PowerUse.EnergyConsumptionWhenActive.ToString();
			mood_selfHeat.Text = Mod.Settings.MoodLamp.PowerUse.SelfHeatKilowattsWhenActive.ToString();
			mood_exhaust.Text = Mod.Settings.MoodLamp.PowerUse.ExhaustKilowattsWhenActive.ToString();
			mood_lux.Text = Mod.Settings.MoodLamp.Lux.Amount.ToString();
			mood_luxRange.Text = Mod.Settings.MoodLamp.Lux.Range.ToString();
			mood_enableLightRays.On = !Mod.Settings.MoodLamp.DisableLightRays;

			tiles_useDyeTC.On = Mod.Settings.GlassTile.UseDyeTC;
			tiles_nerfAbyssalite.On = Mod.Settings.GlassTile.NerfAbyssalite;
			tiles_disableShift.On = Mod.Settings.GlassTile.DisableColorShiftEffect;
			tiles_insulate.On = Mod.Settings.GlassTile.InsulateConstructionStorage;
			tiles_dyeRatio.Text = Mod.Settings.GlassTile.DyeRatio.ToString();
			tiles_decor.Text = Mod.Settings.GlassTile.Decor.Amount.ToString();
			tiles_decorRange.Text = Mod.Settings.GlassTile.Decor.Range.ToString();
			tiles_speed.Text = Mod.Settings.GlassTile.SpeedBonus.ToString();
		}

		private int GetIntOrDefault(string str, int defaultValue) => int.TryParse(str, out var val) ? val : defaultValue;

		private float GetFloatOrDefault(string str, float defaultValue) => float.TryParse(str, out var val) ? val : defaultValue;

		public void SaveConfig()
		{
			var defaults = new Config();

			Mod.Settings.GlassSculpture.BaseDecor.Amount = GetIntOrDefault(glassSculptures_decor.Text, defaults.GlassSculpture.BaseDecor.Amount);
			Mod.Settings.GlassSculpture.BaseDecor.Range = GetIntOrDefault(glassSculptures_decorRange.Text, defaults.GlassSculpture.BaseDecor.Range);

			Mod.Settings.MoodLamp.VibrantColors = mood_vibrantColors.On;
			Mod.Settings.MoodLamp.Decor.Amount = GetIntOrDefault(mood_decor.Text, defaults.MoodLamp.Decor.Amount);
			Mod.Settings.MoodLamp.Decor.Range = GetIntOrDefault(mood_decorRange.Text, defaults.MoodLamp.Decor.Range);
			Mod.Settings.MoodLamp.PowerUse.EnergyConsumptionWhenActive = GetFloatOrDefault(mood_power.Text, defaults.MoodLamp.PowerUse.EnergyConsumptionWhenActive);
			Mod.Settings.MoodLamp.PowerUse.SelfHeatKilowattsWhenActive = GetFloatOrDefault(mood_selfHeat.Text, defaults.MoodLamp.PowerUse.SelfHeatKilowattsWhenActive);
			Mod.Settings.MoodLamp.PowerUse.ExhaustKilowattsWhenActive = GetFloatOrDefault(mood_exhaust.Text, defaults.MoodLamp.PowerUse.ExhaustKilowattsWhenActive);
			Mod.Settings.MoodLamp.Lux.Amount = GetIntOrDefault(mood_lux.Text, defaults.MoodLamp.Lux.Amount);
			Mod.Settings.MoodLamp.Lux.Range = GetIntOrDefault(mood_luxRange.Text, defaults.MoodLamp.Lux.Range);
			Mod.Settings.MoodLamp.DisableLightRays = !mood_enableLightRays;

			Mod.Settings.GlassTile.UseDyeTC = tiles_useDyeTC.On;
			Mod.Settings.GlassTile.NerfAbyssalite = tiles_nerfAbyssalite.On;
			Mod.Settings.GlassTile.DisableColorShiftEffect = tiles_disableShift.On;
			Mod.Settings.GlassTile.InsulateConstructionStorage = tiles_insulate.On;
			Mod.Settings.GlassTile.DyeRatio = GetFloatOrDefault(tiles_dyeRatio.Text, defaults.GlassTile.DyeRatio);
			Mod.Settings.GlassTile.Decor.Amount = GetIntOrDefault(tiles_decor.Text, defaults.GlassTile.Decor.Amount);
			Mod.Settings.GlassTile.Decor.Range = GetIntOrDefault(tiles_decorRange.Text, defaults.GlassTile.Decor.Range);
			Mod.Settings.GlassTile.SpeedBonus = GetFloatOrDefault(tiles_speed.Text, defaults.GlassTile.SpeedBonus);

			Mod.SaveSettings();

			// update incorrect inputs
			SetValuesFromConfig();
		}

		protected override void OnShow(bool show)
		{
			base.OnShow(show);
			tabs.UpdateTabs();
		}
	}
}
