using UnityEngine;

namespace AETNTweaks
{
    internal class ModAssets
    {
        public static Material tetherMaterial;
        public static Material tetherPlaceMaterial;

        public static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("aetntweaks");

            Texture2D tetherTex = bundle.LoadAsset<Texture2D>("tether base");

            Texture2D tetherPlaceTex = FUtility.Assets.LoadTexture("tether_place", null);
            tetherPlaceTex.wrapMode = TextureWrapMode.Repeat;
            tetherPlaceTex.Apply();

            tetherMaterial = new Material(Shader.Find("Sprites/Default"));
            tetherMaterial.SetTexture("_MainTex", tetherTex);

            tetherPlaceMaterial = new Material(tetherMaterial);
            tetherPlaceMaterial.SetTexture("_MainTex", tetherPlaceTex);
        }
    }
}
