using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asphalt
{
    public class FSpeedSlider : KMonoBehaviour, IEventSystemHandler
    {
        private const string SPEED_MULTIPLIER_PREFIX = "Speed bonus: ";

        private Text speedMultiplerLabel;
        private Text speedRangeLabel;

        public List<Range> ranges;

        private FSlider fSlider;
        Sprite plaidSprite;
        Image backgroundImage;
        Color weirdPink;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            #region  set object references
            fSlider = gameObject.AddComponent<FSlider>();
            speedMultiplerLabel = transform.Find("SliderLabel").GetComponent<Text>();
            speedRangeLabel = transform.Find("SliderRangeLabel").GetComponent<Text>();
            backgroundImage = transform.Find("Fill Area/Fill").GetComponent<Image>();
            plaidSprite = backgroundImage.sprite;
            weirdPink = backgroundImage.color;
            #endregion

            fSlider.OnChange += UpdateLabels;
            backgroundImage.color = weirdPink;
            backgroundImage.sprite = null;
            UpdateLabels();
        }

        public void AssignRanges(List<Range> rangeList)
        {
            
            ranges = rangeList;
            UpdateLabels();
        }

        public void SetValue(float val)
        {
            fSlider.Value = val;
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            float val = MapValue();
            speedMultiplerLabel.text = SPEED_MULTIPLIER_PREFIX + val.ToString() + "x";

            if (fSlider.slider.value < fSlider.slider.maxValue)
            {
                backgroundImage.color = weirdPink;
                backgroundImage.sprite = null;
            }
            else
            {
                backgroundImage.color = Color.white;
                backgroundImage.sprite = plaidSprite;
            }

            UpdateRange(val);

        }

        private void UpdateRange(float val)
        {
            if (ranges != null && ranges.Count > 0)
            {
                int currentIndex = ranges.FindLastIndex(r => r.min <= val);
                var currentRange = ranges[currentIndex];
                
                if(currentRange.name != null)
                {
                    speedRangeLabel.text = currentRange.name;
                    speedRangeLabel.color = currentRange.color;
                }
            }
        }

        public static float MapValue(float val)
        {
            float actualValue;
            const float maxValue = 20f;
            const float e = 4.565703f;

            if (val < .66f) 
                actualValue = 3f * val + 1; // linear scale from 1-2.95 (rounded)
            else if (val == 0.66f)
                actualValue = 3f; // static value
            else
                actualValue = (float)(maxValue * Math.Pow(val, e)); // exponential scale 3.05 - 20 (rounded)

            actualValue = Mathf.Clamp(actualValue, 1f, maxValue);
            actualValue = (float)Math.Round(actualValue * 20) / 20; // rounding to .05

            return actualValue;
        }

        private float MapValue()
        {
            float val = fSlider.Value;
            return MapValue(val);
        }

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
