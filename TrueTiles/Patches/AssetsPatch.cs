using FUtility;
using HarmonyLib;

namespace TrueTiles.Patches
{
    public class AssetsPatch
    {
        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public static class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                Log.Debuglog("---------------------- ASSETS POST");
                TileAssetLoader.Instance.LoadOverrides();
            }
        }
    }
}
