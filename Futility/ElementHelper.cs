using UnityEngine;

namespace FUtility
{
    public class ElementHelper
    {
        public static Substance CreateSubstance(
         Material material,
         string name,
         string animName,
         Element.State state,
         Color color,
         bool shiny = false,
         int worldUVScale = 5,
         float frequency = 1,
         Color? colourTint = null,
         Color? shineColour = null)
        {
            KAnimFile animFile = global::Assets.Anims.Find(anim => anim.name == animName);
            var substanceTexture = Assets.LoadTexture(name.ToLower());

            if (substanceTexture == null)
            {
                Log.Warning($"Texture file not found for {name}.");
                return null;
            }

            Material mat = Object.Instantiate(material);
            mat.mainTexture = substanceTexture;
            mat.SetColor("_ColourTint", colourTint == null ? Color.white : (Color)colourTint);
            mat.SetFloat("_Frequency", frequency);
            mat.SetInt("_WorldUVScale", Mathf.Clamp(worldUVScale, 0, 100));

            if (shiny)
            {
                var normalMap = Assets.LoadTexture(name.ToLower() + "_normal");
                var mask = Assets.LoadTexture(name.ToLower() + "_mask");

                if (normalMap == null || mask == null)
                {
                    Log.Warning($"Substance was marked shiny, but texture file was not found for {name}.");
                }
                else
                {
                    mat.SetTexture("_NormalNoise", normalMap);
                    mat.SetTexture("_ShineMask", mask);
                    mat.SetColor("_ShineColour", shineColour == null ? Color.white : (Color)shineColour);
                }
            }

            return ModUtil.CreateSubstance(name, state, animFile, mat, color, color, color);
        }
    }
}
