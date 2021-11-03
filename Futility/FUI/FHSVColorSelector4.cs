using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FUtility.FUI
{
    public class FHSVColorSelector4 : KMonoBehaviour
    {
        public string Hex => Util.ToHexString(rgb);
        public Color rgb;
        public HSVColor hsv;
        private HSVColor defaultHsv;

        public event System.Action OnChange;

        private List<ColorSlider> sliders;

        private Image previewImage;
        private FButton resetButton;
        private Image resetImg;

        public void Initialize(string hex) => Initialize(Util.ColorFromHex(hex));
        public void Initialize(Color color)
        {
            defaultHsv = color;
            SetColor(color);
            UpdateColorPreviews();
        }

        protected override void OnPrefabInit()
        {
            SetObjects();

            sliders = new List<ColorSlider>()
            {
                new ColorSlider(gameObject, ColorSlider.Dimension.Hue, "HueSlider", SliderOnChange),
                new ColorSlider(gameObject, ColorSlider.Dimension.Saturation, "SaturationSlider", SliderOnChange),
                new ColorSlider(gameObject, ColorSlider.Dimension.Value, "ValueSlider", SliderOnChange)
            };

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
            rgb = RGB;
            hsv = rgb.HSV();
            UpdateAll();
        }

        public void SetColorFromHex(string hex) => SetColor(Util.ColorFromHex(hex));

        private void SliderOnChange()
        {
            foreach (ColorSlider slider in sliders)
                hsv[slider.index] = slider.Value;

            rgb = hsv.RGB;

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
            foreach (ColorSlider slider in sliders)
                slider.RefreshColors(hsv);
        }

        private void UpdateSliderValues()
        {
            foreach (ColorSlider slider in sliders)
                slider.Value = hsv[slider.index];
        }

        private void UpdateColorPreviews()
        {
            previewImage.color = rgb;

            bool NotDefault = !rgb.Equals(defaultHsv);

            resetImg.gameObject.SetActive(NotDefault);
            if (NotDefault)
                resetImg.color = defaultHsv;
        }
        private void ResetColor() => SetColor(defaultHsv);

    }
}