using System.Collections.Generic;
using UnityEngine;
using FUtility;

namespace Dowsing
{
    public static class ModAssets
    {
        public static List<Tag> geyserElements;
        public static Tag GeyserRenewable = TagManager.Create("AD_geyserRenewable", "Geyser Renewable");
        public static GameObject sideScreenPrefab; 
        
        internal static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("a_d_uiassets");
            sideScreenPrefab = bundle.LoadAsset<GameObject>("DowserSideScreen");
        }
    }
}
