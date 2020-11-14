using FUtility.FUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asphalt.Settings
{
    public class FPlaidSlider : FSlider
    {
        const float MAX = 20f;
        private LocText label;
        private LocText speedLabel;
        public List<Range> ranges;
        Sprite plaidSprite;
        Image backgroundImage;
        Color weirdPink;

        public void Initialize(string label, string speedLabel, float value, List<Range> ranges)
        {
            slider = gameObject.GetComponent<Slider>();
            Value = value;
            this.label = transform.parent.Find(label).gameObject.GetComponent<LocText>();
            this.speedLabel = transform.parent.Find(speedLabel).gameObject.GetComponent<LocText>();
            this.ranges = ranges;
        }

        protected override void OnPrefabInit()
        {
            backgroundImage = transform.Find("Fill Area/Fill").GetComponent<Image>();
            plaidSprite = backgroundImage.sprite;
            weirdPink = backgroundImage.color;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            OnChange += UpdateLabels;
            backgroundImage.color = weirdPink;
            backgroundImage.sprite = null;

            UpdateLabels();
        }

        public void UpdateLabels()
        {
            label.text = GetFormattedSpeed();
            if (Value < MAX)
            {
                backgroundImage.color = weirdPink;
                backgroundImage.sprite = null;
            }
            else
            {
                backgroundImage.color = Color.white;
                backgroundImage.sprite = plaidSprite;
            }

            UpdateRange();
        }

        private string GetFormattedSpeed() => STRINGS.UI.ASPHALTSETTINGSDIALOG.CONTENT.SLIDERPANEL.LABEL.text.Replace("{number}", (Value * 100f).ToString());

        private void UpdateRange()
        {
            if (ranges != null && ranges.Count > 0)
            {
                int currentIndex = ranges.FindLastIndex(r => r.min <= Value);
                var currentRange = ranges[currentIndex];

                if (currentRange.name != null)
                {
                    speedLabel.text = STRINGS.UI.ASPHALTSETTINGSDIALOG.CONTENT.SLIDERPANEL.SPEEDLABEL.text.Replace("{label}", currentRange.name);
                    speedLabel.color = currentRange.color;
                }
            }
        }

        new public float Value
        {
            get => Tuning.FormatSpeed(slider.value);
            set => slider.value = value;
        }

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
