using UnityEngine;

namespace CrittersDropBones.Items
{
    public class LargeBoneConfig : IEntityConfig
    {
        public static string ID = "CDB_LargeBone";

        public GameObject CreatePrefab()
        {
            return FEntityTemplates.CreateBone(
                ID,
                STRINGS.ITEMS.BONES.CDB_LARGEBONE.NAME,
                STRINGS.ITEMS.BONES.CDB_LARGEBONE.DESC,
                2f,
                "cdb_largebone_kanim",
                1.2f,
                0.7f
                );
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}