using FUtility.FUI;
using UnityEngine;

namespace Bestagons
{
    public class ModAssets
    {
        public static GameObject purchasableHexPrefab;

        public static void LoadAssets()
        {
            var bundle = FAssets.LoadAssetBundle("bestagonsassets");
            purchasableHexPrefab = bundle.LoadAsset<GameObject>("Assets/Bestagons/Hexagon_tmpconverted.prefab");
            TMPConverter.ReplaceAllText(purchasableHexPrefab);
        }
    }
}
