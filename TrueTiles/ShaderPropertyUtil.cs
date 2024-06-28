using UnityEngine;

namespace TrueTiles
{
	public class ShaderPropertyUtil
	{
		public static void SetMainTexProperty(Material material, Texture2D texture)
		{
			if (texture != null && material != null)
				material.SetTexture("_MainTex", texture);
		}

		public static void SetSpecularProperties(Material material, Texture2D texture, float frequency, Color color)
		{
			if (material == null)
				return;

			if (texture != null)
			{
				material.SetTexture("_SpecularTex", texture);
				material.EnableKeyword("ENABLE_SHINE");
			}

			material.SetFloat("_Frequency", frequency);

			if (color != null)
				material.SetColor("_ShineColour", color);
		}
	}
}
