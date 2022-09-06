using FUtility;
using UnityEngine;

namespace ZipLine
{
    public class ModAssets
    {
        public static GameObject linePrefab;
        public static GameObject ghostLinePrefab;

        public static Texture2D ropeTex;
        public static Texture2D ghostTex;

        public static void LoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("ziplineassets");

            linePrefab = bundle.LoadAsset<GameObject>("Assets/prefabs/zipRopePrefab.prefab");
            ropeTex = linePrefab.GetComponent<LineRenderer>().material.mainTexture as Texture2D;
            Log.Assert("ropetex", ropeTex);
            linePrefab.SetActive(false);

            ghostLinePrefab = bundle.LoadAsset<GameObject>("Assets/prefabs/zipRopeGhostPrefab.prefab");
            ghostTex = ghostLinePrefab.GetComponent<LineRenderer>().material.mainTexture as Texture2D;
            ghostLinePrefab.SetActive(false);
        }
    }
}