using FUtility;
using FUtility.FUI;
using UnityEngine;

namespace TrueTiles
{
    internal class ModAssets
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
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("truetilesassets");

            Prefabs.settingsDialog = bundle.LoadAsset<GameObject>("SettingsDialog");
            TMPConverter tmp = new TMPConverter();
            tmp.ReplaceAllText(Prefabs.settingsDialog);

            Log.Debuglog("SCREEN PREFAB LOADED");
            Log.Assert("rpefab", Prefabs.settingsDialog);
        }
    }
}
