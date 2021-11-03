using UnityEngine;
using UnityEngine.UI;

namespace FUtility.FUI
{
    class ColorSlider
    {
        public FSlider slider;
        private readonly FNumberInputField field;
        private readonly Image overlayImage;
        private readonly Image fade;
        private readonly bool updateFade;
        private readonly float max;
        public int index;
        readonly Dimension dimension;

        public float Value
        {
            get => slider.Value;
            set => slider.Value = value;
        }

        public ColorSlider(GameObject gameObject, Dimension dim, string name, System.Action onChange)
        {
            switch(dim)
            {
                case Dimension.Hue:
                    max = 255;
                    index = 0; 
                    updateFade = false;
                    break;
                case Dimension.Saturation:
                    max = 100;
                    index = 1;
                    updateFade = true;
                    break;
                case Dimension.Value:
                    max = 100;
                    index = 2;
                    updateFade = false;
                    break;
                case Dimension.Alpha:
                    max = 100;
                    index = 3;
                    updateFade = false;
                    break;
                default:
                    Log.Warning("Invalid color dimension.");
                    break;
            }

            slider = gameObject.transform.Find(name).gameObject.AddComponent<FSlider>();
            field = slider.transform.Find("InputField").gameObject.AddComponent<FNumberInputField>();
            overlayImage = slider.transform.Find("Background").GetComponent<Image>();

            if (updateFade)
                fade = slider.transform.Find("BackgroundFade").GetComponent<Image>();

            slider.AttachInputField(field, (x) => x / max);
            slider.OnChange += onChange;
        }

        internal void RefreshColors(HSVColor color)
        {
            switch (dimension)
            {
                case Dimension.Saturation:
                    color.s = 1;
                    break;
                case Dimension.Value:
                    color.v = 1;
                    break;
                case Dimension.Alpha:
                case Dimension.Hue:
                default:
                    color = Color.white;
                    break;
            }

            field.SetDisplayValue(Mathf.Ceil(color[index] * max).ToString());
            overlayImage.color = color;
            if (updateFade)
                fade.color = color.Desat;
        }

        public enum Dimension
        {
            Hue,
            Saturation,
            Value,
            Alpha
        }
    }
}