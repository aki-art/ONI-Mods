using FUtility;
using FUtility.FUI;
using UnityEngine;

namespace TrueTiles
{
    public class ModAssets
    {
        public static class Prefabs
        {
            public static GameObject settingsDialog;
        }

        public static class Tags
        {
            public static readonly Tag texturedTile = TagManager.Create("truetiles_texturedTile");
        }

        public static void LateLoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("truetilesassets");

            Prefabs.settingsDialog = bundle.LoadAsset<GameObject>("SettingsDialog");
            new TMPConverter().ReplaceAllText(Prefabs.settingsDialog);
        }
    }
}
