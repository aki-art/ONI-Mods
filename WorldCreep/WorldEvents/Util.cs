using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class Util
    {
        // Equation based on shadertoy.com/view/Xd2yRd & Sebastian Lague
        public static float Bias(float x, float bias)
        {
            float k = Mathf.Pow(1 - bias, 3);
            return x * k / (x * k - x + 1);
        }

        public static float GetClampedGaussian(float stdDev, float mean)
        {
            return Mathf.Clamp(global::Util.GaussianRandom() * stdDev / 3f + mean, -stdDev, stdDev);
        }

        public static float GetClampedAssymetricGaussian(float stvDev, float mean)
        {
            return Mathf.Abs(GetClampedGaussian(stvDev, mean));
        }
    }
}
