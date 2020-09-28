using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entropea
{
    public static class Extensions
    {
        public static T GetGaussianRandom<T>(this IEnumerable<T> source, float mean, float standardDeviation)
        {
            if (source == null || source.Count() == 0)
                return default;

            int index = Mathf.FloorToInt(Util.GaussianRandom(mean, standardDeviation));
            return source.ElementAt(index);
        }

        public static bool CoinFlip(this SeededRandom seededRandom)
        {
            return seededRandom.RandomValue() >= 0.5f;
        }

        public static Color GetAverageColor(this Texture2D tex)
        {
            Color32[] texColors = tex.GetPixels32();
            int total = texColors.Length;
            var r = 0;
            var g = 0;
            var b = 0;
            for (var i = 0; i < total; i++)
            {
                r += texColors[i].r;
                g += texColors[i].g;
                b += texColors[i].b;
            }

            return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 1);
        }
    }
}
