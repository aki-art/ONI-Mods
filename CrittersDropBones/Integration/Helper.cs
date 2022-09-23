using UnityEngine;

namespace CrittersDropBones.Integration
{
    public class Helper
    {
        public static void RegisterEntity(GameObject prefab)
        {
            Assets.AddPrefab(prefab.GetComponent<KPrefabID>());
        }
    }
}
