using FUtility.FUI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.UI
{
	public class SwatchSelector : KMonoBehaviour
	{
		public const int Invalid = -1;

		[SerializeField] private ColorToggle togglePrefab;
		[SerializeField] private ToggleGroup toggleGroup;
		[SerializeField] public Action<Color, int> OnChange;

		private ColorToggle[] toggles;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Initialize();
		}

		private void Initialize()
		{
			if (togglePrefab != null)
				return;

			togglePrefab = transform.Find("SwatchPrefab").gameObject.AddOrGet<ColorToggle>();
			toggleGroup = transform.gameObject.AddOrGet<ToggleGroup>();
			toggleGroup.allowSwitchOff = true;
		}

		public void SetSwatch(int index, bool triggerUpdate)
		{
			Deselect_Internal();
			SetupColorToggles();

			if (index == Invalid)
				return;

			if (index >= 0 && index < toggles.Length)
			{
				var toggle = toggles[index];

				if (toggle == null)
				{
					Log.Warning("single toggle has a null entry");
					return;
				}

				if (triggerUpdate)
					toggle.isOn = true;
				else
				{
					toggle.SetIsOnWithoutNotify(true);
					toggle.UpdateSoundsAndVisuals(true);
				}
			}
		}

		public void Deselect()
		{
			Deselect_Internal();
			SetupColorToggles();
		}

		private void Deselect_Internal()
		{
			if (toggleGroup == null)
				toggleGroup = transform.gameObject.AddOrGet<ToggleGroup>();

			if (toggles == null)
				return;

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
			Initialize();

			if (toggles != null && toggles.Length > 0)
				return;

			toggles = new ColorToggle[ModAssets.colors.Length];

			for (var i = 0; i < ModAssets.colors.Length; i++)
			{
				if (toggles[i] != null)
					continue;

				var color = ModAssets.colors[i];

				var toggle = Instantiate(togglePrefab, toggleGroup.transform);
				toggle.Setup(i);
				toggle.group = toggleGroup;
				toggleGroup.RegisterToggle(toggle);
				toggle.onValueChanged.AddListener(value =>
				{
					toggle.OnToggle(value);
					if (value)
						OnChange?.Invoke(color, toggle.swatchIdx);
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
				UpdateSoundsAndVisuals(on);
			}

			public void UpdateSoundsAndVisuals(bool on)
			{
				if (on)
				{
					PlaySound(UISoundHelper.Click);
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
