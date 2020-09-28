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
            sideScreenPrefab = FUtility.Assets.LoadUIPrefab("a_d_uiassets", "DowserSideScreen");
            FUtility.FUI.Helper.ReplaceAllText(ref sideScreenPrefab);
        }
    }
}
