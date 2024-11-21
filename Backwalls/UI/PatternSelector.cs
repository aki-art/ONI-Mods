using Backwalls.Buildings;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Backwalls.UI
{
	public class PatternSelector : KScreen
	{
		public const float SCREEN_SORT_KEY = 300f;

		new bool ConsumeMouseScroll = true; // do not remove!!!!
		private bool shown = false;
		public bool pause = true;

		[SerializeField] private PatternToggle patternTogglePrefab;
		[SerializeField] private ToggleGroup patternToggleGroup;

		public PatternToggle activeToggle;
		public PatternToggle lastActiveToggle;
		public string currentPattern;
		public Action<BackwallPattern> OnSetVariant;

		private Dictionary<string, PatternToggle> patternToggles = new Dictionary<string, PatternToggle>();

		public PatternToggle ActiveToggle() => activeToggle;

		public override float GetSortKey() => -1;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			var patterns = transform.Find("Scroll View/Viewport/Content");
			patternTogglePrefab = patterns.Find("TogglePrefab").gameObject.AddOrGet<PatternToggle>();
			patternToggleGroup = patterns.gameObject.AddOrGet<ToggleGroup>();
			patternToggleGroup.allowSwitchOff = true;
			patternToggleGroup.SetAllTogglesOff();
		}

		public void SetPattern(string pattern, bool triggerUpdate)
		{
			if (patternToggles.TryGetValue(pattern, out var toggle))
			{
				currentPattern = pattern;
				if (triggerUpdate)
				{
					toggle.isOn = true;
				}
				else
				{
					toggle.SetIsOnWithoutNotify(true);
				}
			}
		}

		private bool IsPatternHiddenByUser(string pattern)
		{
			if (Mod.Settings.HiddenPatterns == null)
				return false;

			return Mod.Settings.HiddenPatterns.Contains(pattern);
		}

		public void SetupVariantToggles()
		{
			/*			var groups = new Dictionary<string, List<string>>();

						var variants = new List<BackwallPattern>(Mod.variants.Values);
						for (int i = 0; i < variants.Count; i++)
						{
							var variant = variants[i];
							if (variant.BorderTag.IsNullOrWhiteSpace())
								groups[variant.ID] = [variant.ID];

							else
							{
								if (!groups.ContainsKey(variant.BorderTag))
									groups[variant.BorderTag] = [];

								groups[variant.BorderTag].Add(variant.ID);
							}
						}*/

			var sortedVariants = Mod.variants
				.Values
				.OrderBy(v => v.GetSortOrder())
				.ThenBy(v => v.name)
				.ToList();

			//foreach (Transform toggle in patternToggleGroup.transform)
			//{
			//Destroy(toggle);
			//}

			foreach (var variant in sortedVariants)
			{
				if (patternToggles.ContainsKey(variant.ID))
				{
					continue;
				}

				var toggle = Instantiate(patternTogglePrefab, patternToggleGroup.transform);
				toggle.transform.Find("Image").GetComponent<Image>().sprite = variant.UISprite;
				toggle.gameObject.SetActive(!IsPatternHiddenByUser(variant.ID));
				toggle.Setup(variant);
				toggle.group = patternToggleGroup;
				toggle.onValueChanged.AddListener(value =>
				{
					if (value)
					{
						OnSetVariant?.Invoke(toggle.pattern);
						currentPattern = toggle.pattern.ID;
					}

					toggle.UpdateState(value);
				});

				toggle.onHover += () =>
				{
					activeToggle = toggle;
					lastActiveToggle = toggle;
				};
				toggle.onHoverEnd += () =>
				{
					if (activeToggle == toggle)
						activeToggle = null;
				};

				patternToggleGroup.RegisterToggle(toggle);
				patternToggles.Add(variant.ID, toggle);

				Helper.AddSimpleToolTip(toggle.gameObject, variant.name);
			}
		}

		public void UpdateToggles()
		{
			Log.Debug("updating toggles");

			if (Mod.Settings.HiddenPatterns == null || patternToggles == null)
				return;

			foreach (var toggle in patternToggles)
			{
				var isHiddenByUser = Mod.Settings.HiddenPatterns.Contains(toggle.Key);
				toggle.Value.gameObject.SetActive(BackwallSidescreen.Instance.isAllPatternsVisible || !isHiddenByUser);
				toggle.Value.UpdateColor();
			}
		}

		public void HideAllWithToggle(string borderTag)
		{
			if (borderTag == null)
				return;

			foreach (var toggle in patternToggles)
			{
				if (toggle.Value.pattern.BorderTag == borderTag)
				{
					toggle.Value.Hide(true, false);
					toggle.Value.gameObject.SetActive(BackwallSidescreen.Instance.isAllPatternsVisible);
				}
			}

			Mod.SaveSettings();
		}

		public class PatternToggle : Toggle, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
		{
			public BackwallPattern pattern;
			private Image bg;
			private static Color defaultColor = new Color32(62, 67, 87, 255);
			private static Color selectedColor = new Color32(129, 138, 179, 255);
			private static Color hiddenColor = Util.ColorFromHex("a3a3a3") with { a = 0.7f };
			private static Color hiddenSelectedColor = Util.ColorFromHex("c2c2c2") with { a = 0.7f };
			public System.Action onHover;
			public System.Action onHoverEnd;
			public bool hidden;

			public override void OnPointerEnter(PointerEventData eventData)
			{
				base.OnPointerEnter(eventData);
				onHover?.Invoke();
			}

			public override void OnPointerExit(PointerEventData eventData)
			{
				base.OnPointerExit(eventData);
				onHoverEnd?.Invoke();
			}

			public override void OnPointerClick(PointerEventData eventData)
			{
				if (eventData.button == PointerEventData.InputButton.Right)
				{
					BackwallSidescreen.Instance.ShowContextMenu();
				}
				else
					base.OnPointerClick(eventData);
			}

			public virtual void Setup(BackwallPattern pattern)
			{
				this.pattern = pattern;
				bg = GetComponent<Image>();
			}

			public void ToggleHidden()
			{
				Hide(!Mod.Settings.HiddenPatterns.Contains(pattern.ID), true);
			}

			public void Hide(bool hidden, bool saveSettings)
			{
				this.hidden = hidden;
				Mod.Settings.HiddenPatterns ??= [];

				if (hidden)
					Mod.Settings.HiddenPatterns.Add(pattern.ID);
				else
					Mod.Settings.HiddenPatterns.Remove(pattern.ID);

				Log.Debug($"hidden {pattern.ID} {hidden}");

				if (saveSettings)
					Mod.SaveSettings();
			}

			public void UpdateState(bool on)
			{
				if (on)
				{
					PlaySound(CONSTS.UI_SOUNDS_EVENTS.CLICK);
					bg.color = hidden ? hiddenSelectedColor : selectedColor;
				}
				else
				{
					bg.color = hidden ? hiddenColor : defaultColor;
				}
			}

			public void UpdateColor()
			{
				if (isOn)
					bg.color = hidden ? hiddenSelectedColor : selectedColor;
				else
					bg.color = hidden ? hiddenColor : defaultColor;
			}
		}
	}
}
