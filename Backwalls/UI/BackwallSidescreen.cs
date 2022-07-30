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

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "Backwalls.STRINGS.UI.WALLSIDESCREEN.TITLE";

            hsvColorSelector = transform.Find("Contents/ColorSelector").gameObject.AddComponent<HSVColorSelector>();
            swatchSelector = transform.Find("Contents/ColorGrid").gameObject.AddComponent<SwatchSelector>();
            patternSelector = transform.Find("Contents/FacadeSelector").gameObject.AddComponent<PatternSelector>();

            showHSVToggle = transform.Find("Contents/ColorTabs/Sliders").gameObject.AddComponent<TabToggle>();
            showSwatchToggle = transform.Find("Contents/ColorTabs/Swatches").gameObject.AddComponent<TabToggle>();
            
            copyPatternToggle = transform.Find("Contents/CopyToggles/PatternToggle/Toggle").GetComponent<Toggle>();
            copyColorToggle = transform.Find("Contents/CopyToggles/ColorToggle/Toggle").GetComponent<Toggle>();

            "ssads".IsNullOrWhiteSpace();
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

            copyColorToggle.isOn = Mod.Settings.CopyColor;
            copyColorToggle.onValueChanged.AddListener(on =>
            {
                Mod.Settings.CopyColor = on;
                Mod.SaveSettings();
            });

            copyPatternToggle.isOn = Mod.Settings.CopyPattern;
            copyPatternToggle.onValueChanged.AddListener(on =>
            {
                Mod.Settings.CopyPattern = on;
                Mod.SaveSettings();
            });

            Log.Assert("showSwatchToggle", showSwatchToggle);
            Log.Assert("showHSVToggle", showHSVToggle);

            showHSVToggle.Setup(hsvColorSelector.gameObject);
            showSwatchToggle.Setup(swatchSelector.gameObject);

            showHSVToggle.onValueChanged.AddListener(on =>
            {
                Mod.Settings.ShowHSVSliders = on;
                Mod.SaveSettings();

                //hsvColorSelector.gameObject.SetActive(on);
                showHSVToggle.OnToggle(on);
            });
            showHSVToggle.isOn = Mod.Settings.ShowHSVSliders;

            showSwatchToggle.onValueChanged.AddListener(on =>
            {
                Mod.Settings.ShowColorSwatches = on;
                Mod.SaveSettings();

                //swatchSelector.gameObject.SetActive(on);
                showSwatchToggle.OnToggle(on);
            });
            showSwatchToggle.isOn = Mod.Settings.ShowColorSwatches;

            hsvColorSelector.OnChange += OnHSVColorChange;
            patternSelector.OnSetVariant += OnPatternChange;
            swatchSelector.OnChange += OnSwatchChange;
        }

        private void OnHSVColorChange(Color color)
        {
            target.SetColor(color);
            if(swatchSelector.isActiveAndEnabled)
            {
                swatchSelector.SetSwatch(SwatchSelector.Invalid, false);
            }
        }

        private void OnPatternChange(BackwallPattern pattern)
        {
            target.SetPattern(pattern);
        }

        private void OnSwatchChange(Color color, int index)
        {
            target.SetColor(index);

            if(hsvColorSelector.isActiveAndEnabled)
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
            private static Color offColor = new Color();
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
