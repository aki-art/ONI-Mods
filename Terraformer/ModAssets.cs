using FUtility.FUI;
using UnityEngine;

namespace Terraformer
{
    public class ModAssets
    {
        public static class Prefabs
        {
            public static GameObject confirmScreen;
            public static GameObject detonationSelectorScreen;
        }

        public static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("terraformerassets");

            var dialogs = bundle.LoadAsset<GameObject>("Dialogs").transform;

            Prefabs.confirmScreen = dialogs.Find("DetonationConfirmDialog").gameObject;
            Prefabs.detonationSelectorScreen = dialogs.Find("ResultDialog").gameObject;

            TMPConverter tmp = new TMPConverter();
            tmp.ReplaceAllText(Prefabs.confirmScreen);
            tmp.ReplaceAllText(Prefabs.detonationSelectorScreen);
        }
    }
}
