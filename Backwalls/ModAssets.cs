using System.Collections.Generic;
using UnityEngine;

namespace Backwalls
{
    internal class ModAssets
    {
        public static GameObject wallSidescreenPrefab;
        public static GameObject swatchSidescreenPrefab;
        public static Dictionary<string, Sprite> uiSprites;

        public static void LoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("backwallassets");
            wallSidescreenPrefab = bundle.LoadAsset<GameObject>("WallSidescreen");
            swatchSidescreenPrefab = bundle.LoadAsset<GameObject>("SwatchSidescreen");
        }
    }
}
