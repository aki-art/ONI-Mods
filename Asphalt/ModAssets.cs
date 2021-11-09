using FUtility.FUI;
using UnityEngine;

namespace Asphalt
{
    class ModAssets
    {
        public static Texture2D bitumenTexture;

        public class Prefabs
        {
            public static GameObject settingsDialog;
        }

        public class Colors
        {
            public static readonly Color bitumen = new Color32(65, 65, 79, 255);
        }

        public static void LoadAssets()
        {
            bitumenTexture = FUtility.Assets.LoadTexture("solid_bitumen");
        }


        public static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("asphaltassets");

            Prefabs.settingsDialog = bundle.LoadAsset<GameObject>("SettingsDialog");
            TMPConverter tmp = new TMPConverter();
            tmp.ReplaceAllText(Prefabs.settingsDialog);
        }
    }
}
