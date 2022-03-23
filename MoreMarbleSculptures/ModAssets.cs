using UnityEngine;

namespace MoreMarbleSculptures
{
    public class ModAssets
    {
        public class Prefabs
        {
            public static GameObject colorPickerSidescreen;
        }

        public static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("moremarblesassets");
            Prefabs.colorPickerSidescreen = bundle.LoadAsset<GameObject>("PaintableSideScreen");
        }
    }
}
