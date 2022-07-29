using FUtility.FUI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls.UI
{
    public class SwatchSelector : KMonoBehaviour
    {
        public const int Invalid = -1;

        [SerializeField]
        private ColorToggle togglePrefab;

        [SerializeField]
        private ToggleGroup toggleGroup;

        private ColorToggle[] toggles;

        [SerializeField]
        public Action<Color, int> OnChange;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            togglePrefab = transform.Find("SwatchPrefab").gameObject.AddComponent<ColorToggle>();
            toggleGroup = transform.FindOrAddComponent<ToggleGroup>();
            toggleGroup.allowSwitchOff = true;
        }

        internal void SetSwatch(int index, bool triggerUpdate)
        {
            DeselectAll();
            if (index > 0 && index < toggles.Length)
            {
                var toggle = toggles[index];

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

        public void DeselectAll()
        {
            toggleGroup.SetAllTogglesOff();
        }

        public void Setup()
        {
            SetupColorToggles();
        }

        private void SetupColorToggles()
        {
            toggles = new ColorToggle[ModAssets.colors.Length];

            for (var i = 0; i < ModAssets.colors.Length; i++)
            {
                var color = ModAssets.colors[i];
                var toggle = Instantiate(togglePrefab, toggleGroup.transform);
                toggle.Setup(i);
                toggle.group = toggleGroup;
                toggleGroup.RegisterToggle(toggle);
                toggle.onValueChanged.AddListener(value =>
                {
                    toggle.OnToggle(value);
                    if(value)
                    {
                        OnChange?.Invoke(color, toggle.swatchIdx);
                    }
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
                this.swatchIdx = color;
                outline = GetComponent<Outline>();
                GetComponent<Image>().color = ModAssets.colors[color];
            }

            public void OnToggle(bool on)
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
