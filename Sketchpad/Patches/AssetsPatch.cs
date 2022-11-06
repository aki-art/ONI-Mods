using HarmonyLib;

namespace SketchPad.Patches
{
    internal class AssetsPatch
    {

        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                ModAssets.CreateLinePrefab();
            }
        }
    }
}
