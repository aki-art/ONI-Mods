using FUtility.FUI;
using UnityEngine;

namespace PrintingPodRecharge
{
    public class ModAssets
    {
        public static class Tags
        {
            public static Tag bioInk = TagManager.Create("ppr_bioink");

        }
        public static class Prefabs
        {
            public static GameObject bioInkSideScreen;
        }

        public static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("pprechargeassets");

            Prefabs.bioInkSideScreen = bundle.LoadAsset<GameObject>("BioInkSidescreen");
            TMPConverter tmp = new TMPConverter();
            tmp.ReplaceAllText(Prefabs.bioInkSideScreen);
        }
    }
}
