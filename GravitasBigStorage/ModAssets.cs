using FUtility.FUI;
using UnityEngine;

namespace GravitasBigStorage
{
    public class ModAssets
    {
        public static GameObject analyzableSidescreenPrefab;

        public static void LoadAll()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("akis_universal_sidesceen_v1", platformSpecific: true);
            analyzableSidescreenPrefab = bundle.LoadAsset<GameObject>("Assets/UniversalSidescreen_tmpconverted.prefab");

            new TMPConverter().ReplaceAllText(analyzableSidescreenPrefab);
        }
    }
}
