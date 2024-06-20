using System;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
	public class SwatchSelector : KMonoBehaviour
	{
		public const int Invalid = -1;

		[SerializeField]
		private ColorToggle togglePrefab;

		[SerializeField]
		private ToggleGroup toggleGroup;

		private ColorToggle[] toggles;

		[SerializeField]
		public Action<Color, int> OnChange;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			togglePrefab = transform.Find("SwatchPrefab").gameObject.AddOrGet<ColorToggle>();
			toggleGroup = transform.gameObject.AddOrGet<ToggleGroup>();
			toggleGroup.allowSwitchOff = true;
		}

		internal void SetSwatch(int index, bool triggerUpdate)
		{
			Log.Debug("set swatch " + index);
			DeselectAll(false);

			if (toggles == null)
			{
				SetupColorToggles();
			}

			if (index >= 0 && index < toggles.Length)
			{
				var toggle = toggles[index];

				if (toggle == null)
				{
					Log.Warning("single toggle has a null entry");
					return;
				}

				if (triggerUpdate)
				{
					Log.Debug("update trugger");
					toggle.isOn = true;
				}
				else
				{
					Log.Debug("update no trigger");
					toggle.SetIsOnWithoutNotify(true);
					toggle.UpdateSoundsAndVisuals(true);
				}
			}
		}


		public void DeselectAll(bool trigger)
		{
			Log.Debug("DeselectAll");
			if (toggleGroup == null)
			{
				Log.Warning("togglegroup is null");
				toggleGroup = transform.gameObject.AddOrGet<ToggleGroup>();
			}

			if (toggles == null)
			{
				return;
			}

			foreach (var toggle in toggles)
			{
				toggle.SetIsOnWithoutNotify(false);
				toggle.UpdateSoundsAndVisuals(false);
			}
		}

		public void Setup()
		{
			SetupColorToggles();
		}

		private void SetupColorToggles()
		{
			if (toggles != null && toggles.Length > 0)
			{
				return;
			}

			toggles = new ColorToggle[ModAssets.colors.Length];

			for (var i = 0; i < ModAssets.colors.Length; i++)
			{
				if (toggles[i] != null)
				{
					continue;
				}

				var color = ModAssets.colors[i];

				var toggle = Instantiate(togglePrefab, toggleGroup.transform);
				toggle.Setup(i);
				toggle.group = toggleGroup;
				toggleGroup.RegisterToggle(toggle);
				toggle.onValueChanged.AddListener(value =>
				{
					Log.Debug("on value changed");
					toggle.OnToggle(value);
					if (value)
					{
						OnChange?.Invoke(color, toggle.swatchIdx);
					}
				});

				toggle.gameObject.SetActive(true);
				toggles[i] = toggle;
			}
		}

		public class ColorToggle : Toggle
		{
			public int swatchIdx;
			private Outline outline;

			public void Setup(int color)
			{
				swatchIdx = color;
				outline = GetComponent<Outline>();
				GetComponent<Image>().color = ModAssets.colors[color];
			}

			public void OnToggle(bool on)
			{
				Log.Debug("on toggle " + swatchIdx + " " + on);
				UpdateSoundsAndVisuals(on);
			}

			public void UpdateSoundsAndVisuals(bool on)
			{
				Log.Debug("update sounds and visuals " + swatchIdx + " " + on);
				if (on)
				{
					PlaySound(CONSTS.UI_SOUNDS_EVENTS.CLICK);
					outline.effectDistance = Vector2.one * 2f;
					outline.effectColor = Color.cyan;
				}
				else
				{
					outline.effectDistance = Vector2.one;
					outline.effectColor = Color.black;
				}
			}
		}
	}
}
