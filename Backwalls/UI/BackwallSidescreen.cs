using Backwalls.Buildings;
using Backwalls.Cmps;
using FUtility;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
    public class BackwallSidescreen : SideScreenContent
    {
        private Backwall target;

        [SerializeField]
        private HSVColorSelector hsvColorSelector;

        [SerializeField]
        private PatternSelector patternSelector;

        [SerializeField]
        private SwatchSelector swatchSelector;

        [SerializeField]
        private Toggle copyPatternToggle;

        [SerializeField]
        private Toggle copyColorToggle;

        [SerializeField]
        private TabToggle showHSVToggle;

        [SerializeField]
        private TabToggle showSwatchToggle;

        [SerializeField]
        private GameObject noCopyWarning;

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
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return target.TryGetComponent(out Backwall _);
        }

        public override void SetTarget(GameObject target)
        {
            if (target != null && target.TryGetComponent(out Backwall newTarget))
            {
                if (newTarget.swatchIdx != SwatchSelector.Invalid)
                {
                    swatchSelector.SetSwatch(newTarget.swatchIdx, false);
                }
                else
                {
                    hsvColorSelector.SetColor(Util.ColorFromHex(newTarget.colorHex), false);
                }

                patternSelector.SetPattern(newTarget.pattern, false);

                this.target = newTarget;
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            gameObject.SetActive(true);
            RefreshUI();
        }

        private void RefreshUI()
        {
            patternSelector.SetupVariantToggles();
            swatchSelector.Setup();

            copyColorToggle.isOn = ModStorage.Instance.CopyColor;
            copyColorToggle.onValueChanged.AddListener(on =>
            {
                noCopyWarning.SetActive(!copyColorToggle.isOn && !copyPatternToggle.isOn);
                ModStorage.Instance.CopyColor = on;
                Mod.SaveSettings();
            });

            copyPatternToggle.isOn = ModStorage.Instance.CopyPattern;
            copyPatternToggle.onValueChanged.AddListener(on =>
            {
                noCopyWarning.SetActive(!copyColorToggle.isOn && !copyPatternToggle.isOn);
                ModStorage.Instance.CopyPattern = on;
                Mod.SaveSettings();
            });

            showHSVToggle.Setup(hsvColorSelector.gameObject);
            showSwatchToggle.Setup(swatchSelector.gameObject);

            showHSVToggle.onValueChanged.AddListener(on =>
            {
                ModStorage.Instance.ShowHSV = on;
                Mod.SaveSettings();

                showHSVToggle.OnToggle(on);
            });
            showHSVToggle.isOn = ModStorage.Instance.ShowHSV;

            showSwatchToggle.onValueChanged.AddListener(on =>
            {
                ModStorage.Instance.ShowSwatches = on;
                Mod.SaveSettings();

                showSwatchToggle.OnToggle(on);
            });

            showSwatchToggle.isOn = ModStorage.Instance.ShowSwatches;

            hsvColorSelector.OnChange += OnHSVColorChange;
            patternSelector.OnSetVariant += OnPatternChange;
            swatchSelector.OnChange += OnSwatchChange;

            noCopyWarning.SetActive(!copyColorToggle.isOn && !copyPatternToggle.isOn);
        }

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
            Log.Debug("pattern changed");

            if (target == null)
            {
                return;
            }

            target.SetPattern(pattern);
        }

        private void OnSwatchChange(Color color, int index)
        {
            Log.Debug("on swatch change");

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

            [SerializeField]
            public GameObject target;

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
