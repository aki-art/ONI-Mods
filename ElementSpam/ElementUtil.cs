using UnityEngine;

namespace ElementSpam
{
    internal class ElementUtil
    {
        public static Substance CreateSubstance(SimHashes id, string uiAnim, Element.State state, Color color)
        {
            return CreateSubstance(id, uiAnim, state, color, color, color);
        }

        public static Substance CreateSubstance(SimHashes id, string uiAnim, Element.State state, Color color, Color uiColor, Color conduitColor)
        {
            var animFile = Assets.Anims.Find(anim => anim.name == uiAnim);
            var material = GetMaterialForState(state);

            return ModUtil.CreateSubstance(id.ToString(), state, animFile, material, color, uiColor, conduitColor);
        }

        private static Material GetMaterialForState(Element.State state)
        {
            // (gases use liquid material)
            var material = state == Element.State.Solid ? Assets.instance.substanceTable.solidMaterial : Assets.instance.substanceTable.liquidMaterial;
            return new Material(material);
        }

        public static Material SetTextures(SimHashes id, Material newMaterial, string folder, string texture, string spec = null)
        {
            var substance = ElementLoader.FindElementByHash(id).substance;
            var tex = FUtility.Assets.LoadTexture(texture, folder);

            if (newMaterial != null)
            {
                substance.material = new Material(newMaterial);
            }

            substance.material.mainTexture = tex;

            if (!spec.IsNullOrWhiteSpace())
            {
                var specTex = FUtility.Assets.LoadTexture(spec, folder);
                substance.material.SetTexture("_ShineMask", specTex);
            }

            return substance.material;
        }
    }
}
