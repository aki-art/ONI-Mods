using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using static Asphalt.STRINGS.UI.SETTINGSDIALOG.SLIDERPANEL;

namespace Asphalt.Settings
{
    class PlaidSlider : FSlider
    {
        const float MAX = 20f;

        private List<Range> ranges;
        private LocText rangeLabel;
        private LocText speedLabel;
        private Sprite plaidSprite;
        private Image backgroundImage;
        private bool isMax;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            backgroundImage = transform.Find("Fill Area/Fill").GetComponent<Image>();
            rangeLabel = transform.Find("RangeLabel").gameObject.GetComponent<LocText>();
            speedLabel = transform.Find("Label").gameObject.GetComponent<LocText>();

            plaidSprite = backgroundImage.sprite;

            backgroundImage.sprite = null;
            backgroundImage.color = CONSTS.COLORS.KLEI_PINK;

            ranges = new List<Range> {
                new Range(1f,  RANGES.TIER1_NOBONUS, Color.grey),
                new Range(1.05f, RANGES.TIER2_SMALLBONUS, Color.grey),
                new Range(1.25f, RANGES.TIER3_REGULARTILE, Color.white),
                new Range(1.3f, RANGES.TIER4_SOMEBONUS, Color.white),
                new Range(1.5f, RANGES.TIER5_METALTILE, Color.white),
                new Range(1.55f, RANGES.TIER6_FAST, Color.white),
                new Range(2f,RANGES.TIER7_DEFAULT, Color.white),
                new Range(2.05f, RANGES.TIER8_GOFAST, new Color32(55, 168, 255, 255)),
                new Range(3f, RANGES.TIER9_LIGHTSPEED, new Color32(183, 226, 13, 255)),
                new Range(5f, RANGES.TIER10_RIDICULOUS, new Color32(226, 93, 13, 255)),
                new Range(20f, RANGES.TIER11_LUDICROUS, new Color32(226, 23, 13, 255))
            };

            OnChange += UpdateRanges;
        }

        public void UpdateRanges()
        {
            speedLabel.SetText(SLIDER.LABEL.Replace("{number}", Value.ToString("F2", CultureInfo.InvariantCulture)));

            int currentIndex = ranges.FindLastIndex(r => r.min <= Value);
            var currentRange = ranges[currentIndex];

            if (currentRange.name != null)
            {
                rangeLabel.text = SLIDER.RANGELABEL.text.Replace("{label}", currentRange.name);
                rangeLabel.color = currentRange.color;
            }

            if (Value == 20f && !isMax)
            {
                backgroundImage.sprite = plaidSprite;
                backgroundImage.color = Color.white;
            }
            else if (Value < 20f && isMax)
            {
                backgroundImage.sprite = null;
                backgroundImage.color = CONSTS.COLORS.KLEI_PINK;
            }

            isMax = Value == 20f;
        }

        public new float Value
        {
            get => FormatSpeed1to20(slider.value);
            set
            {
                slider.value = FormatSpeed20to1(value);
                UpdateRanges();
            }
        }

        const float n = 4.565703f;

        public float FormatSpeed1to20(float input)
        {

            float result = input <= .66f ? 3f * input + 1 : MAX * Mathf.Pow(input, n);

            result = Mathf.Clamp(result, 1f, MAX);
            result = RoundTo05(result);

            return result;
        }

        public float FormatSpeed20to1(float val)
        {
            return val < 3f ? (val - 1) / 3f : Mathf.Pow(val / MAX, 1f / n);
        }

        public static float RoundTo05(float input) => Mathf.Round(input * 20) / 20;

        [Serializable]
        public struct Range
        {
            public float min;
            public string name;
            public Color color;

            public Range(float minimum, string rangeName, Color rangeColor)
            {
                min = minimum;
                name = rangeName;
                color = rangeColor;
            }
        }
    }
}
