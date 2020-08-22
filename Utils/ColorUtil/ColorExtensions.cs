using UnityEngine;

namespace FUtility.ColorUtil
{
    public static class ColorExtensions
    {
        public static HSVColor HSV(this Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return new HSVColor(h, s, v, color.a);
        }
    }
}
