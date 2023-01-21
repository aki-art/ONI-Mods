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

            hsvColorSelector = transform.Find("Contents/ColorSelector").FindOrAddComponent<HSVColorSelector>();
            swatchSelector = transform.Find("Contents/ColorGrid").FindOrAddComponent<SwatchSelector>();
            patternSelector = transform.Find("Contents/FacadeSelector").FindOrAddComponent<PatternSelector>();

            showHSVToggle = transform.Find("Contents/ColorTabs/Sliders").FindOrAddComponent<TabToggle>();
            showSwatchToggle = transform.Find("Contents/ColorTabs/Swatches").FindOrAddComponent<TabToggle>();

            copyPatternToggle = transform.Find("Contents/CopyToggles/PatternToggle/Toggle").GetComponent<Toggle>();
            copyColorToggle = transform.Find("Contents/CopyToggles/ColorToggle/Toggle").GetComponent<Toggle>();

            noCopyWarning = transform.Find("Contents/CopyToggles/Warning").gameObject;
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<Backwall>() != null;
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);

            this.target = target.GetComponent<Backwall>();

            if (this.target == null)
            {
                Log.Warning("BackwallSideScreen: target null.");
                return;
            }

            if (this.target.swatchIdx != SwatchSelector.Invalid)
            {
                swatchSelector.SetSwatch(this.target.swatchIdx, false);
            }
            else
            {
                hsvColorSelector.SetColor(Util.ColorFromHex(this.target.colorHex), false);
            }


            patternSelector.SetPattern(this.target.pattern);
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
            if(target == null)
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
            {
                return;
            }

            target.SetPattern(pattern);
        }

        private void OnSwatchChange(Color color, int index)
        {
            if (target == null)
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
