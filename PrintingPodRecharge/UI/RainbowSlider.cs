using FUtility;
using FUtility.FUI;
using UnityEngine;
using UnityEngine.UI;

namespace PrintingPodRecharge.UI
{
    class RainbowSlider : FSlider
    {
        const float MAX = 1f;

        private LocText label;
        private Sprite rainbowSprite;
        private Image backgroundImage;
        private bool isMax;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            backgroundImage = transform.Find("Fill Area/Fill").GetComponent<Image>();
            label = transform.Find("RangeLabel").gameObject.GetComponent<LocText>();

            rainbowSprite = backgroundImage.sprite;

            backgroundImage.sprite = null;
            backgroundImage.color = CONSTS.COLORS.KLEI_PINK;

            OnChange += UpdateRanges;
        }

        public void UpdateRanges()
        {
            var percent = GameUtil.GetFormattedPercent(RoundTo05(Value * 100f));
            label.SetText(percent);

            if (Value == MAX && !isMax)
            {
                backgroundImage.sprite = rainbowSprite;
                backgroundImage.color = Color.white;
            }
            else if (Value < MAX && isMax)
            {
                backgroundImage.sprite = null;
                backgroundImage.color = CONSTS.COLORS.KLEI_PINK;
            }

            isMax = Value == MAX;
        }

        public new float Value
        {
            get => slider.value;
            set
            {
                slider.value = value;
                UpdateRanges();
            }
        }

        public float GetRoundedValue() => RoundTo05(Value * 100f) / 100f;

        public static float RoundTo05(float input) => Mathf.Round(input * 20) / 20;
    }
}
