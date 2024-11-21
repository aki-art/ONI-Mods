using Backwalls.Buildings;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
	public class FavoriteSelector : KMonoBehaviour
	{
		[SerializeField] private PresetToggle patternTogglePrefab;
		[SerializeField] private ToggleGroup patternToggleGroup;
		public Action<BackwallPattern> OnSetVariant;
		private LayoutElement layoutElement;

		private readonly List<PresetToggle> patternToggles = [];

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			var patterns = transform.Find("Scroll View/Viewport/Content");
			patternTogglePrefab = patterns.Find("TogglePrefab").gameObject.AddOrGet<PresetToggle>();
			patternToggleGroup = patterns.gameObject.AddOrGet<ToggleGroup>();
			patternToggleGroup.allowSwitchOff = true;
			patternToggleGroup.SetAllTogglesOff();
			layoutElement = GetComponent<LayoutElement>();
		}

		public void RemoveFavoriteToggle(PresetToggle toggle)
		{
			Log.Debug("removing favorite");
			if (toggle == null)
				return;

			Mod.Settings.FavoritedPatterns.RemoveAll(p => p.Index == toggle.settingsIndex);
			Mod.SaveSettings();

			Log.Debug("removed from mod settings");

			patternToggles.Remove(toggle);
			toggle.gameObject.SetActive(false);

			Log.Debug("hidden");
			Destroy(toggle);
		}

		public void AddFavoriteToggle(string pattern, Color color, int existingIndex = -1)
		{
			if (!Mod.variants.TryGetValue(pattern, out var variant))
				return;

			var toggle = Instantiate(patternTogglePrefab, patternToggleGroup.transform);

			var image = toggle.transform.Find("Image").GetComponent<Image>();
			image.sprite = variant.UISprite;
			image.color = color;

			toggle.transform.Find("Color").GetComponent<Image>().color = color;

			toggle.gameObject.SetActive(true);

			toggle.Setup(variant);

			var lastIndex = (existingIndex == -1)
				? Mod.Settings.FavoritedPatterns.Count == 0
					? 0
					: Mod.Settings.FavoritedPatterns.Max(p => p.Index)
				: existingIndex;

			toggle.settingsIndex = lastIndex + 1;

			toggle.group = patternToggleGroup;
			toggle.color = color;
			toggle.onValueChanged.AddListener(value =>
			{
				if (value)
				{
					//OnSetVariant?.Invoke(toggle.pattern);
					BackwallSidescreen.Instance.patternSelector.SetPattern(toggle.pattern.ID, true);
					BackwallSidescreen.Instance.hsvColorSelector.SetColor(toggle.color, true);
				}

				toggle.UpdateState(value);
			});

			toggle.onHover += () =>
			{
				BackwallSidescreen.Instance.patternSelector.activeToggle = toggle;
				BackwallSidescreen.Instance.patternSelector.lastActiveToggle = toggle;
			};

			toggle.onHoverEnd += () =>
			{
				if (BackwallSidescreen.Instance.patternSelector.activeToggle == toggle)
					BackwallSidescreen.Instance.patternSelector.activeToggle = null;
			};

			patternToggleGroup.RegisterToggle(toggle);
			patternToggles.Add(toggle);

			Helper.AddSimpleToolTip(toggle.gameObject, variant.name);
		}

		public void SetupVariantToggles()
		{
			foreach (var preset in Mod.Settings.FavoritedPatterns)
			{
				AddFavoriteToggle(preset.Pattern, Util.ColorFromHex(preset.Color), preset.Index);
			}
		}

		public void UpdateHeight()
		{
			if (patternToggles.Count <= 5)
				layoutElement.minHeight = 43f;
			else layoutElement.minHeight = 89f;
		}

		public class PresetToggle : PatternSelector.PatternToggle
		{
			public Color color;
			public int settingsIndex;

			public void SetColor(Color color)
			{
				this.color = color;
				transform.Find("Color").GetComponent<Image>().color = color;
			}
		}
	}
}
