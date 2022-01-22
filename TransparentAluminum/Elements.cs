using FUtility;
using UnityEngine;
using static TransparentAluminum.ModAssets;

namespace TransparentAluminum
{
    public class Elements
    {
        public static SimHashes TransparentAluminum = (SimHashes)Hash.SDBMLower("TransparentAluminum");

        public static Substance CreateTransparentAluminumSubstance(Material solidMaterial)
        {
            KAnimFile animFile = Assets.Anims.Find(anim => anim.name == "glass_kanim");

            if (animFile is null || Textures.Alon.diffuse is null || Textures.Alon.specular is null)
            {
                Log.Warning("Not all graphics files were found for Transparent Aluminum, using Aluminum's substance instead.");
                return ElementLoader.GetElement(SimHashes.Aluminum.CreateTag()).substance;
            }

            Material material = GetAlonMaterial(solidMaterial);

            return ModUtil.CreateSubstance("TransparentAluminum", Element.State.Solid, animFile, material, Colors.alon, Colors.alonOpaque, Colors.alonOpaque);
        }

        private static Material GetAlonMaterial(Material solidMaterial)
        {
            Material material = Object.Instantiate(solidMaterial);
            material.mainTexture = Textures.Alon.diffuse;
            material.SetTexture("_ShineMask", Textures.Alon.specular);

            return material;
        }
    }
}
