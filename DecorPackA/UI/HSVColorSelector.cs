using FUtility.FUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.UI
{
	public class HSVColorSelector : KMonoBehaviour
	{
		private HueSlider hueSlider;
		private SaturationSlider saturationSlider;
		private ValueSlider valueSlider;

		public event Action<Color> OnChange;

		private bool triggerState = true;

		public void SetColor(Color color, bool trigger = false)
		{
			Color.RGBToHSV(color, out var h, out var s, out var v);

			triggerState = trigger;

			hueSlider.Value = Mathf.Floor(h * 255f);
			saturationSlider.Value = Mathf.Floor(s * 255f);
			valueSlider.Value = Mathf.Floor(v * 255f);

			triggerState = true;

			UpdateAllColors(color, h, s, v);
		}

		private void UpdateAllColors(Color color, float h, float s, float v)
		{
			hueSlider.UpdateColors(color, h, s, v, color.a);
			saturationSlider.UpdateColors(color, h, s, v, color.a);
			valueSlider.UpdateColors(color, h, s, v, color.a);
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			hueSlider = transform.Find("Hue/Slider").gameObject.AddOrGet<HueSlider>();
			saturationSlider = transform.Find("Sat/Slider").gameObject.AddOrGet<SaturationSlider>();
			valueSlider = transform.Find("Val/Slider").gameObject.AddOrGet<ValueSlider>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			hueSlider.slider.onValueChanged.AddListener(OnSliderChanged);
			saturationSlider.slider.onValueChanged.AddListener(OnSliderChanged);
			valueSlider.slider.onValueChanged.AddListener(OnSliderChanged);
		}

		private void OnSliderChanged(float value)
		{
			var h = hueSlider.Value / 255f;
			var s = saturationSlider.Value / 255f;
			var v = valueSlider.Value / 255f;

			var color = Color.HSVToRGB(h, s, v);

			UpdateAllColors(color, h, s, v);

			if (triggerState)
				OnChange?.Invoke(color);
		}

		public abstract class ColorSlider : KMonoBehaviour
		{
			protected FInputField2 numberInput;
			public Slider slider;
			protected Image background;

			public float Value
			{
				get => slider.value;
				set => slider.value = value;
			}

			public override void OnPrefabInit()
			{
				base.OnPrefabInit();

				numberInput = transform.Find("InputField").gameObject.AddComponent<FInputField2>();

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
				if (int.TryParse(value, out var number))
				{
					if (slider.value == number)
					{
						return;
					}

					if (number < 0 || number > 255)
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

			public override void OnPrefabInit()
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
	}
}
