using System;
using UnityEngine;
using UnityEngine.UI;

namespace FUtility.FUI
{
    public class FHSVColorSelector : KMonoBehaviour
    {
        public string Hex => Util.ToHexString(color);
        public Color color;
        private Color defaultColor;

        public event System.Action OnChange;

        private DimensionSlider hue;
        private DimensionSlider sat;
        private DimensionSlider val;

        private float h;
        private float s;
        private float v;

        private Image previewImage;
        private FButton resetButton;
        private Image resetImg;

        public void Initialize(string hex) => Initialize(Util.ColorFromHex(hex));
        public void Initialize(Color color)
        {
            defaultColor = color;
            SetColor(color);
            UpdateColorPreviews();
        }

        protected override void OnPrefabInit()
        {
            SetObjects();

            hue = new DimensionSlider(gameObject, "HueSlider", 255, SliderOnChange);
            sat = new DimensionSlider(gameObject, "SaturationSlider", 100, SliderOnChange, true);
            val = new DimensionSlider(gameObject, "ValueSlider", 100, SliderOnChange);

            resetButton.OnClick += ResetColor;
        }

        private void SetObjects()
        {
            previewImage = transform.Find("Image").GetComponent<Image>();
            resetImg = transform.Find("ResetOriginal").GetComponent<Image>();
            resetButton = resetImg.gameObject.AddComponent<FButton>();
        }

        public void SetColor(Color RGB)
        {
            Color.RGBToHSV(RGB, out h, out s, out v);
            color = RGB;
            UpdateAll(); 
        }

        public void SetColorFromHex(string hex) => SetColor(Util.ColorFromHex(hex));

        private void SliderOnChange()
        {
            h = hue.Value;
            s = sat.Value;
            v = val.Value;

            color = Color.HSVToRGB(h, s, v);

            UpdateColorPreviews();
            UpdateColorRanges();

            OnChange?.Invoke();
        }

        public void UpdateAll()
        {
            UpdateColorRanges();
            UpdateSliderValues();
            UpdateColorPreviews();
        }

        private void UpdateColorRanges()
        {
            hue.RefreshColors(h, Color.white);
            sat.RefreshColors(s, Color.HSVToRGB(h, 1, v), Color.HSVToRGB(0, 0, v));
            val.RefreshColors(v, Color.HSVToRGB(h, s, 1));
        }

        private void UpdateSliderValues()
        {
            hue.Value = h;
            sat.Value = s;
            val.Value = v;
        }

        private void UpdateColorPreviews()
        {
            previewImage.color = color;

            bool NotDefault = !IsSame(color, defaultColor);

            resetImg.gameObject.SetActive(NotDefault);
            if(NotDefault)
                resetImg.color = defaultColor;
        }
        private void ResetColor() => SetColor(defaultColor);

        // Unity-s Color.Equals() kept failing because of conversions and roundings
        private bool IsSame(Color c1, Color c2, double treshold = 1)
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
        public class DimensionSlider
        {
            public FSlider slider;
            private FNumberInputField field;
            private readonly Image overlayImage;
            private readonly Image fade;
            private readonly bool updateFade;
            private readonly float max;

            public float Value
            {
                get => slider.Value;
                set => slider.Value = value;
            }

            public DimensionSlider(GameObject gameObject, string name, float max, System.Action action, bool updateFade = false)
            {
                this.max = max;
                this.updateFade = updateFade;

                slider = gameObject.transform.Find(name).gameObject.AddComponent<FSlider>();
                field = slider.transform.Find("InputField").gameObject.AddComponent<FNumberInputField>();
                overlayImage = slider.transform.Find("Background").GetComponent<Image>();

                if(updateFade)
                    fade = slider.transform.Find("BackgroundFade").GetComponent<Image>();

                slider.AttachInputField(field, (x) => x / this.max);
                slider.OnChange += action;
            }

            internal void RefreshColors(float val, Color color, Color? desat = null)
            {
                field.SetDisplayValue(Mathf.Ceil(val * max).ToString());
                overlayImage.color = color;
                if (updateFade)
                    fade.color = (Color)desat;
            }
        }
    }
}