using UnityEngine;

namespace ZipLine
{
    public class ModAssets
    {
        public static GameObject linePrefab;
        public static GameObject ghostLinePrefab;

        public static void LoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("ziplineassets");

            linePrefab = bundle.LoadAsset<GameObject>("Assets/prefabs/zipRopePrefab.prefab");
            linePrefab.SetActive(false);

            ghostLinePrefab = bundle.LoadAsset<GameObject>("Assets/prefabs/zipRopeGhostPrefab.prefab");
            ghostLinePrefab.SetActive(false);
        }
    }
}