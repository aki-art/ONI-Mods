using Backwalls.Buildings;
using FUtility.FUI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls
{
    internal class BackwallSidescreen : SideScreenContent
    {
        private Backwall target;

        [SerializeField]
        private PatternToggle patternTogglePrefab;

        [SerializeField]
        private ToggleGroup patternToggleGroup;

        [SerializeField]
        private ColorToggle colorTogglePrefab;

        [SerializeField]
        private ToggleGroup colorToggleGroup;

        [SerializeField]
        private Toggle copyPatternToggle;

        [SerializeField]
        private Toggle copyColorToggle;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "Backwalls.STRINGS.UI.WALLSIDESCREEN.TITLE";

            var patterns = transform.Find("Contents/FacadeSelector/Scroll View/Viewport/Content");
            patternTogglePrefab = patterns.Find("TogglePrefab").gameObject.AddComponent<PatternToggle>();
            patternToggleGroup = patterns.FindOrAddComponent<ToggleGroup>();

            var colors = transform.Find("Contents/ColorGrid");
            colorTogglePrefab = colors.Find("SwatchPrefab").gameObject.AddComponent<ColorToggle>();
            colorToggleGroup = colors.FindOrAddComponent<ToggleGroup>();

            copyPatternToggle = transform.Find("Contents/CopyToggles/PatternToggle/Toggle").GetComponent<Toggle>();
            copyColorToggle = transform.Find("Contents/CopyToggles/ColorToggle/Toggle").GetComponent<Toggle>();
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<Backwall>() != null;
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);
            this.target = target.GetComponent<Backwall>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            gameObject.SetActive(true);
            RefreshUI();
        }

        private void RefreshUI()
        {
            SetupVariantToggles();
            SetupColorToggles();

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
        }

        private void SetupColorToggles()
        {
            foreach (Transform toggle in colorToggleGroup.transform)
            {
                //Destroy(toggle);
            }

            for (var i = 0; i < ModAssets.colors.Length; i++)
            {
                var color = ModAssets.colors[i];
                var toggle = Instantiate(colorTogglePrefab, colorToggleGroup.transform);
                toggle.Setup(i);
                toggle.group = colorToggleGroup;
                colorToggleGroup.RegisterToggle(toggle);
                toggle.onValueChanged.AddListener(value => toggle.OnToggle(value, target));
                toggle.gameObject.SetActive(true);
            }
        }

        private void SetupVariantToggles()
        {
            Mod.variants = Mod.variants
                .OrderBy(v => v.sortOrder)
                .ThenBy(v => v.name)
                .ToList();

            foreach (Transform toggle in patternToggleGroup.transform)
            {
                //Destroy(toggle);
            }

            foreach (var variant in Mod.variants)
            {
                var toggle = Instantiate(patternTogglePrefab, patternToggleGroup.transform);
                toggle.transform.Find("Image").GetComponent<Image>().sprite = variant.UISprite;
                toggle.gameObject.SetActive(true);
                toggle.Setup(variant);
                toggle.group = patternToggleGroup;
                toggle.onValueChanged.AddListener(value => toggle.OnToggle(value, target));

                patternToggleGroup.RegisterToggle(toggle);

                Helper.AddSimpleToolTip(toggle.gameObject, variant.name);
            }
        }

        public class PatternToggle : Toggle
        {
            public BackwallPattern pattern;
            private Image bg;
            private static Color defaultColor = new Color32(62, 67, 87, 255);
            private static Color selectedColor = new Color32(129, 138, 179, 255);

            public void Setup(BackwallPattern pattern)
            {
                this.pattern = pattern;
                bg = GetComponent<Image>();
            }

            public void OnToggle(bool on, Backwall target)
            {
                if (on)
                {
                    target.SetPattern(pattern);
                    PlaySound(UISoundHelper.Click);
                    bg.color = selectedColor;
                }
                else
                {
                    bg.color = defaultColor;
                }
            }
        }

        public class ColorToggle : Toggle
        {
            public int colorIdx;
            private Outline outline;

            public void Setup(int colorIdx)
            {
                this.colorIdx = colorIdx;
                outline = GetComponent<Outline>();
                GetComponent<Image>().color = ModAssets.colors[colorIdx];
            }

            public void OnToggle(bool on, Backwall target)
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

                target.SetColor(colorIdx);
            }
        }
    }
}
