using UnityEngine;

namespace CrittersDropBones.Items
{
    public class FishBoneConfig : IEntityConfig
    {
        public static string ID = "CDB_FishBone";

        public GameObject CreatePrefab()
        {
            return FEntityTemplates.CreateBone(
                ID,
                STRINGS.ITEMS.BONES.CDB_FISHBONE.NAME,
                STRINGS.ITEMS.BONES.CDB_FISHBONE.DESC,
                0.5f,
                "cdb_fishbone_kanim",
                0.8f,
                0.6f
                );
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}