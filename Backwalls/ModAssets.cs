using UnityEngine;

namespace Backwalls
{
    internal class ModAssets
    {
        public static GameObject wallSidescreenPrefab;

        public static void LoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("backwallassets");
            wallSidescreenPrefab = bundle.LoadAsset<GameObject>("WallSidescreen");
        }
    }
}
