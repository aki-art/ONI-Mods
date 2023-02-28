using System.Collections.Generic;
using UnityEngine;

namespace Buildings.StainedGlassTile
{
    public class DecorPackAGlassShineColors : KMonoBehaviour
    {
        public Gradient rainbowGradient;
        public static Material neutroniumAlloyMaterial;

        public override void OnPrefabInit()
        {
            rainbowGradient = new Gradient();

            var colors = new List<Color>();

            for(int i = 0; i < 6; i++)
            {
                colors.Add(Color.HSVToRGB(1f / i, 1f, 1f) * 4f);
            }

            var colorKey = new GradientColorKey[colors.Count];

            for (var i = 0; i < colors.Count; i++)
            {
                colorKey[i] = new GradientColorKey(colors[i], (i + 1f) / colors.Count);
            }

            var alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;

            rainbowGradient.SetKeys(colorKey, alphaKey);
        }

        private void Update()
        {
            if(neutroniumAlloyMaterial == null)
            {
                return;
            }

            var camera = Camera.main.transform.position;
            var scale = CameraController.Instance.zoomFactor;
            var t = (Mathf.Cos(camera.x / scale) + Mathf.Sin(camera.y / scale)) / 4f + 0.5f;
            var rainbowColor = rainbowGradient.Evaluate(t);

            neutroniumAlloyMaterial.SetColor("_ShineColour", rainbowColor);
        }
    }
}
