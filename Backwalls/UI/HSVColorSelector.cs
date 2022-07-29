using FUtility;
using System;
using System.Text.RegularExpressions;
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
        private Color color;
        private Regex allowedHex = new Regex(@"/[0-9A-Fa-f]{6}/g");
        private string hex;


        [SerializeField]
        private FInputField hexInput;

        public event Action<Color> OnChange;

        private bool triggerState = true;

        public void SetColor(Color color, bool trigger = false)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);

            triggerState = trigger;

            hueSlider.Value = h * 255f;
            saturationSlider.Value = s * 255f;
            valueSlider.Value = v * 255f;
            alphaSlider.Value = color.a * 255f;

            triggerState = true;

            hueSlider.UpdateColors(color, h, s, v, color.a);
            saturationSlider.UpdateColors(color, h, s, v, color.a);
            valueSlider.UpdateColors(color, h, s, v, color.a);
            alphaSlider.UpdateColors(color, h, s, v, color.a);

            var newHex = color.ToHexString();
            if (hex != newHex)
            {
                hex = newHex;
                hexInput.Text = hex;
            }
            Log.Debuglog("color updated");
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            hueSlider = transform.Find("Hue/Slider").gameObject.AddComponent<HueSlider>();
            saturationSlider = transform.Find("Sat/Slider").gameObject.AddComponent<SaturationSlider>();
            valueSlider = transform.Find("Val/Slider").gameObject.AddComponent<ValueSlider>();
            alphaSlider = transform.Find("Alpha/Slider").gameObject.AddComponent<AlphaSlider>();

            hexInput = transform.Find("Misc/HexInput").gameObject.AddComponent<FInputField>();

            var tmpInput = hexInput.GetComponent<TMP_InputField>();
            tmpInput.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            tmpInput.inputValidator = ScriptableObject.CreateInstance<HexValidator>();

            tmpInput.onValueChanged.AddListener(OnHexEdited);
            tmpInput.onSubmit.AddListener(OnHexChanged);
        }

        private void OnHexEdited(string arg0)
        {
            if(arg0.Length == 6 || arg0.Length == 8)
            {
                hexInput.Submit();
            }
        }

        private void OnHexChanged(string value)
        {
            Log.Debuglog("hex changed " + value);

            if (value.IsNullOrWhiteSpace())
            {
                return;
            }

            if (value.Length == 8 || value.Length == 6)
            {
                var color = Util.ColorFromHex(value);
                SetColor(color, true);
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

            color = Color.HSVToRGB(h, s, v);
            color.a = a;

            hueSlider.UpdateColors(color, h, s, v, a);
            saturationSlider.UpdateColors(color, h, s, v, a);
            valueSlider.UpdateColors(color, h, s, v, a);
            alphaSlider.UpdateColors(color, h, s, v, a);

            var newHex = color.ToHexString();
            if (!hexInput.IsEditing() && hex != newHex)
            {
                hex = newHex;
                hexInput.Text = hex;
            }

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
