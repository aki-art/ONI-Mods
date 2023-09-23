using Backwalls.Buildings;
using Backwalls.Cmps;
using FUtility;
using FUtility.FUI;
using rendering;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
	public class BackwallSidescreen : SideScreenContent
	{
		private Backwall target;

		[SerializeField] private HSVColorSelector hsvColorSelector;
		[SerializeField] private PatternSelector patternSelector;
		[SerializeField] private SwatchSelector swatchSelector;
		[SerializeField] private Toggle copyPatternToggle;
		[SerializeField] private Toggle copyColorToggle;
		//[SerializeField] private Toggle shinyToggle;
		//[SerializeField] private Toggle bordersToggle;
		[SerializeField] private TabToggle showHSVToggle;
		[SerializeField] private TabToggle showSwatchToggle;
		[SerializeField] private GameObject noCopyWarning;
		[SerializeField] private FButton setDefaultsButton;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();

			titleKey = "Backwalls.STRINGS.UI.WALLSIDESCREEN.TITLE";

			hsvColorSelector = transform.Find("Contents/ColorSelector").gameObject.AddOrGet<HSVColorSelector>();
			swatchSelector = transform.Find("Contents/ColorGrid").gameObject.AddOrGet<SwatchSelector>();
			patternSelector = transform.Find("Contents/FacadeSelector").gameObject.AddOrGet<PatternSelector>();

			showHSVToggle = transform.Find("Contents/ColorTabs/Sliders").gameObject.AddOrGet<TabToggle>();
			showSwatchToggle = transform.Find("Contents/ColorTabs/Swatches").gameObject.AddOrGet<TabToggle>();

			copyPatternToggle = transform.Find("Contents/CopyToggles/PatternToggle/Toggle").GetComponent<Toggle>();
			copyColorToggle = transform.Find("Contents/CopyToggles/ColorToggle/Toggle").GetComponent<Toggle>();

			noCopyWarning = transform.Find("Contents/CopyToggles/Warning").gameObject;
			setDefaultsButton = transform.Find("Contents/SetDefaultButton/Button").gameObject.AddOrGet<FButton>();

			var toggles = transform.Find("Contents/Toggles");
			toggles.gameObject.SetActive(false);

			//shinyToggle = transform.Find("Contents/Toggles/ShinyToggle/Toggle").GetComponent<Toggle>();
			//bordersToggle = transform.Find("Contents/Toggles/BordersToggle/Toggle").GetComponent<Toggle>();
		}

		public override bool IsValidForTarget(GameObject target)
		{
			return target.TryGetComponent(out Backwall _);
		}

		public override void SetTarget(GameObject target)
		{
			if (target != null && target.TryGetComponent(out Backwall newTarget))
			{
				if (newTarget.settings.swatchIdx != SwatchSelector.Invalid)
				{
					swatchSelector.SetSwatch(newTarget.settings.swatchIdx, false);
				}
				else
				{
					hsvColorSelector.SetColor(Util.ColorFromHex(newTarget.settings.colorHex), false);
				}

				patternSelector.SetPattern(newTarget.settings.pattern, false);
/*
				Log.Debug("set target: " + newTarget.settings.pattern);
				if(newTarget.CanBeShiny())
				{
					Log.Debug("can be shiny");
					shinyToggle.interactable = true;
					shinyToggle.SetIsOnWithoutNotify(newTarget.settings.shiny);
				}
				else
				{
					Log.Debug("can NOT be shiny");
					shinyToggle.interactable = false;
					shinyToggle.SetIsOnWithoutNotify(false);
				}*/

				this.target = newTarget;
			}
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			gameObject.SetActive(true);
			RefreshUI();

			setDefaultsButton.gameObject.GetComponentInChildren<LocText>().text = STRINGS.UI.BACKWALLS_DEFAULTSETTERSIDESCREEN.SET_DEFAULT.BUTTON;
			Helper.AddSimpleToolTip(setDefaultsButton.gameObject, STRINGS.UI.BACKWALLS_DEFAULTSETTERSIDESCREEN.SET_DEFAULT.TOOLTIP);
		}

		private void RefreshUI()
		{
			patternSelector.SetupVariantToggles();
			swatchSelector.Setup();

			copyColorToggle.isOn = Backwalls_Mod.Instance.CopyColor;
			copyColorToggle.onValueChanged.AddListener(on =>
			{
				noCopyWarning.SetActive(!copyColorToggle.isOn && !copyPatternToggle.isOn);
				Backwalls_Mod.Instance.CopyColor = on;
				Mod.SaveSettings();
			});

			copyPatternToggle.isOn = Backwalls_Mod.Instance.CopyPattern;
			copyPatternToggle.onValueChanged.AddListener(on =>
			{
				noCopyWarning.SetActive(!copyColorToggle.isOn && !copyPatternToggle.isOn);
				Backwalls_Mod.Instance.CopyPattern = on;
				Mod.SaveSettings();
			});

			showHSVToggle.Setup(hsvColorSelector.gameObject);
			showSwatchToggle.Setup(swatchSelector.gameObject);

			showHSVToggle.onValueChanged.AddListener(on =>
			{
				Backwalls_Mod.Instance.ShowHSV = on;
				Mod.SaveSettings();

				showHSVToggle.OnToggle(on);
			});
			showHSVToggle.isOn = Backwalls_Mod.Instance.ShowHSV;

			showSwatchToggle.onValueChanged.AddListener(on =>
			{
				Backwalls_Mod.Instance.ShowSwatches = on;
				Mod.SaveSettings();

				showSwatchToggle.OnToggle(on);
			});

			showSwatchToggle.isOn = Backwalls_Mod.Instance.ShowSwatches;

			//shinyToggle.onValueChanged.AddListener(OnShinyChanged);

			hsvColorSelector.OnChange += OnHSVColorChange;
			patternSelector.OnSetVariant += OnPatternChange;
			swatchSelector.OnChange += OnSwatchChange;

			noCopyWarning.SetActive(!copyColorToggle.isOn && !copyPatternToggle.isOn);

			setDefaultsButton.OnClick += OnSetDefaults;
		}

		private void OnSetDefaults()
		{
			if (target == null)
				return;

			Mod.Settings.DefaultPattern = target.settings.pattern;
			Mod.Settings.DefaultColor = target.settings.colorHex;
			Mod.SaveSettings();
		}

		/*		private void OnShinyChanged(bool value)
				{
					if(target == null) return;
					Log.Debug($"shiny changed to {value} on {Grid.PosToCell(target)}");
					target.SetShiny(value);
				}
		*/
		private void OnHSVColorChange(Color color)
		{
			if (target == null || (target.TryGetComponent(out KSelectable kSelectable) && !kSelectable.IsSelected))
			{
				return;
			}

			target.SetColor(color);
			if (swatchSelector.isActiveAndEnabled)
			{
				swatchSelector.SetSwatch(SwatchSelector.Invalid, false);
			}
		}

		private void OnPatternChange(BackwallPattern pattern)
		{
			if (target == null) 
				return;

			target.SetPattern(pattern);
		}

		private void OnSwatchChange(Color color, int index)
		{
			if (target == null || (target.TryGetComponent(out KSelectable kSelectable) && !kSelectable.IsSelected))
			{
				return;
			}

			target.SetColor(index);

			if (hsvColorSelector.isActiveAndEnabled)
			{
				hsvColorSelector.SetColor(color, false);
			}
		}

		public override void OnKeyDown(KButtonEvent e)
		{
			base.OnKeyDown(e);
		}

		public class TabToggle : Toggle
		{
			private Image icon;
			private static Color offColor = new Color(0.7f, 0.7f, 0.7f);
			private static Color onColor = Color.black;

			[SerializeField] public GameObject target;

			public void Setup(GameObject target)
			{
				icon = GetComponent<Image>();
				this.target = target;
			}

			public void OnToggle(bool on)
			{
				icon.color = on ? onColor : offColor;
				target.SetActive(on);
			}
		}
	}
}
