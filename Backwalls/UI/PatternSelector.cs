using Backwalls.Buildings;
using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
    internal class PatternSelector : KMonoBehaviour
    {
        [SerializeField]
        private PatternToggle patternTogglePrefab;

        [SerializeField]
        private ToggleGroup patternToggleGroup;

        public Action<BackwallPattern> OnSetVariant;

        private Dictionary<string, PatternToggle> patternToggles = new Dictionary<string, PatternToggle>();

        protected override void OnPrefabInit()
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
                if(triggerUpdate)
                {
                    toggle.isOn = true;
                }
                else
                {
                    toggle.SetIsOnWithoutNotify(true);
                }
            }
        }

        public void SetupVariantToggles()
        {
            var sortedVariants = Mod.variants
                .Values
                .OrderBy(v => v.sortOrder)
                .ThenBy(v => v.name)
                .ToList();

            foreach (Transform toggle in patternToggleGroup.transform)
            {
                //Destroy(toggle);
            }

            foreach (var variant in sortedVariants)
            {
                if (patternToggles.ContainsKey(variant.ID))
                {
                    continue;
                }

                var toggle = Instantiate(patternTogglePrefab, patternToggleGroup.transform);
                toggle.transform.Find("Image").GetComponent<Image>().sprite = variant.UISprite;
                toggle.gameObject.SetActive(true);
                toggle.Setup(variant);
                toggle.group = patternToggleGroup;
                toggle.onValueChanged.AddListener(value =>
                {
                    if (value)
                    {
                        OnSetVariant?.Invoke(toggle.pattern);
                    }

                    toggle.UpdateState(value);
                });

                patternToggleGroup.RegisterToggle(toggle);
                patternToggles.Add(variant.ID, toggle);

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

            public void UpdateState(bool on)
            {
                if (on)
                {
                    PlaySound(CONSTS.UI_SOUNDS_EVENTS.CLICK);
                    bg.color = selectedColor;
                }
                else
                {
                    bg.color = defaultColor;
                }
            }
        }
    }
}
