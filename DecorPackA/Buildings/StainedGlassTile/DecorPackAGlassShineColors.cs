using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
	public class DecorPackAGlassShineColors : KMonoBehaviour
	{
		public Gradient rainbowGradient;
		public static Color oil1 = new Color(2.45f, 0.71f, 2.18f) * 0.5f;
		public static Color oil2 = new Color(0.31f, 1.60f, 2.34f) * 0.5f;
		public static Material neutroniumAlloyMaterial;
		public static Material oilMaterial;

		public override void OnPrefabInit()
		{
			rainbowGradient = new Gradient();

			var colors = new List<Color>();

			for (int i = 0; i < 6; i++)
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
			if (neutroniumAlloyMaterial == null && oilMaterial == null)
			{
				return;
			}

			var camera = Camera.main.transform.position;
			var scale = CameraController.Instance.zoomFactor;
			var t = (Mathf.Cos(camera.x / scale) + Mathf.Sin(camera.y / scale)) / 4f + 0.5f;

			if (neutroniumAlloyMaterial != null)
			{
				var rainbowColor = rainbowGradient.Evaluate(t);
				neutroniumAlloyMaterial.SetColor("_ShineColour", rainbowColor);
			}

			oilMaterial?.SetColor("_ShineColour", Color.LerpUnclamped(oil1, oil2, t));
		}
	}
}
