using Backwalls.Buildings;
using Backwalls.Cmps;
using FUtility.FUI;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
	public class BackwallSidescreen : SideScreenContent
	{
		private Backwall target;

		[SerializeField] public HSVColorSelector hsvColorSelector;
		[SerializeField] public PatternSelector patternSelector;
		[SerializeField] public FavoriteSelector favoriteSelector;
		[SerializeField] private SwatchSelector swatchSelector;
		[SerializeField] private ContextMenu contextMenu;
		[SerializeField] private Toggle copyPatternToggle;
		[SerializeField] private Toggle copyColorToggle;
		//[SerializeField] private Toggle shinyToggle;
		//[SerializeField] private Toggle bordersToggle;
		[SerializeField] private TabToggle showHSVToggle;
		[SerializeField] private TabToggle showSwatchToggle;
		[SerializeField] private HiddenToggle showHiddenToggle;
		[SerializeField] private GameObject noCopyWarning;
		[SerializeField] private FButton setDefaultsButton;
		[SerializeField] private FButton addFavoriteButton;

		public bool isAllPatternsVisible;

		public static BackwallSidescreen Instance { get; private set; }

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			Instance = this;

			titleKey = "Backwalls.STRINGS.UI.WALLSIDESCREEN.TITLE";

			Initialize();

			//shinyToggle = transform.Find("Contents/Toggles/ShinyToggle/Toggle").GetComponent<Toggle>();
			//bordersToggle = transform.Find("Contents/Toggles/BordersToggle/Toggle").GetComponent<Toggle>();
		}

		public override void ClearTarget()
		{
			base.ClearTarget();
			if (contextMenu != null)
				contextMenu.gameObject.SetActive(false);
		}

		private void Initialize()
		{
			if (hsvColorSelector != null)
				return;

			hsvColorSelector = transform.Find("Contents/ColorSelector").gameObject.AddOrGet<HSVColorSelector>();
			swatchSelector = transform.Find("Contents/ColorGrid").gameObject.AddOrGet<SwatchSelector>();
			patternSelector = transform.Find("Contents/FacadeSelector").gameObject.AddOrGet<PatternSelector>();

			showHSVToggle = transform.Find("Contents/ColorTabs/Sliders").gameObject.AddOrGet<TabToggle>();
			showSwatchToggle = transform.Find("Contents/ColorTabs/Swatches").gameObject.AddOrGet<TabToggle>();
			showHiddenToggle = transform.Find("Contents/ColorTabs/Hidden").gameObject.AddOrGet<HiddenToggle>();

			copyPatternToggle = transform.Find("Contents/CopyToggles/PatternToggle/Toggle").GetComponent<Toggle>();
			copyColorToggle = transform.Find("Contents/CopyToggles/ColorToggle/Toggle").GetComponent<Toggle>();

			noCopyWarning = transform.Find("Contents/CopyToggles/Warning").gameObject;
			setDefaultsButton = transform.Find("Contents/SetDefaultButton/Button").gameObject.AddOrGet<FButton>();

			addFavoriteButton = transform.Find("Contents/Title/Button").gameObject.AddComponent<FButton>();
			favoriteSelector = transform.Find("Contents/Favorites").gameObject.AddOrGet<FavoriteSelector>();
			favoriteSelector.gameObject.SetActive(false);

			contextMenu = Instantiate(ModAssets.contextMenuPrefab).AddComponent<ContextMenu>();
			contextMenu.SetObjects();
			contextMenu.transform.parent = GetComponentInParent<Canvas>().transform;
			contextMenu.gameObject.SetActive(true);
			contextMenu.AddOption("remove", "Remove", RemoveFavorite);
			contextMenu.AddOption("hide", "Hide", ToggleHide);
			contextMenu.AddOption("unhide", "Unhide", ToggleHide);
			contextMenu.AddOption("hideall", "Hide all with tag", ToggleHideAll);
			contextMenu.gameObject.SetActive(false);

			var toggles = transform.Find("Contents/Toggles");
			toggles.gameObject.SetActive(false);
		}

		private void ToggleHideAll()
		{
			if (patternSelector.lastActiveToggle != null)
				patternSelector.HideAllWithToggle(patternSelector.lastActiveToggle.pattern.BorderTag);

			contextMenu.gameObject.SetActive(false);
		}

		private void RemoveFavorite()
		{
			if (patternSelector.lastActiveToggle is FavoriteSelector.PresetToggle presetToggle)
				favoriteSelector.RemoveFavoriteToggle(presetToggle);

			contextMenu.gameObject.SetActive(false);
			favoriteSelector.UpdateHeight();
		}

		private void ToggleHide()
		{
			if (patternSelector.lastActiveToggle != null)
				patternSelector.lastActiveToggle.ToggleHidden();

			patternSelector.UpdateToggles();
			contextMenu.gameObject.SetActive(false);
		}

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out Backwall _);

		public override void SetTarget(GameObject target)
		{
			Instance = this;
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

		public override void OnSpawn()
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
			favoriteSelector.SetupVariantToggles();
			favoriteSelector.gameObject.SetActive(Mod.Settings.FavoritedPatterns.Count > 0);

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

			if (showHiddenToggle == null)
				Log.Warning("showHiddenToggle is null");

			showHiddenToggle.Setup();
			showHiddenToggle.onValueChanged.AddListener(on =>
			{
				isAllPatternsVisible = showHiddenToggle.isOn;

				if (patternSelector != null)
					patternSelector.UpdateToggles();

				showHiddenToggle.OnToggle(on);
			});

			showHiddenToggle.isOn = Mod.Settings.ShowHiddenToggles;
			//shinyToggle.onValueChanged.AddListener(OnShinyChanged);

			hsvColorSelector.OnChange += OnHSVColorChange;
			patternSelector.OnSetVariant += OnPatternChange;
			swatchSelector.OnChange += OnSwatchChange;

			noCopyWarning.SetActive(!copyColorToggle.isOn && !copyPatternToggle.isOn);

			addFavoriteButton.OnClick += AddCurrentAsFavorite;
			setDefaultsButton.OnClick += OnSetDefaults;
		}

		private void AddCurrentAsFavorite()
		{
			if (favoriteSelector == null)
			{
				Log.Warning("favoriteselector is null");
				return;
			}

			var color = hsvColorSelector.GetColor();
			var pattern = patternSelector.currentPattern ?? Mod.Settings.DefaultPattern;

			favoriteSelector.gameObject.SetActive(true);
			favoriteSelector.AddFavoriteToggle(pattern, hsvColorSelector.GetColor(), -1);

			Mod.Settings.FavoritedPatterns.Add(new Settings.Config.FavoritePreset()
			{
				Color = color.ToHexString(),
				Pattern = pattern
			});

			Mod.SaveSettings();

			favoriteSelector.UpdateHeight();
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
				return;

			target.SetColor(color);

			if (swatchSelector.isActiveAndEnabled)
				swatchSelector.SetSwatch(SwatchSelector.Invalid, false);
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
				return;

			target.SetColor(index);

			if (hsvColorSelector.isActiveAndEnabled)
				hsvColorSelector.SetColor(color, false);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public bool ShouldPreventRightClick() => patternSelector.ActiveToggle() != null;

		public void ShowContextMenu()
		{
			Initialize();

			var toggle = patternSelector.ActiveToggle();
			if (toggle != null)
			{
				contextMenu.transform.position = toggle.transform.position;

				if (toggle is FavoriteSelector.PresetToggle)
					contextMenu.SetOptions("remove");
				else
				{
					if (Mod.Settings.HiddenPatterns.Contains(toggle.pattern.ID))
						contextMenu.SetOptions("unhide");
					else
						contextMenu.SetOptions("hide", "hideall");
				}

				contextMenu.gameObject.SetActive(true);
			}
		}
		public class HiddenToggle : Toggle
		{
			private Image icon;
			private static Color offColor = new Color(0.7f, 0.7f, 0.7f);
			private static Color onColor = Color.black;


			public void Setup()
			{
				icon = GetComponent<Image>();
			}

			public void OnToggle(bool on)
			{
				icon.color = on ? onColor : offColor;
			}
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
