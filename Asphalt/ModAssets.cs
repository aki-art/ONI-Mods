using FUtility.FUI;
using UnityEngine;

namespace Asphalt
{
    public class ModAssets
    {
        public static string ExternalSettingsPath;
        public static string ModPath;
        public static Texture2D bitumenTexture;
        public static GameObject settingsDialogPrefab;
        public static TMPConverter tmp;

        public static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("at_uiasset");
            settingsDialogPrefab = bundle.LoadAsset<GameObject>("AsphaltSettingsDialog");
            tmp = new TMPConverter();
            tmp.ReplaceAllText(settingsDialogPrefab);
        }
    }
}
