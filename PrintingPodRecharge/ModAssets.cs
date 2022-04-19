using FUtility.FUI;
using Klei.AI;
using UnityEngine;

namespace PrintingPodRecharge
{
    public class ModAssets
    {
        public static class Tags
        {
            public static Tag bioInk = TagManager.Create("ppr_bioink");
        }

        public static class Traits
        {
            public static Trait grantRandomTrait;
        }

        public static class Prefabs
        {
            public static GameObject bioInkSideScreen;
        }

        public static class StatusItems
        {
            public static StatusItem printReady;
        }

        public static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("pprechargeassets");

            Prefabs.bioInkSideScreen = bundle.LoadAsset<GameObject>("BioInkSidescreen");
            TMPConverter tmp = new TMPConverter();
            tmp.ReplaceAllText(Prefabs.bioInkSideScreen);

            StatusItems.printReady = new StatusItem(
                "ppr_printready",
                "BUILDINGS.",
                "status_item_doubleexclamation",
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID);
        }
    }
}
