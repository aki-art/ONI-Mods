using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuxSensor
{
    public class SliderTest : KMonoBehaviour, IDualSliderControl
    {
        float[] someValue = new float[2];
        public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";

        public string SliderUnits => "unit";

        public float GetSliderMax(int index) => 3000;

        public float GetSliderMin(int index) => 0;

        public string GetSliderTooltip() => "tooltip";

        public string GetSliderTooltipKey(int index) => "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";

        public float GetSliderValue(int index) => someValue[index];

        public void SetSliderValue(float percent, int index) => someValue[index] = percent;

        public int SliderDecimalPlaces(int index) => 0;
    }
}
