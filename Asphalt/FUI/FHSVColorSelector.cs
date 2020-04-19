using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Asphalt
{
    class FHSVColorSelector : KMonoBehaviour
    {
        public event System.Action OnChange;

        public FSlider hueSlider;
        public FSlider satSlider;
        public FSlider valSlider;

        private FNumberInputField hueField;
        private FNumberInputField satField;
        private FNumberInputField valField;

        private const float HUE_MAX = byte.MaxValue;
        private const float SAT_MAX = 100;
        private const float VAL_MAX = 100;

        private Image satColorImage;
        private Image valImage;

        private Image previewImage;

        private float h;
        private float s;
        private float v;

        public Color color;
        private Color defaultColor;
        private Color loadedColor;

        private FButton resetButton;
        private FButton resetTempButton;
        private Image resetImg;
        private Image resetTempImg;


        static float MapHue(float x)
        {
            return x / HUE_MAX;
        }
        static float MapSat(float x)
        {
            return x / SAT_MAX;
        }
        static float MapVal(float x)
        {
            return x / VAL_MAX;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            #region setting object references
            hueSlider = transform.Find("HueSlider").gameObject.AddComponent<FSlider>();
            satSlider = transform.Find("SaturationSlider").gameObject.AddComponent<FSlider>();
            valSlider = transform.Find("ValueSlider").gameObject.AddComponent<FSlider>();

            hueField = hueSlider.transform.Find("InputField").gameObject.AddComponent<FNumberInputField>();
            satField = satSlider.transform.Find("InputField").gameObject.AddComponent<FNumberInputField>();
            valField = valSlider.transform.Find("InputField").gameObject.AddComponent<FNumberInputField>();

            satColorImage = satSlider.transform.Find("Background").GetComponent<Image>();
            valImage = valSlider.transform.Find("Background").GetComponent<Image>();
            previewImage = transform.Find("Image").GetComponent<Image>();

            resetImg = transform.Find("ResetOriginal").GetComponent<Image>();
            resetButton = resetImg.gameObject.AddComponent<FButton>();

            resetTempImg = transform.Find("ResetLast").GetComponent<Image>();
            resetTempButton = resetTempImg.gameObject.AddComponent<FButton>();
            #endregion

            resetButton.OnClick += ResetColor;
            resetTempButton.OnClick += ResetTempColor;

            hueSlider.OnChange += SliderOnChange;
            satSlider.OnChange += SliderOnChange;
            valSlider.OnChange += SliderOnChange;

            hueSlider.AttachInputField(hueField, MapHue);
            satSlider.AttachInputField(satField, MapVal);
            valSlider.AttachInputField(valField, MapSat);

            hueField.maxValue = (int)HUE_MAX;
            satField.maxValue = (int)SAT_MAX;
            valField.maxValue = (int)VAL_MAX;

            SetColorFromHex(SettingsManager.Settings.BitumenColor); // set default values

            defaultColor = Util.ColorFromHex(SettingsManager.DefaultSettings.BitumenColor);
            loadedColor = Util.ColorFromHex(SettingsManager.LoadedSettings.BitumenColor);

            resetImg.gameObject.SetActive(IsChanged(defaultColor));

            resetImg.color = defaultColor;
            resetTempImg.color = loadedColor;
        }

        private void SliderOnChange()
        {
            h = hueSlider.Value;
            s = satSlider.Value;
            v = valSlider.Value;

            color = Color.HSVToRGB(h, s, v);

            UpdateColorPreviews();
            UpdateInputFields();

            OnChange?.Invoke();
        }

        public void UpdateAll()
        {
            UpdateInputFields();
            UpdateSliderValues();
            UpdateColorPreviews();

        }

        public void SetColor(Color RGBColor)
        {
            Color.RGBToHSV(RGBColor, out h, out s, out v);
            color = RGBColor;

            UpdateAll();
        }
        public void SetColorFromHex(string hex)
        {
            color = Util.ColorFromHex(hex);
            Color.RGBToHSV(color, out h, out s, out v);

            UpdateAll();
        }
        public string GetHexValue()
        {
            return Util.ToHexString(color);
        }

        private void UpdateInputFields()
        {
            float m = 100f;
            hueField.SetDisplayValue(Mathf.Ceil(h * byte.MaxValue).ToString());
            satField.SetDisplayValue(Mathf.Ceil(s * m).ToString());
            valField.SetDisplayValue(Mathf.Ceil(v * m).ToString());
        }

        private void UpdateColorPreviews()
        {
            satColorImage.color = color;
            valImage.color = color;
            previewImage.color = color;

            resetImg.gameObject.SetActive(IsChanged(defaultColor));
            resetTempImg.gameObject.SetActive(IsChanged(loadedColor) && !CompareColors(loadedColor, defaultColor, 1));
        }

        private void UpdateSliderValues()
        {
            hueSlider.Value = h;
            satSlider.Value = s;
            valSlider.Value = v;
        }

        private void ResetTempColor()
        {
            SetColor(loadedColor);
        }

        private void ResetColor()
        {
            SetColor(defaultColor);
        }
        public bool IsChanged(Color defaultValue)
        {
            return !CompareColors(color, defaultValue, 1);
        }

        // color.Equals didnt seem reliable
        private bool CompareColors(Color c1, Color c2, double treshold)
        {
            var c1r = Math.Floor(c1.r * 1000);
            var c1g = Math.Floor(c1.g * 1000);
            var c1b = Math.Floor(c1.b * 1000);
            var c2r = Math.Floor(c2.r * 1000);
            var c2g = Math.Floor(c2.g * 1000);
            var c2b = Math.Floor(c2.b * 1000);

            bool rDiff = Math.Abs(c1r - c2r) < treshold;
            bool gDiff = Math.Abs(c1g - c2g) < treshold;
            bool bDiff = Math.Abs(c1b - c2b) < treshold;

            return (rDiff && gDiff && bDiff);
        }
    }
}
