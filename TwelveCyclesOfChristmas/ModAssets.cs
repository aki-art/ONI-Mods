using UnityEngine;

namespace TwelveCyclesOfChristmas
{
    class ModAssets
    {
        public static Texture2D dog;
        public static Material dogMaterial;

        public static void LoadAssets()
        {
            dog = FUtility.Assets.LoadTexture("dog");

            //dogMaterial = new Material(Shader.Find("Klei/TiledBlock"));
            dogMaterial = new Material(Assets.GetMaterial("tiles_solid"));
            //dogMaterial.SetInt("__dst", 1);
        }
    }
}
