using FUtility;
using UnityEngine;

namespace AETNTweaks
{
    internal class ModAssets
    {
        public static Material tetherMaterial;

        public static void LateLoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("aetntweaks");
            var tetherTex = bundle.LoadAsset<Texture2D>("tether base");

            tetherMaterial = new Material(Shader.Find("Sprites/Default"));
            tetherMaterial.SetTexture("_MainTex", tetherTex);
        }
    }
}
