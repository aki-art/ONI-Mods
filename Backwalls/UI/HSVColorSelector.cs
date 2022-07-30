using FUtility;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
    internal class HSVColorSelector : KMonoBehaviour
    {
        private HueSlider hueSlider;
        private SaturationSlider saturationSlider;
        private ValueSlider valueSlider;
        private AlphaSlider alphaSlider;
        private Image preview;
        //private string hex;

        //[SerializeField]
        //private FInputField hexInput;

        public event Action<Color> OnChange;

        private bool triggerState = true;

        public void SetColor(Color color, bool trigger = false)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);

            triggerState = trigger;

            hueSlider.Value = Mathf.Floor(h * 255f);
            saturationSlider.Value = Mathf.Floor(s * 255f);
            valueSlider.Value = Mathf.Floor(v * 255f);
            alphaSlider.Value = Mathf.Floor(color.a * 255f);

            triggerState = true;

            UpdateAllColors(color, h, s, v);
        }

        private void UpdateAllColors(Color color, float h, float s, float v)
        {
            /*
            var newHex = color.ToHexString();
            if (hex != newHex)
            {
                hex = newHex;
                hexInput.Text = hex;
            }
            */

            hueSlider.UpdateColors(color, h, s, v, color.a);
            saturationSlider.UpdateColors(color, h, s, v, color.a);
            valueSlider.UpdateColors(color, h, s, v, color.a);
            alphaSlider.UpdateColors(color, h, s, v, color.a);

            preview.color = color;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            hueSlider = transform.Find("Hue/Slider").gameObject.AddComponent<HueSlider>();
            saturationSlider = transform.Find("Sat/Slider").gameObject.AddComponent<SaturationSlider>();
            valueSlider = transform.Find("Val/Slider").gameObject.AddComponent<ValueSlider>();
            alphaSlider = transform.Find("Alpha/Slider").gameObject.AddComponent<AlphaSlider>();

            var hexInput = transform.Find("Misc/HexInput").gameObject; //.AddComponent<FInputField>();
            hexInput.SetActive(false);

            //var tmpInput = hexInput.GetComponent<TMP_InputField>();
            //tmpInput.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            //tmpInput.inputValidator = ScriptableObject.CreateInstance<HexValidator>();

            //tmpInput.onValueChanged.AddListener(OnHexEdited);
            //tmpInput.onSubmit.AddListener(OnHexChanged);

            preview = transform.Find("Misc/Swatch").GetComponent<Image>();
        }

        /*
        private void OnHexEdited(string value)
        {
            if((value.Length == 6 || value.Length == 8) && value != hex)
            {
                hexInput.Submit();
            }
        }

        private void OnHexChanged(string value)
        {
            Log.Debuglog("hex changed " + value);

            if (value.IsNullOrWhiteSpace() || !triggerState)
            {
                return;
            }

            if(!int.TryParse(value, out _))
            {
                return;
            }

            if (value.Length == 8 || value.Length == 6)
            {
                var color = Util.ColorFromHex(value);
                SetColor(color, true);

                hex = value;
            }
        }

        public class HexValidator : TMP_InputValidator
        {
            public override char Validate(ref string text, ref int pos, char ch)
            {
                if ((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f'))
                {
                    text = text.Insert(pos, ch.ToString());
                    pos += 1;

                    return ch;
                }
                else
                {
                    return (char)0;
                }
            }
        }
        */

        protected override void OnSpawn()
        {
            base.OnSpawn();

            hueSlider.slider.onValueChanged.AddListener(OnSliderChanged);
            saturationSlider.slider.onValueChanged.AddListener(OnSliderChanged);
            valueSlider.slider.onValueChanged.AddListener(OnSliderChanged);
            alphaSlider.slider.onValueChanged.AddListener(OnSliderChanged);

            transform.Find("Misc/HexInput/Text").gameObject.GetComponent<LocText>().text = "#";
        }

        private void OnSliderChanged(float value)
        {
            var h = hueSlider.Value / 255f;
            var s = saturationSlider.Value / 255f;
            var v = valueSlider.Value / 255f;
            var a = alphaSlider.Value / 255f;

            var color = Color.HSVToRGB(h, s, v);
            color.a = a;

            UpdateAllColors(color, h, s, v);

            if (triggerState)
            {
                OnChange?.Invoke(color);
            }
        }

        public abstract class ColorSlider : KMonoBehaviour
        {
            protected FInputField numberInput;
            public Slider slider;
            protected Image background;

            public float Value
            {
                get => slider.value;
                set => slider.value = value;
            }

            protected override void OnPrefabInit()
            {
                base.OnPrefabInit();

                numberInput = transform.Find("InputField").gameObject.AddComponent<FInputField>();

                var tmpInputField = numberInput.GetComponent<TMP_InputField>();

                // rehook references, these were lost on LocText conversion
                tmpInputField.textComponent = tmpInputField.textViewport.transform.Find("Text").GetComponent<LocText>();
                tmpInputField.placeholder = tmpInputField.textViewport.transform.Find("Placeholder").GetComponent<LocText>();

                tmpInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                tmpInputField.onValueChanged.AddListener(OnNumberChanged);

                slider = GetComponent<Slider>();
                slider.onValueChanged.AddListener(OnChange);

                background = transform.Find("Background").GetComponent<Image>();
            }

            private void OnNumberChanged(string value)
            {
                if(int.TryParse(value, out var number))
                {
                    if(slider.value == number)
                    {
                        return;
                    }

                    if(number < 0 || number > 255)
                    {
                        number = Mathf.Clamp(number, 0, 255);
                        numberInput.Text = number.ToString();
                        return;
                    }

                    slider.value = number;
                }
                else
                {
                    numberInput.Text = slider.value.ToString();
                }
            }

            protected virtual void OnChange(float value)
            {
                numberInput.Text = ((int)value).ToString();
            }

            public virtual void UpdateColors(Color color, float h, float s, float v, float a)
            {
            }
        }

        public class HueSlider : ColorSlider
        {
        }

        public class SaturationSlider : ColorSlider
        {
            private Image grayOverlay;

            protected override void OnPrefabInit()
            {
                base.OnPrefabInit();
                grayOverlay = transform.Find("Overlay").GetComponent<Image>();
            }

            public override void UpdateColors(Color color, float h, float s, float v, float a)
            {
                grayOverlay.color = new Color(v, v, v, 1);
                var overlayColor = color * grayOverlay.color;
                overlayColor.a = 1f;
                background.color = overlayColor;
            }
        }

        public class ValueSlider : ColorSlider
        {
            public override void UpdateColors(Color color, float h, float s, float v, float a)
            {
                background.color = Color.HSVToRGB(h, s, 1f);
            }
        }

        public class AlphaSlider : ColorSlider
        {
            private Image solidOverlay;

            protected override void OnPrefabInit()
            {
                base.OnPrefabInit();
                solidOverlay = transform.Find("OverlaySolid").GetComponent<Image>();
            }

            public override void UpdateColors(Color color, float h, float s, float v, float a)
            {
                solidOverlay.color = new Color(color.r, color.g, color.b, 1f);
            }
        }
    }
}
